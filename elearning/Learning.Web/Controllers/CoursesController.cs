using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Learning.Data;
using Learning.Data.Entity;
using Learning.Web.Models;

namespace Learning.Web.Controllers
{
    public class CoursesController : BaseApiController
    {
        public CoursesController(ILearningRepository repo) : base(repo) { }
        public IHttpActionResult Get()
        {
            IQueryable<Course> query;
            query = TheRepository.GetAllCourses();
            var result = query.ToList().Select(s => TheModelFactory.Create(s));
            return Ok(result);
        }
        public IHttpActionResult GetCourse(int id)
        {
            try
            {
                var course = TheRepository.GetCourse(id);
                if (course != null)
                {
                    //return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(course));
                    return Ok(TheModelFactory.Create(course));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        public object Get(int page , int pageSize)
    {
        IQueryable<Course> query;
 
        query = TheRepository.GetAllCourses().OrderBy(c => c.Name);
 
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
 
        var urlHelper = new UrlHelper(Request);
        var prevLink = page > 0 ? urlHelper.Link("Courses", new { page = page - 1, pageSize = pageSize }) : "";
        var nextLink = page < totalPages - 1 ? urlHelper.Link("Courses", new { page = page + 1, pageSize = pageSize }) : "";
 
        var paginationHeader = new
        {
            TotalCount = totalCount,
            TotalPages = totalPages,
            PrevPageLink = prevLink,
            NextPageLink = nextLink
        };
 
        System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",
        Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
 
        var results = query
        .Skip(pageSize * page)
        .Take(pageSize)
        .ToList()
        .Select(s => TheModelFactory.Create(s));

        return results;
    }
        public IHttpActionResult Post([FromBody] CourseModel courseModel)
        {
            try
            {
                var entity = TheModelFactory.Parse(courseModel);
                if (entity == null) return BadRequest("Could not read subject/tutor from body");
                if (TheRepository.Insert(entity) && TheRepository.SaveAll())
                {
                    var newCourseModel = TheModelFactory.Create(entity);
                    return Created<CourseModel>(new Uri(newCourseModel.Url), newCourseModel);
                }
                else
                {
                    return BadRequest("Could not save to database");
                }
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [HttpPatch]
        public IHttpActionResult Put(int id, [FromBody] CourseModel courseModel)
        {
            try
            {
                var updatedCourse = TheModelFactory.Parse(courseModel);

                if (updatedCourse == null) BadRequest("Could not read subject/tutor from body");

                var originalCourse = TheRepository.GetCourse(id);

                if (originalCourse == null || originalCourse.Id != id)
                {
                    return Content<string>(HttpStatusCode.NotModified, "Course is not found");
                }
                else
                {
                    updatedCourse.Id = id;
                }
                if (TheRepository.SaveAll())//TheRepository.Update(originalCourse, updatedCourse) &&
                {
                    return Ok(TheModelFactory.Create(updatedCourse));
                }
                else
                {
                    return Content<string>(HttpStatusCode.NotModified, "");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return BadRequest(e.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
    }
}

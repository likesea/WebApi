using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using Learning.Data.Entity;

namespace Learning.Web.Models
{
    public class ModelFactory
    {
        private System.Web.Http.Routing.UrlHelper _urlHelper;
        //public ModelFactory()
        //{

        //}

        public ModelFactory(HttpRequestMessage request)
        {
            _urlHelper = new UrlHelper(request);
        }
        public CourseModel Create(Course course)
        {
            return new CourseModel()
            {
                Id = course.Id,
                Url = _urlHelper.Link("courses",new {id= course.Id}),
                Name = course.Name,
                Duration = course.Duration,
                Description = course.Description,
                Tutor = Create(course.CourseTutor),
                Subject = Create(course.CourseSubject)
            };
        }

        public TutorModel Create(Tutor tutor)
        {
            return new TutorModel()
            {
                Id = tutor.Id,
                Email = tutor.Email,
                UserName = tutor.UserName,
                FirstName = tutor.FirstName,
                LastName = tutor.LastName,
                Gender = tutor.Gender
            };
        }

        public SubjectModel Create(Subject subject)
        {
            return new SubjectModel()
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }

        public EnrollmentModel Create(Enrollment enrollment)
        {
            return new EnrollmentModel()
            {
                EnrollmentDate = enrollment.EnrollmentDate,
                Course = Create(enrollment.Course)
            };
        }
    }
}
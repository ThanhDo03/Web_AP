using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class AssignTraineeCourseController : Controller
    {
        private readonly TranningDBContext _dbContext;
        private readonly ILogger<AssignTraineeCourseController> _logger;

        public AssignTraineeCourseController(TranningDBContext context, ILogger<AssignTraineeCourseController> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Trainee_courseModel trainee_courseModel = new Trainee_courseModel();
            trainee_courseModel.Trainee_courseDetailLists = new List<Trainee_courseModelDetail>();

            var data = from m in _dbContext.Trainee_course select m;

            data = data.Where(m => m.deleted_at == null);
            var dataList = data.ToList();

            foreach (var item in dataList)
            {
                var dataT = _dbContext.Users.Where(m => m.id == item.trainee_id && m.deleted_at == null).FirstOrDefault();
                var dataC = _dbContext.Courses.Where(m => m.id == item.course_id && m.deleted_at == null).FirstOrDefault();

                trainee_courseModel.Trainee_courseDetailLists.Add(new Trainee_courseModelDetail
                {
                    //id = item.id,
                    trainee_id = item.trainee_id,
                    traineeName = dataT.full_name,
                    course_id = item.course_id,
                    courseName = dataC.name,
                    status = item.status,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            }
            return View(trainee_courseModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var traineeList = _dbContext.Users
                    .Where(u => u.deleted_at == null && u.role_id == 4)
                    .Select(u => new SelectListItem { Value = u.id.ToString(), Text = u.full_name })
                    .ToList();

                var courseList = _dbContext.Courses
                    .Where(m => m.deleted_at == null)
                    .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name })
                    .ToList();

                ViewBag.Users = traineeList;  // Corrected ViewBag name
                ViewBag.Courses = courseList;

                Trainee_courseModelDetail trainee_course = new Trainee_courseModelDetail();
                return View(trainee_course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data for Trainee Course form.");
                // Handle the error appropriately, maybe redirect to an error page
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Trainee_courseModelDetail trainee_course)
        {
            try
            {
                if (ModelState.IsValid && trainee_course != null)
                {
                    var trainee_courseData = new Trainee_course()
                    {
                        trainee_id = trainee_course.trainee_id,
                        course_id = trainee_course.course_id,
                        status = trainee_course.status,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    _dbContext.Trainee_course.Add(trainee_courseData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding Trainee Course.");
                TempData["saveStatus"] = false;
                TempData["errorMessage"] = ex.Message; // Add this line to display the error message
            }

            var courseList = _dbContext.Courses
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name })
                .ToList();

            var userList = _dbContext.Users
                .Where(u => u.deleted_at == null && u.role_id == 4)
                .Select(u => new SelectListItem { Value = u.id.ToString(), Text = u.full_name })
                .ToList();

            ViewBag.Courses = courseList;
            ViewBag.Users = userList;

            return View(trainee_course);
        }
    }
}

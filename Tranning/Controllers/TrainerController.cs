using Microsoft.AspNetCore.Mvc;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ILogger<TrainerController> _logger;
        private readonly TranningDBContext _dbContext;

        public TrainerController(ILogger<TrainerController> logger, TranningDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }


        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            CategoryModel categoryModel = new CategoryModel();
            categoryModel.CategoryDetailLists = new List<CategoryDetail>();


            var data = from m in _dbContext.Categories select m;

            data = data.Where(m => m.deleted_at == null);
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.name.Contains(SearchString));
            }
            var categoryList = data.ToList();

            foreach (var item in categoryList)
            {

                categoryModel.CategoryDetailLists.Add(new CategoryDetail
                {
                    id = item.id,
                    name = item.name,
                    icon = item.icon,
                    description = item.description,
                });
            }
            ViewData["CurrentFilter"] = SearchString;
            return View(categoryModel);
        }

        [HttpGet]
        public IActionResult CategoryDetail(int id = 0)
        {
            CourseModel course = new CourseModel();
            course.CourseDetailLists = new List<CourseDetail>();
            var data = _dbContext.Categories.Where(m => m.id == id).FirstOrDefault();
            var dataC = from m in _dbContext.Courses select m;
            dataC = dataC.Where(m => m.category_id == id && m.deleted_at == null);
            if (dataC != null)
            {
                var dataList = dataC.ToList();
                foreach (var item in dataList)
                {
                    course.CourseDetailLists.Add(new CourseDetail
                    {
                        id = item.id,
                        name = item.name,
                        description = item.description,
                        start_date = item.start_date,
                        end_date = item.end_date,
                        avatar = item.avatar,
                    });
                }
                ViewBag.Stores = data;
                return View(course);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CourseDetail(int id = 0)
        {
            TopicModel topic = new TopicModel();
            topic.TopicDetailLists = new List<TopicDetail>();
            var data = _dbContext.Courses.Where(m => m.id == id).FirstOrDefault();
            var dataC = from m in _dbContext.Topics select m;
            dataC = dataC.Where(m => m.course_id == id && m.deleted_at == null);
            if (dataC != null)
            {
                var dataList = dataC.ToList();
                foreach (var item in dataList)
                {
                    topic.TopicDetailLists.Add(new TopicDetail
                    {
                        id = item.id,
                        name = item.name,
                        description = item.description,
                    });
                }
                ViewBag.Stores = data;
                return View(topic);
            }
            return View();
        }
        [HttpGet]
        public IActionResult TopicDetail(int id = 0)
        {
            TopicDetail topic = new TopicDetail();
            var data = _dbContext.Topics.Where(m => m.id == id && m.deleted_at == null).FirstOrDefault();
            topic.id = data.id;
            topic.name = data.name;
            topic.course_id = data.course_id;
            topic.description = data.description;
            topic.videos = data.videos;
            topic.documents = data.documents;
            topic.attach_file = data.attach_file;
            topic.status = data.status;
            return View(topic);
        }   
    }
}

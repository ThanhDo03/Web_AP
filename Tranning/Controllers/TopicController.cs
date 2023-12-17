using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class TopicController : Controller
    {
        private readonly TranningDBContext _dbContext;

        public TopicController(TranningDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            var data = _dbContext.Topics
                .Where(m => m.deleted_at == null)
                .Select(item => new TopicDetail
                {
                    id = item.id,
                    course_id = item.course_id,
                    name = item.name,
                    description = item.description,
                    documents = item.documents,
                    videos = item.videos,
                    attach_file = item.attach_file,
                    status = item.status,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                })
                .ToList();

            foreach (var topic in data)
            {
                var course = _dbContext.Courses.FirstOrDefault(c => c.id == topic.course_id);
                if (course != null)
                {
                    topic.courseName = course.name;
                }
            }
            // Apply additional search filter if needed
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.name.Contains(SearchString) || m.description.Contains(SearchString)).ToList();
            }

            TopicModel topicModel = new TopicModel();
            topicModel.TopicDetailLists = data;
            return View(topicModel);
        }

        //public IActionResult Index(string SearchString)
        //{
        //    TopicModel topicModel = new TopicModel();
        //    topicModel.TopicDetailLists = new List<TopicDetail>();

        //    var data = from m in _dbContext.Topics select m;

        //    data = data.Where(m => m.deleted_at == null);
        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        data = data.Where(m => m.name.Contains(SearchString) || m.description.Contains(SearchString));
        //    }
        //    var dataList = data.ToList();


        //    foreach (var item in dataList)
        //    {
        //        var dataTo = _dbContext.Topics.Where(m => m.id == item.course_id && m.deleted_at == null).FirstOrDefault();
        //        topicModel.TopicDetailLists.Add(new TopicDetail
        //        {
        //            id = item.id,
        //            course_id = item.course_id,
        //            courseName = dataTo.name,
        //            name = item.name,
        //            description = item.description,
        //            videos = item.videos,
        //            documents = item.documents,
        //            attach_file = item.attach_file,
        //            status = item.status,
        //            created_at = item.created_at,
        //            updated_at = item.updated_at
        //        });
        //    }
        //    ViewData["CurrentFilter"] = SearchString;
        //    return View(topicModel);
        //}

        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();
            var courseList = _dbContext.Courses
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = courseList;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic)
        {
            ViewBag.Course = _dbContext.Courses.ToList();
            try
            {
                string fileAttach_file = UploadFile(topic.fileAttach_file);
                string fileVideos = UploadVideos(topic.fileVideos);
                string fileDocument = string.Empty;
                if (topic.fileDocuments != null)
                {
                    fileDocument = UploadFile(topic.fileDocuments);
                }
                var topicData = new Topic()
                {
                    course_id = topic.course_id,
                    name = topic.name,
                    description = topic.description,
                    videos = fileVideos,
                    status = topic.status,
                    documents = fileDocument,
                    attach_file = fileAttach_file,
                    created_at = DateTime.Now
                };
                _dbContext.Topics.Add(topicData);
                _dbContext.SaveChanges(true);
                TempData["saveStatus"] = true;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra.Vui lòng thử lại";
                return View(topic);
            }
            return RedirectToAction(nameof(TopicController.Index), "Topic");
        }
        private string UploadFile(IFormFile file)
        {
            string filePath = string.Empty;
            try
            {
                if (file != null)
                {
                    string pathUploadImage = "wwwroot\\uploads\\images";
                    string fileName = file.FileName;
                    fileName = Path.GetFileName(fileName);
                    string uniqueStr = Guid.NewGuid().ToString();
                    fileName = uniqueStr + "-" + fileName;
                    if (!Directory.Exists(pathUploadImage))
                    {
                        Directory.CreateDirectory(pathUploadImage);
                    }
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadImage, fileName);
                    var stream = new FileStream(uploadPath, FileMode.Create);
                    file.CopyToAsync(stream);
                    filePath = fileName;
                }
            }
            catch (Exception ex)
            {

            }
            return filePath;
        }
        private string UploadVideos(IFormFile file)
        {
            string filePath = string.Empty;
            try
            {
                if (file != null)
                {
                    string pathUploadVideos = "wwwroot\\uploads\\videos";
                    string fileName = file.FileName;
                    fileName = Path.GetFileName(fileName);
                    string uniqueStr = Guid.NewGuid().ToString();
                    fileName = uniqueStr + "-" + fileName;
                    if (!Directory.Exists(pathUploadVideos))
                    {
                        Directory.CreateDirectory(pathUploadVideos);
                    }
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadVideos, fileName);
                    var stream = new FileStream(uploadPath, FileMode.Create);
                    file.CopyToAsync(stream);
                    filePath = fileName;
                }
            }
            catch (Exception ex)
            {

            }
            return filePath;
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.Topics.Where(m => m.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.deleted_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = true;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteStatus"] = false;
                //return Ok(ex.Message);
            }
            return RedirectToAction(nameof(TopicController.Index), "Topic");
        }
        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            var topicDetail = new TopicDetail();
            var topic = _dbContext.Topics.FirstOrDefault(m => m.id == id && m.deleted_at == null);
            if (topic != null)
            {
                topicDetail.id = topic.id;
                topicDetail.name = topic.name;
                topicDetail.course_id = topic.course_id;
                topicDetail.description = topic.description;
                topicDetail.videos = topic.videos;
                topicDetail.attach_file = topic.attach_file;
                topicDetail.status = topic.status;
                topicDetail.documents = topic.documents;
            }

            var courseList = _dbContext.Courses
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name })
                .ToList();
            ViewBag.Stores = courseList;

            return View(topicDetail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(TopicDetail topic, IFormFile fileVideos, IFormFile fileDocuments, IFormFile fileAttach_file)
        {
            try
            {
                var data = _dbContext.Topics.Where(m => m.id == topic.id).FirstOrDefault();
                string uniqueDocument = "";
                string uniqueAttachFile = "";
                string uniqueVideo = "";
                if (topic.fileDocuments != null)
                {
                    uniqueDocument = uniqueDocument = UploadFile(topic.fileDocuments);
                }
                if (topic.fileAttach_file != null)
                {
                    uniqueAttachFile = uniqueAttachFile = UploadFile(topic.fileAttach_file);
                }
                if(topic.fileVideos != null)
                {
                    uniqueVideo = uniqueVideo = UploadVideos(topic.fileVideos);
                }
                if (data != null)
                {
                    // gan lai du lieu trong db bang du lieu tu form model gui len
                    data.name = topic.name;
                    data.course_id = topic.course_id;
                    data.description = topic.description;
                    //data.videos = topic.videos;
                    data.status = topic.status;
                    if (!string.IsNullOrEmpty(uniqueDocument))
                    {
                        data.documents = uniqueDocument;
                    }
                    if (!string.IsNullOrEmpty(uniqueAttachFile))
                    {
                        data.attach_file = uniqueAttachFile;
                    }
                    if (!string.IsNullOrEmpty(uniqueVideo))
                    {
                        data.videos = uniqueVideo;
                    }
                    data.updated_at = DateTime.Now;
                    _dbContext.SaveChanges(true);
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateStatus"] = false;
                // Xử lý exception (ghi log, thông báo lỗi)
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

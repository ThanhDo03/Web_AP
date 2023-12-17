using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tranning.DataDBContext;
using Tranning.Models;


namespace Tranning.Controllers
{
    public class AssignTrainerTopicController : Controller
    {
        private readonly TranningDBContext _dbContext;
        private readonly ILogger<AssignTrainerTopicController> _logger;

        public AssignTrainerTopicController(TranningDBContext context, ILogger<AssignTrainerTopicController> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Trainer_topicModel tt = new Trainer_topicModel();
            tt.Trainer_topicDetailLists = new List<Trainer_topicModelDetail>();

            var data = from m in _dbContext.Trainer_topic select m;

            data = data.Where(m => m.deleted_at == null);
            var dataList = data.ToList();

            foreach (var item in dataList)
            {
                var dataTr = _dbContext.Users.Where(m => m.id == item.trainer_id && m.deleted_at == null).FirstOrDefault();
                var dataTo = _dbContext.Topics.Where(m => m.id == item.topic_id && m.deleted_at == null).FirstOrDefault();

                tt.Trainer_topicDetailLists.Add(new Trainer_topicModelDetail
                {
                    //id = item.id,
                    trainer_id = item.trainer_id,
                    trainerName = dataTr.full_name,
                    topic_id = item.topic_id,
                    topicName = dataTo.name,
                    status = item.status,
                    created_at = item.created_at,
                    updated_at = item.updated_at
                });
            };
            return View(tt);
        }


        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var trainerList = _dbContext.Users
                    .Where(u => u.deleted_at == null && u.role_id == 3)
                    .Select(u => new SelectListItem { Value = u.id.ToString(), Text = u.full_name })
                    .ToList();

                var topicList = _dbContext.Topics
                    .Where(m => m.deleted_at == null)
                    .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name })
                    .ToList();

                ViewBag.Users = trainerList;  // Corrected ViewBag name
                ViewBag.Topics = topicList;

                Trainer_topicModelDetail trainer_topic = new Trainer_topicModelDetail();
                return View(trainer_topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data for Trainer Topic form.");
                // Handle the error appropriately, maybe redirect to an error page
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Trainer_topicModelDetail trainer_topic)
        {
            try
            {
                if (ModelState.IsValid && trainer_topic != null)
                {
                    var trainertopicData = new Trainer_topic()
                    {
                        trainer_id = trainer_topic.trainer_id,
                        topic_id = trainer_topic.topic_id,
                        status = trainer_topic.status,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    _dbContext.Trainer_topic.Add(trainertopicData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;

                    return RedirectToAction(nameof(Index));
                }
                else // Log model state errors
                {
                    foreach (var modelStateKey in ModelState.Keys)
                    {
                        var errors = ModelState[modelStateKey].Errors;
                        foreach (var error in errors)
                        {
                            _logger.LogError($"Model state error in key {modelStateKey}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle database-related exceptions
                // ...
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // ...
            }

            // Populate dropdown lists in case of failure
            var trainer_topicList = _dbContext.Topics
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name })
                .ToList();

            var userList = _dbContext.Users
                .Where(u => u.deleted_at == null && u.role_id == 3)
                .Select(u => new SelectListItem { Value = u.id.ToString(), Text = u.full_name })
                .ToList();

            ViewBag.Topics = trainer_topicList;  // Corrected ViewBag name
            ViewBag.Users = userList;

            return View(trainer_topic);
        }

    }
}

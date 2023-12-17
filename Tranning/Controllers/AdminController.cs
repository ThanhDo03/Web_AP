using Microsoft.AspNetCore.Mvc;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class AdminController : Controller
    {

        private readonly TranningDBContext _dbContext;
        private readonly ILogger<AdminController> _logger;

        public AdminController(TranningDBContext context, ILogger<AdminController> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            UserModel userModel = new UserModel();
            userModel.UserDetailLists = new List<UserDetail>();

            var data = from m in _dbContext.Users select m;

            data = data.Where(m => m.deleted_at == null);
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.full_name.Contains(SearchString) || m.email.Contains(SearchString));
            }
            data.ToList();

            foreach (var item in data)
            {
                userModel.UserDetailLists.Add(new UserDetail
                {
                    id = item.id,
                    role_id = item.role_id,
                    extra_code = item.extra_code,
                    username = item.username,
                    password = item.password,
                    email = item.email,
                    phone = item.phone,
                    address = item.address,
                    gender = item.gender,
                    birthday = item.birthday,
                    avatar = item.avatar,
                    last_login = item.last_login,
                    last_logout = item.last_logout,
                    status = item.status,
                    created_at = DateTime.Now,
                    full_name = item.full_name,
                    education = item.education,
                    programming_laguague = item.programming_laguague,
                    toeic_score = item.toeic_score,
                    experience = item.experience,
                    department = item.department,
                });
            }
            ViewData["CurrentFilter"] = SearchString;
            return View(userModel);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            UserDetail user = new UserDetail();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UserDetail user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userData = new Users()
                    {
                        role_id = user.role_id,
                        extra_code = user.extra_code,
                        username = user.username,
                        password = user.password,
                        email = user.email,
                        phone = user.phone,
                        address = user.address,
                        gender = user.gender,
                        birthday = user.birthday,
                        avatar = user.avatar,
                        last_login = user.last_login,
                        last_logout = user.last_logout,
                        status = user.status,
                        full_name = user.full_name,
                        education = user.education,
                        programming_laguague = user.programming_laguague,
                        toeic_score = user.toeic_score,
                        experience = user.experience,
                        department = user.department,
                        created_at = DateTime.Now,
                    };

                    _dbContext.Users.Add(userData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(AdminController.Index), "Admin");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            UserDetail user = new UserDetail();
            var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();

            if (data != null)
            {
                user.role_id = data.role_id;
                user.extra_code = data.extra_code;
                user.username = data.username;
                user.password = data.password;
                user.email = data.email;
                user.phone = data.phone;
                user.address = data.address;
                user.gender = data.gender;
                user.birthday = data.birthday;
                user.avatar = data.avatar;
                user.last_login = data.last_login;
                user.last_logout = data.last_logout;
                user.status = data.status;
                user.full_name = data.full_name;
                user.education = data.education;
                user.programming_laguague = data.programming_laguague;
                user.toeic_score = data.toeic_score;
                user.experience = data.experience;
                user.department = data.department;
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(UserDetail user)
        {
            try
            {
                var data = _dbContext.Users.Where(m => m.id == user.id).FirstOrDefault();
                if (data != null)
                {
                    data.role_id = user.role_id;
                    data.extra_code = user.extra_code;
                    data.username = user.username;
                    data.password = user.password;
                    data.email = user.email;
                    data.phone = user.phone;
                    data.address = user.address;
                    data.gender = user.gender;
                    data.birthday = user.birthday;
                    data.avatar = user.avatar;
                    data.last_login = user.last_login;
                    data.last_logout = user.last_logout;
                    data.status = user.status;
                    data.full_name = user.full_name;
                    data.education = user.education;
                    data.programming_laguague = user.programming_laguague;
                    data.toeic_score = user.toeic_score;
                    data.experience = user.experience;
                    data.department = user.department;
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
            }
            return RedirectToAction(nameof(AdminController.Index), "Admin");
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.Users.Where(m => m.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.deleted_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    _dbContext.SaveChanges(true);
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch
            {
                TempData["DeleteStatus"] = false;
            }
            return RedirectToAction(nameof(AdminController.Index), "Admin");
        }
    }
}
    


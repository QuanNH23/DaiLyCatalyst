using HealMe.Controllers.Helper;
using HealMe.Services;
using HealMe.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealMe.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        public TaskController(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy danh sách nhiệm vụ của người dùng từ dịch vụ
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var tasks = await _taskService.GetTasksByUserIdAsync(userId);
                var tasksCompleted = await _taskService.GetTasksCompletedByUserId(userId);
                var vm = new TaskUserVM
                {
                    UserId = userId,
                    TaskHaveAdded = tasks,
                    TaskHaveCompleted = tasksCompleted
                };
                return View("~/Views/Task/HealMeVer2.2.cshtml", vm);
            }
            catch(Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi";
                TempData["ToastMessage"] = "Đã xảy ra lỗi khi tải nhiệm vụ. Vui lòng thử lại sau.";
                return View("~/Views/Task/HealMeVer2.2.cshtml");
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(string taskTitle, string taskNote, IFormFile taskImage)
        {
            try
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var imagePath = await FileHelper.SaveImageAsync(taskImage, HttpContext.RequestServices.GetService<IWebHostEnvironment>(), "images");
                var newTask = new Models.Task
                {
                    UserId = userId,
                    Title = taskTitle,
                    Note = taskNote,
                    Completed = false,
                    ImageUrl = imagePath,
                    CreatedAt = DateTime.Now,
                };
                await _taskService.CreateTaskAsync(newTask);
                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Thành công";
                TempData["ToastMessage"] = "Nhiệm vụ đã được tạo thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi";
                TempData["ToastMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminTask()
        {
            try
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var user = await _userService.GetUserByIdAsync(userId);
                if (user.Role != "admin")
                {
                    return RedirectToAction("Login", "Authentication");
                }
                var dto = await _taskService.GetAdminTask();

                return View("~/Views/Task/TaskAdmin.cshtml", dto);
            }
            catch (Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi";
                TempData["ToastMessage"] = ex.Message;
                return View("~/Views/Task/TaskAdmin.cshtml");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUserTasks(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                var tasksRes = await _taskService.GetTasksByUserIdAsync(userId);
                if (user.Role != "admin")
                {
                    return RedirectToAction("Login", "Authentication");
                }
                return Json(new
                {
                        userId = user.UserId,
                        username = user.Username,
                        tasks = tasksRes.Select(task => new
                        {
                            taskId = task.TaskId,
                            title = task.Title,
                            note = task.Note,
                            completed = task.Completed,
                            imageUrl = task.ImageUrl,

                        })
                    
                });
            }
            catch (Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Lỗi";
                TempData["ToastMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] TaskStatusUpdateRequestVM request)
        {
            try
            {
                var taskFind = await _taskService.GetTaskByIdAsync(request.TaskId);
                if (taskFind == null)
                {
                    return Json(new { success = false, message = "Nhiệm vụ không tồn tại." });
                }
                taskFind.Completed = request.Status;
                await _taskService.UpdateTaskAsync(taskFind);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

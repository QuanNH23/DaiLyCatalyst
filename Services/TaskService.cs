using HealMe.DAO;
using HealMe.DTO;

namespace HealMe.Services
{
    public interface ITaskService
    {
        public Task<Models.Task> GetTaskByIdAsync(int taskId);
        public Task<List<Models.Task>> GetTasksByUserIdAsync(int userId);
        public Task CreateTaskAsync(Models.Task task);
        public Task UpdateTaskAsync(Models.Task task);
        public Task<List<Models.Task>> GetTasksCompletedByUserId(int userId);
        public Task<int> GetTaskCountCompletedByUserId(int userId);
        public Task<int> GetTaskCountUnCompletedByUserId(int userId);

        public Task<List<AdminTaskDTO>> GetAdminTask();
    }
    public class TaskService : ITaskService
    {
        private readonly ITaskDAO _taskDAO;
        private readonly IUserDAO _userDAO;

        public TaskService(ITaskDAO taskDAO, IUserDAO userDAO)
        {
            _taskDAO = taskDAO;
            _userDAO = userDAO;
        }

        public async Task<Models.Task> GetTaskByIdAsync(int taskId)
        {
            try
            {
                return await _taskDAO.GetTaskByIdAsync(taskId);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<List<Models.Task>> GetTasksByUserIdAsync(int userId)
        {
            try
            {
                return await _taskDAO.GetTasksByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy danh sách nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task CreateTaskAsync(Models.Task task)
        {
            try
            {
                await _taskDAO.CreateTaskAsync(task);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi tạo nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task UpdateTaskAsync(Models.Task task)
        {
            try
            {
                await _taskDAO.UpdateTaskAsync(task);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi cập nhật nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public Task<List<Models.Task>> GetTasksCompletedByUserId(int userId)
        {
            try
            {
                return _taskDAO.GetTasksCompletedByUserId(userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetTaskCountCompletedByUserId(int userId)
        {
            try
            {
                return await _taskDAO.GetTaskCountCompletedByUserId(userId);
            }catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetTaskCountUnCompletedByUserId(int userId)
        {
            try
            {
                return await _taskDAO.GetTaskCountUnCompletedByUserId(userId);
            }catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy số lượng nhiệm vụ chưa hoàn thành. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<List<AdminTaskDTO>> GetAdminTask()
        {
            try
            {
                var users = await _userDAO.GetAllUsers();
                var result = new List<AdminTaskDTO>();
                foreach (var user in users)
                {
                    var completed = await _taskDAO.GetTaskCountCompletedByUserId((int)user.UserId);
                    var uncompletedTasks = await _taskDAO.GetTaskCountUnCompletedByUserId((int)user.UserId);

                    result.Add(new AdminTaskDTO
                    {
                        UserId = (int)user.UserId,
                        Username = user.Username,
                        CompletedTasksCount = completed,
                        UncompletedTasksCount = uncompletedTasks
                    });
                }
                return result;
            }catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy danh sách nhiệm vụ quản trị. Vui lòng thử lại sau.", ex);
            }
        }
    }
    
}

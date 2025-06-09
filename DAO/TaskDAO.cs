using HealMe.Models;
using Microsoft.EntityFrameworkCore;

namespace HealMe.DAO
{
    public interface ITaskDAO
    {
        public Task<Models.Task> GetTaskByIdAsync(int taskId);
        public Task<List<Models.Task>> GetTasksByUserIdAsync(int userId);
        public System.Threading.Tasks.Task CreateTaskAsync(Models.Task task);
        public System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task);
        public Task<List<Models.Task>> GetTasksCompletedByUserId(int userId);
        public Task<Int32> GetTaskCountCompletedByUserId(int userId);
        public Task<Int32> GetTaskCountUnCompletedByUserId(int userId);

    }
    public class TaskDAO : ITaskDAO
    {
        private readonly HealMeDbContext _context;
        public TaskDAO(HealMeDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(Models.Task task)
        {
            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi tạo nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<Models.Task> GetTaskByIdAsync(int taskId)
        {
            try
            {
                return await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<int> GetTaskCountCompletedByUserId(int userId)
        {
            try
            {
                var count = await _context.Tasks
                    .Where(t => t.UserId == userId && t.Completed)
                    .CountAsync();
                return count;
            }catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy số lượng nhiệm vụ đã hoàn thành. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<int> GetTaskCountUnCompletedByUserId(int userId)
        {
            try
            {
                var count = await _context.Tasks
                    .Where(t => t.UserId == userId && !t.Completed)
                    .CountAsync();
                return count;
            }catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy số lượng nhiệm vụ chưa hoàn thành. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<List<Models.Task>> GetTasksByUserIdAsync(int userId)
        {
            try
            {
               
                return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy danh sách nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }

        public async Task<List<Models.Task>> GetTasksCompletedByUserId(int userId)
        {
            try
            {
                var completedTasks = await _context.Tasks
                    .Where(t => t.UserId == userId && t.Completed)
                    .ToListAsync();
                return completedTasks;
            }catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy danh sách nhiệm vụ đã hoàn thành. Vui lòng thử lại sau.", ex);
            }
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task)
        {
            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi cập nhật nhiệm vụ. Vui lòng thử lại sau.", ex);
            }
        }
    }
}

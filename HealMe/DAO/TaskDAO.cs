using HealMe.DTO;
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
        Task<TaskPagedResult> GetAllTasksAsync(int page, int pageSize, long? userId, bool? completed);

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

        public async Task<TaskPagedResult> GetAllTasksAsync(int page, int pageSize, long? userId, bool? completed)
        {
            try
            {
                var query = _context.Tasks.AsQueryable();

                if (userId.HasValue)
                {
                    query = query.Where(t => t.UserId == userId.Value);
                }

                if (completed.HasValue)
                {
                    query = query.Where(t => t.Completed == completed.Value);
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var skip = (page - 1) * pageSize;

                var tasks = await query
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip(skip)
                    .Take(pageSize)
                    .Include(t => t.User)
                    .Select(t => new TaskDto
                    {
                        TaskId = t.TaskId,
                        UserId = t.UserId,
                        Title = t.Title,
                        Note = t.Note,
                        ImageUrl = t.ImageUrl,
                        Completed = t.Completed,
                        CreatedAt = t.CreatedAt,
                        CompletedAt = t.CompletedAt,
                        UserName = t.User.Username
                    })
                    .ToListAsync();

                return new TaskPagedResult
                {
                    Tasks = tasks,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasPrevious = page > 1,
                    HasNext = page < totalPages
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy danh sách nhiệm vụ. Vui lòng thử lại sau.", ex);
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

using HealMe.Models;
using Microsoft.EntityFrameworkCore;

namespace HealMe.DAO
{
    public interface IUserDAO
    {
        public Task<User> GetUserByIdAsync(int userId);
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<List<User>> GetAllUsers();
    }
    public class UserDAO : IUserDAO
    {
        private readonly HealMeDbContext _context;

        public UserDAO(HealMeDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }catch(Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy danh sách người dùng. Vui lòng thử lại sau.");
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            }catch(Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi. Vui lòng thử lại sau.");
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }catch(Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi. Vui lòng thử lại sau.");
            }
        }
    }
}

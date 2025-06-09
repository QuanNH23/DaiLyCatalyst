using HealMe.DAO;
using HealMe.Models;

namespace HealMe.Services
{
    public interface IUserService
    {
       public Task<User> GetUserByIdAsync(int userId);
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<User> Login(string username, string password);
        public Task<List<User>> GetAllUser();
    }
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        public UserService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<List<User>> GetAllUser()
        {
            try
            {
                return await _userDAO.GetAllUsers();
            }catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _userDAO.GetUserByIdAsync(userId);
            }catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _userDAO.GetUserByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                var user = await _userDAO.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    throw new Exception("Không tìm thấy người dùng!");
                }
                if (user.PasswordHash != password)
                {
                    throw new Exception("Mật khẩu không đúng!");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

using System.Threading.Tasks;
using testing.Data.Models;

namespace testing.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user, string password);
        Task<User> AuthenticateUserAsync(string username, string password);
        Task<User> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(User user);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> DeleteUserAsync(int userId);
    }
} 
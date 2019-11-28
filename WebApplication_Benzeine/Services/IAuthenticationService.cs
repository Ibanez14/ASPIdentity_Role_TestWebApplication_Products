using System.Threading.Tasks;
using WebApplication_Benzeine.Models.ResponseDTO;

namespace WebApplication_Benzeine.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}







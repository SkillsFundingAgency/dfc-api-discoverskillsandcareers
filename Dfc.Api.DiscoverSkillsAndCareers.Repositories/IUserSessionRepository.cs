using DFC.Api.DiscoverSkillsAndCareers.Models;
using System.Threading.Tasks;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    public interface IUserSessionRepository
    {
        Task<UserSession> GetUserSession(string primaryKey);

        Task CreateUserSession(UserSession userSession);

        Task UpdateUserSession(UserSession userSession);
    }
}
using DFC.Api.DiscoverSkillsAndCareers.Models;
using System.Threading.Tasks;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    public interface IUserSessionRepository
    {
        Task CreateUserSession(UserSession userSession);

        Task<UserSession> GetByIdAsync(string sessionId, string partitionKey);
    }
}
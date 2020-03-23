using DFC.Api.DiscoverSkillsAndCareers.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFC.Api.DiscoverSkillsAndCareers.Repositories
{
    public interface IUserSessionRepository
    {
        Task CreateUserSession(UserSession userSession);

        Task<UserSession> GetAsync(Expression<Func<UserSession, bool>> where);
    }
}
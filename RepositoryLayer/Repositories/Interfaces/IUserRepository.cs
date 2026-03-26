using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<int> Count();
    Task<User?> GetByEmail(string email);
}

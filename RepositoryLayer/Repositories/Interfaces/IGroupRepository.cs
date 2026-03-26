using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IGroupRepository : IBaseRepository<Group>
{
    Task<int> Count();
    Task<List<Group>> GetBySemester(int semesterId);
}

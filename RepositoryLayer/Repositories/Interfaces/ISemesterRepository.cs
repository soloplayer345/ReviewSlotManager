using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface ISemesterRepository : IBaseRepository<Semester>
{
    Task<int> Count();
    Task<Semester?> GetActiveSemester();
}

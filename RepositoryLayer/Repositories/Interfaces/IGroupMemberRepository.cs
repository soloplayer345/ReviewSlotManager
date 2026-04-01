using RepositoryLayer.Entities;

namespace RepositoryLayer.Repositories.Interfaces;

public interface IGroupMemberRepository : IBaseRepository<GroupMember>
{
    Task<int> Count();
    Task<List<GroupMember>> GetByGroup(int groupId);
    Task<List<GroupMember>> GetByStudent(int studentId);
    Task<GroupMember?> GetByStudentAndSemester(int studentId, int semesterId);
}

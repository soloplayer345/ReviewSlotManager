using RepositoryLayer.Entities;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services.Interfaces;

public interface ISemesterService : IBaseService<Semester, SemesterDto>
{
    Task<int> Count();
    Task<SemesterDto?> GetActiveSemester();
    Task<SemesterDto> Create(CreateSemesterDto dto);
    Task<SemesterDto> Update(int id, UpdateSemesterDto dto);
}

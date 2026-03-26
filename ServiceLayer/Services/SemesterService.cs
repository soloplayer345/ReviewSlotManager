using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;
using AutoMapper;
using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.DTOs;

namespace ServiceLayer.Services;

public class SemesterService : BaseService<Semester, SemesterDto>, ISemesterService
{
    private readonly ISemesterRepository _semesterRepository;
    private readonly IMapper _mapper;

    public SemesterService(ISemesterRepository semesterRepository, IMapper mapper)
        : base(semesterRepository, mapper)
    {
        _semesterRepository = semesterRepository;
        _mapper = mapper;
    }

    public Task<int> Count()
    {
        return _semesterRepository.Count();
    }

    public async Task<SemesterDto?> GetActiveSemester()
    {
        var semester = await _semesterRepository.GetActiveSemester();
        return semester is null ? null : _mapper.Map<SemesterDto>(semester);
    }

    public async Task<SemesterDto> Create(CreateSemesterDto dto)
    {
        var entity = _mapper.Map<Semester>(dto);
        var created = await _semesterRepository.Create(entity);
        return _mapper.Map<SemesterDto>(created);
    }

    public async Task<SemesterDto> Update(int id, UpdateSemesterDto dto)
    {
        var existing = await _semesterRepository.Read(id)
            ?? throw new KeyNotFoundException($"Semester {id} not found.");
        _mapper.Map(dto, existing);
        await _semesterRepository.Update(existing);
        return _mapper.Map<SemesterDto>(existing);
    }
}

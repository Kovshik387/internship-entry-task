using InternshipTask.Application.DTOs;

namespace InternshipTask.Application.Interfaces;

public interface IETagService
{
    string GenerateETag(MakeMoveDto move);
}
using InternshipTask.Application.DTOs;
using InternshipTask.Domain.Models;

namespace InternshipTask.Application.Extensions.Converters;

public static class MoveTypeConverter
{
    public static MoveModel ToModel(this MoveDto dto)
    {
        return new MoveModel()
        {
            Id = dto.Id,
            GameId = dto.GameId,
            PlayerId = dto.PlayerId,
            MoveCount = dto.MoveCount,
            Col = dto.Column,
            Row = dto.Row,
            Letter = dto.Letter,
            MoveTime = dto.MoveTime,
            ETag = dto.ETag
        };
    }
    
    public static MoveDto ToDto(this MoveModel model)
    {
        return new MoveDto(
            model.Id,
            model.GameId,
            model.PlayerId,
            model.Row,
            model.Col,
            model.Letter,
            model.MoveTime,
            model.MoveCount,
            model.ETag);
    }
}
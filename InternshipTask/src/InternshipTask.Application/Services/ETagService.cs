using System.Text;
using System.Text.Json;
using InternshipTask.Application.DTOs;
using InternshipTask.Application.Interfaces;

namespace InternshipTask.Application.Services;

public class ETagService : IETagService
{
    public string GenerateETag(MakeMoveDto move)
    {
        var data = new
        {
            GameId = move.GameId,
            PlayerId = move.PlayerId,
            Row = move.Row,
            Column = move.Column,
        };
        var json = JsonSerializer.Serialize(data);
        
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }
}
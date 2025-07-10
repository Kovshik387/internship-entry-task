using InternshipTask.Domain.Models;

namespace InternshipTask.UnitTests.Creators;

public static class Create
{
    public static MoveModel CreateMove(int row, int col, string letter)
    {
        return new MoveModel
        {
            Row = row,
            Col = col,
            Letter = letter
        };
    }
}
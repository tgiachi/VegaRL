using Vega.Framework.Data.Entities.Base;
using Vega.Framework.Data.Entities.Buildings;

namespace Vega.Framework.Utils;

public static class BuildingEntityUtils
{
    public static IEnumerable<string> RotateBuilding(this BuildingEntity entity, TileRotationEnum rotationEnum) => entity.Rows.Count == 0 ? Array.Empty<string>() : RotateList(entity.Rows, rotationEnum);

    private static IEnumerable<string> RotateList(List<string> entity, TileRotationEnum angle)
    {
        int numRotations = (int)angle / 90;

        for (int i = 0; i < Math.Abs(numRotations); i++)
        {
            entity = numRotations > 0 ? Rotate90DegreesClockwise(entity) : Rotate90DegreesCounterClockwise(entity);
        }

        return entity;
    }

    private static List<string> Rotate90DegreesClockwise(List<string> house)
    {
        int rows = house.Count;
        int cols = house[0].Length;

        List<string> rotatedHouse = new List<string>();

        for (int col = 0; col < cols; col++)
        {
            string newRow = "";

            for (int row = rows - 1; row >= 0; row--)
            {
                newRow += house[row][col];
            }

            rotatedHouse.Add(newRow);
        }

        return rotatedHouse;
    }

    private static List<string> Rotate90DegreesCounterClockwise(List<string> house)
    {
        int rows = house.Count;
        int cols = house[0].Length;

        List<string> rotatedHouse = new List<string>();

        for (int col = cols - 1; col >= 0; col--)
        {
            string newRow = "";

            for (int row = 0; row < rows; row++)
            {
                newRow += house[row][col];
            }

            rotatedHouse.Add(newRow);
        }

        return rotatedHouse;
    }
}

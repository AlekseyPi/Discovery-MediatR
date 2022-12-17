using System.ComponentModel.DataAnnotations;

namespace Discovery.Domain;

public class Robot
{
    public int Id { get; private set; }
    public decimal X { get; private set; }
    public decimal Y { get; private set; }
    public decimal DirectionAngle { get; private set; }
    public decimal AreaX1 { get; }
    public decimal AreaY1 { get; }
    public decimal AreaX2 { get; }
    public decimal AreaY2 { get; }
    
    public bool IsInArea => X > AreaX1 && X < AreaX2 && Y > AreaY1 && Y < AreaY2;

    public Robot(
        int id, 
        decimal x, 
        decimal y, 
        decimal directionAngle, 
        decimal areaX1,
        decimal areaY1,
        decimal areaX2,
        decimal areaY2
        )
    {
        if (!new decimal[] {0, 90, 180, 270}.Contains(directionAngle))
        {
            throw new ValidationException($"Invalid direction angle {directionAngle}. Supported angles are: 0, 90, 180, 270.");
        }
        if (areaX1 > areaX2)
        {
            throw new ValidationException($"AreaX1 ({areaX1} must be smaller then AreaX2 {areaX2})");
        }
        if (areaY1 > areaY2)
        {
            throw new ValidationException($"AreaY1 ({areaY1} must be smaller then AreaY2 {areaY2})");
        }
        
        Id = id;
        X = x;
        Y = y;
        DirectionAngle = directionAngle;
        AreaX1 = areaX1;
        AreaY1 = areaY1;
        AreaX2 = areaX2;
        AreaY2 = areaY2;
    }

    public int SetId(int id)
    {
        return Id = id;
    }

    

    public void Execute(Command command)
    {
        switch (command)
        {
            case Command.Advance:
                switch (DirectionAngle)
                {
                    case 0:
                        Y++;
                        break;
                    case 90:
                        X++;
                        break;
                    case 180:
                        Y--;
                        break;
                    case 270:
                        X--;
                        break;
                    default:
                        throw new InvalidOperationException($"Invalid direction angle {DirectionAngle}. Supported angles are: 0, 90, 180, 270.");
                };
                break;
                
            case Command.Left:
                DirectionAngle = DirectionAngle == 0 ? 270 : DirectionAngle - 90;
                break;
            case Command.Right:
                DirectionAngle = DirectionAngle == 270 ? 0 : DirectionAngle + 90;
                break;
            default:
                throw new NotImplementedException($"Command {command} is not implemented");
        }
    }
};


namespace Constellation.Bca.Application.DTOs.Enums
{
    public enum VehicleOperatorDto
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Contains,
    }

    public enum ColumnsDto
    {
        RegistrationYear,
        NumberOfSeats,
        NumberOfDoors,
        Model,
        Manufacturer,
        Name,
        UniqueIdentifier,
        LoadCapacity,
        VehicleType, 
        UserName
    }

    public enum LogicDto
    {
        None,
        And,
        Or
    }
}

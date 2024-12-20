﻿
namespace Constellation.Bca.Domain.Enums
{
    public enum VehicleType
    {
        SUV = 1,
        Sedan = 2,
        Hatchback = 3,
        Truck = 4,
    }

    public enum Operator
    {
        Equal,
        NotEqual,
        GreaterThan, 
        GreaterThanOrEqual,
        LessThan, 
        LessThanOrEqual,
        Contains,
    }

    public enum Columns
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

    public enum Logic
    {
        None,
        And,
        Or
    }
}

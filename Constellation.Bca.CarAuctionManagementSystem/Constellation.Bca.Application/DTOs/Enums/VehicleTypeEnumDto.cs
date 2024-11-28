
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Constellation.Bca.Application.DTOs.Enums
{
    [DataContract]
    public enum VehicleTypeEnumDto
    {
        [DataMember]
        SUV = 1,

        [DataMember]
        Sedan = 2,

        [DataMember]
        Hatchback = 3,

        [DataMember]
        Truck = 4,
    }

}

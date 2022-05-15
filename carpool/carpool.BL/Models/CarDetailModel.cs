using AutoMapper;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models;

public record CarDetailModel(
    Manufacturer Manufacturer,
    CarType CarType,
    int SeatCapacity,
    DateTime RegistrationDate,
    Guid OwnerId) : ModelBase
{
    public Manufacturer Manufacturer { get; set; } = Manufacturer;
    public CarType CarType { get; set; } = CarType;
    public int SeatCapacity { get; set; } = SeatCapacity;
    public DateTime RegistrationDate { get; set; } = RegistrationDate;
    public string? PhotoUrl { get; set; }

    public Guid OwnerId { get; set; } = OwnerId;

    public static CarDetailModel Empty => new(default, default, default, default, default);

    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            CreateMap<CarEntity, CarDetailModel>()
                .ReverseMap()
                .ForMember(dst => dst.Owner, opt => opt.Ignore());
        }
    }
}
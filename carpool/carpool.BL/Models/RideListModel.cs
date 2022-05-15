using AutoMapper;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models;

public record RideListModel(
    string Start,
    string End,
    DateTime BeginTime,
    int SeatCapacity) : ModelBase
{
#nullable disable
    public RideListModel() : this(string.Empty, string.Empty, default, default)
    {
    }
#nullable enable
    public string Start { get; set; } = Start;
    public string End { get; set; } = End;
    public DateTime BeginTime { get; set; } = BeginTime;
    public int SeatCapacity { get; set; } = SeatCapacity;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideListModel>()
                .ForMember(dst => dst.SeatCapacity, opt => opt.MapFrom(src => src.Car!.SeatCapacity));
        }
    }
}
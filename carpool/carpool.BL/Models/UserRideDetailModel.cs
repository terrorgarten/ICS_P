﻿using AutoMapper;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models;

public record UserRideDetailModel(
    string Name,
    string Surname) : ModelBase
{
#nullable disable
    public UserRideDetailModel() : this(string.Empty, string.Empty)
    {
    }
#nullable enable

    public Guid PassengerId { get; set; }
    public Guid RideId { get; set; }
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRideEntity, UserRideDetailModel>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Passenger!.Name))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Passenger!.Surname))
                .ReverseMap()
                .ForMember(entity => entity.Passenger, expression => expression.Ignore())
                .ForMember(entity => entity.Ride, expression => expression.Ignore());
        }

        public static UserRideDetailModel Empty => new("Unknown", "Unknown");
    }
}
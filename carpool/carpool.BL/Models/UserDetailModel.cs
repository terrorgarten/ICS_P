﻿using AutoMapper;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models;

public record UserDetailModel(
    string Name,
    string Surname,
    string? PhotoUrl) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string? PhotoUrl { get; set; } = PhotoUrl;

    //List??
    public List<CarDetailModel> OwnedCars { get; init; } = new();

    public List<RideDetailModel> DriverRides { get; init; } = new();

    public List<UserRideDetailModel> PassengerRides { get; init; } = new();

    public static UserDetailModel Empty => new("Unknown", "Unknown", string.Empty);

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserDetailModel>()
                .ForMember(dst => dst.DriverRides, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dst => dst.PassengerRides, opt => opt.Ignore())
                .ForMember(dst => dst.OwnedCars, opt => opt.Ignore());
        }
    }
}
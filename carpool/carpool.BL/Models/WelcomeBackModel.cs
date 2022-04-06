using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record WelcomeBackModel(
        string Name,
        string Surname,
        string PhotoUrl) : ModelBase
    {
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
        public string PhotoUrl { get; set; } = PhotoUrl;

        public class WelcomeMapperProfile : Profile
        {
            public WelcomeMapperProfile()
            {
                CreateMap<UserEntity, WelcomeBackModel>()
                    .ReverseMap();
            }
        }

        public static WelcomeBackModel Empty => new("Unknown", "Unknown", "defaultCestaNaDefaultFoto");

    }
}

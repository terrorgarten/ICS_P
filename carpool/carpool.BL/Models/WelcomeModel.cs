using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Carpool.Common.Enums;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record WelcomeModel(
        //string Name,
        /*string Surname*/) : ModelBase
    {
        //public string Name { get; set; } = Name;
        //public string Surname { get; set; } = Surname;

        //Zobrazujeme dropdown menu uživatelů
        public List<UserListModel> ExistingUsers { get; init; } = new();

        public class WelcomeMapperProfile : Profile
        {
            public WelcomeMapperProfile()
            {
                CreateMap<UserEntity, WelcomeModel>()
                    .ReverseMap();  
            }
        }

        //public static WelcomeModel Empty => new("Unknown", "Unknown");
    }
}

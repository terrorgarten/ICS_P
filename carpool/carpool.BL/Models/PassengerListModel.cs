using AutoMapper;
using Carpool.DAL.Entities;

namespace Carpool.BL.Models
{
    public record PassengerListModel(
        string Name, //ID??
        string Surname) : ModelBase
    {
        public string Name { get; set; } = Name;
        public string Surname { get; set; } = Surname;
    }
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, PassengerListModel>();
        }
    }
}

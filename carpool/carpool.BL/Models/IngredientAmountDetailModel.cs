//using System;
//using System.Collections.Generic;
//using AutoMapper;
//using AutoMapper.EquivalencyExpression;
//using carpool.common.Enums;
//using carpool.DAL.Entities;

//namespace carpool.BL.Models
//{
//    public record UserRideDetailModel(
//        Guid IngredientId,
//        string IngredientName,
//        string IngredientDescription,
//        double Amount,
//        Unit Unit) : ModelBase
//    {
//        public Guid IngredientId { get; set; } = IngredientId;
//        public string IngredientName { get; set; } = IngredientName;
//        public string IngredientDescription { get; set; } = IngredientDescription;
//        public string? IngredientImageUrl { get; set; }
//        public double Amount { get; set; } = Amount;
//        public Unit Unit { get; set; } = Unit;

//        public class MapperProfile : Profile
//        {
//            public MapperProfile()
//            {
//                CreateMap<UserRideEntity, UserRideDetailModel>()
//                    .ReverseMap()
//                    .ForMember(entity => entity.Ingredient, expression => expression.Ignore())
//                    .ForMember(entity => entity.Recipe, expression => expression.Ignore())
//                    .ForMember(entity => entity.RecipeId, expression => expression.Ignore());
//            }
//        }
//    }
//}
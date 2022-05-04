using Carpool.BL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carpool.App.Wrappers
{
    public class UserRideWrapper : ModelWrapper<UserRideDetailModel>
    {
        public UserRideWrapper(UserRideDetailModel model)
            : base(model)
        {
        }

        public string? Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? Surname
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult($"{nameof(Name)} is required", new[] {nameof(Name)});
            }

            if (string.IsNullOrWhiteSpace(Surname))
            {
                yield return new ValidationResult($"{nameof(Surname)} is required", new[] {nameof(Surname)});
            }

            //TODO: tu mozno budu podobne tie validace? :hmmmm:
            //if (string.IsNullOrWhiteSpace(IngredientDescription))
            //{
            //    yield return new ValidationResult($"{nameof(IngredientDescription)} is required", new[] {nameof(IngredientDescription)});
            //}
            //if (Amount > 0)
            //{
            //    yield return new ValidationResult($"{nameof(Amount)} is required", new[] {nameof(Amount)});
            //}
            //if (Unit != Unit.None)
            //{
            //    yield return new ValidationResult($"{nameof(Unit)} is required", new[] {nameof(Amount)});
            //}
        }

        public static implicit operator UserRideWrapper(UserRideDetailModel detailModel)
            => new(detailModel);

        public static implicit operator UserRideDetailModel(UserRideWrapper wrapper)
            => wrapper.Model;
    }
}
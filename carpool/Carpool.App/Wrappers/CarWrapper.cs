using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Carpool.BL.Models;
using Carpool.Common.Enums;

namespace Carpool.App.Wrappers
{
    public class CarWrapper : ModelWrapper<CarDetailModel>
    {
        public CarWrapper(CarDetailModel model)
            : base(model)
        {
        }

        public Manufacturer Manufacturer
        {
            get => GetValue<Manufacturer>();
            set => SetValue(value);
        }
        public CarType CarType
        {
            get => GetValue<CarType>();
            set => SetValue(value);
        }
        public int SeatCapacity
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        public DateTime RegistrationDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }
        public string? PhotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public Guid? OwnerId
        {
            get => GetValue<Guid>();
            set => SetValue(value);
        }
        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    //if (string.IsNullOrWhiteSpace(Name))
        //    //{
        //    //    yield return new ValidationResult($"{nameof(Name)} is required", new[] {nameof(Name)});
        //    //}

        //    //if (string.IsNullOrWhiteSpace(Description))
        //    //{
        //    //    yield return new ValidationResult($"{nameof(Description)} is required", new[] {nameof(Description)});
        //    //}
        //}

        public static implicit operator CarWrapper(CarDetailModel detailModel)
            => new(detailModel);

        public static implicit operator CarDetailModel(CarWrapper wrapper)
            => wrapper.Model;
    }
}
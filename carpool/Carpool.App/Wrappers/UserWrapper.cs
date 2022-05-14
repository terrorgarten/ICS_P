using Carpool.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Carpool.App.Extensions;
using Carpool.Common.Enums;

namespace Carpool.App.Wrappers
{
    public class UserWrapper : ModelWrapper<UserDetailModel>
    {
        public UserWrapper(UserDetailModel model)
            : base(model)
        {
            InitializeCollectionProperties(model);
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

        public string? PhotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        private void InitializeCollectionProperties(UserDetailModel model)
        {
            
            OwnedCars.AddRange(model.OwnedCars.Select(e => new CarWrapper(e)));

            RegisterCollection(OwnedCars, model.OwnedCars);

            DriverRides.AddRange(model.DriverRides.Select(d => new RideWrapper(d)));

            RegisterCollection(DriverRides, model.DriverRides);

            PassangerRides.AddRange(model.PassengerRides.Select(f => new UserRideWrapper(f)));

            RegisterCollection(PassangerRides, model.PassengerRides);
        }

        public ObservableCollection<CarWrapper> OwnedCars { get; set; } = new();

        public ObservableCollection<RideWrapper> DriverRides { get; set; } = new();

        public ObservableCollection<UserRideWrapper> PassangerRides { get; set; } = new();

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult($"{nameof(Name)} is required", new[] {nameof(Name)});
            }

            if (string.IsNullOrWhiteSpace(Surname))
            {
                yield return new ValidationResult($"{nameof(Surname)} is required", new[] { nameof(Surname) });
            }

        }

        public static implicit operator UserWrapper(UserDetailModel detailModel)
            => new(detailModel);

        public static implicit operator UserDetailModel(UserWrapper wrapper)
            => wrapper.Model;
    }
}
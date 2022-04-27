﻿using Carpool.BL.Models;
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
            //InitializeCollectionProperties(model);
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

        //private void InitializeCollectionProperties(UserDetailModel model)
        //{
        //    if (model.OwnedCars == null)
        //    {
        //        throw new ArgumentException("Owned Cars cannot be null");
        //    }
        //    OwnedCars.AddRange(model.OwnedCars.Select(e => new CarWrapper(e)));

        //    RegisterCollection(OwnedCars, model.OwnedCars);
        //}

        public ObservableCollection<CarWrapper> OwnedCars { get; set; } = new();

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

            //if (string.IsNullOrWhiteSpace(Description))
            //{
            //    yield return new ValidationResult($"{nameof(Description)} is required", new[] {nameof(Description)});
            //}
            //if (Duration == default)
            //{
            //    yield return new ValidationResult($"{nameof(Duration)} is required", new[] {nameof(Duration)});
            //}
        }

        public static implicit operator UserWrapper(UserDetailModel detailModel)
            => new(detailModel);

        public static implicit operator UserDetailModel(UserWrapper wrapper)
            => wrapper.Model;
    }
}
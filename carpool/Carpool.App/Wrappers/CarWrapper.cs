﻿using System;
using Carpool.BL.Models;
using Carpool.Common.Enums;

namespace Carpool.App.Wrappers;

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

    public static implicit operator CarWrapper(CarDetailModel detailModel)
    {
        return new CarWrapper(detailModel);
    }

    public static implicit operator CarDetailModel(CarWrapper wrapper)
    {
        return wrapper.Model;
    }
}
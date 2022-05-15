using System;
using Carpool.BL.Models;

namespace Carpool.App.Wrappers;

public class RideWrapper : ModelWrapper<RideDetailModel>
{
    public RideWrapper(RideDetailModel model)
        : base(model)
    {
    }

    public CarDetailModel? Car
    {
        get => GetValue<CarDetailModel>();
        set => SetValue(value);
    }

    public UserDetailModel? User
    {
        get => GetValue<UserDetailModel>();
        set => SetValue(value);
    }

    public string? Start
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? End
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public DateTime BeginTime
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public TimeSpan ApproxRideTime
    {
        get => GetValue<TimeSpan>();
        set => SetValue(value);
    }

    /// <summary>
    ///     TODO - odstranit nullability? pr save async se kontroluje
    /// </summary>
    public Guid? UserId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public Guid? CarId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }


    public static implicit operator RideWrapper(RideDetailModel detailModel)
    {
        return new(detailModel);
    }

    public static implicit operator RideDetailModel(RideWrapper wrapper)
    {
        return wrapper.Model;
    }
}
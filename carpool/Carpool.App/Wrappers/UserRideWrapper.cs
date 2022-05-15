using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Carpool.BL.Models;

namespace Carpool.App.Wrappers;

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
            yield return new ValidationResult($"{nameof(Name)} is required", new[] {nameof(Name)});

        if (string.IsNullOrWhiteSpace(Surname))
            yield return new ValidationResult($"{nameof(Surname)} is required", new[] {nameof(Surname)});

        //TODO: tu mozno budu podobne tie validace? :hmmmm:
    }

    public static implicit operator UserRideWrapper(UserRideDetailModel detailModel)
    {
        return new UserRideWrapper(detailModel);
    }

    public static implicit operator UserRideDetailModel(UserRideWrapper wrapper)
    {
        return wrapper.Model;
    }
}
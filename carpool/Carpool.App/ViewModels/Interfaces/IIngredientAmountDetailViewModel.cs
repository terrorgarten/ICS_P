using System;

namespace Carpool.App.ViewModels
{
    public interface IIngredientAmountDetailViewModel : IViewModel
    {
        Guid RecipeId { get; set; }
    }
}
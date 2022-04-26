using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.BL.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.App.Commands;
using Carpool.BL.Facades;

namespace Carpool.App.ViewModels
{
    public class IngredientListViewModel : ViewModelBase, IIngredientListViewModel
    {
        private readonly CarFacade _ingredientFacade;
        private readonly IMediator _mediator;

        public IngredientListViewModel(CarFacade ingredientFacade, IMediator mediator)
        {
            _ingredientFacade = ingredientFacade;
            _mediator = mediator;

            IngredientSelectedCommand = new RelayCommand<CarListModel>(IngredientSelected);
            IngredientNewCommand = new RelayCommand(IngredientNew);

            mediator.Register<UpdateMessage<IngredientWrapper>>(IngredientUpdated);
            mediator.Register<DeleteMessage<IngredientWrapper>>(IngredientDeleted);
        }

        public ObservableCollection<CarListModel> Ingredients { get; set; } = new();

        public ICommand IngredientSelectedCommand { get; }
        public ICommand IngredientNewCommand { get; }

        private void IngredientNew() => _mediator.Send(new NewMessage<IngredientWrapper>());

        private void IngredientSelected(CarListModel? ingredient) => _mediator.Send(new SelectedMessage<IngredientWrapper> { Id = ingredient?.Id });

        private async void IngredientUpdated(UpdateMessage<IngredientWrapper> _) => await LoadAsync();

        private async void IngredientDeleted(DeleteMessage<IngredientWrapper> _) => await LoadAsync();

        public async Task LoadAsync()
        {
            Ingredients.Clear();
            var ingredients = await _ingredientFacade.GetAsync();
            Ingredients.AddRange(ingredients);
        }

        public override void LoadInDesignMode()
        {
            Ingredients.Add(new CarListModel(Name: "Water") { ImageUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png" });
        }
    }
}

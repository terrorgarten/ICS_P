using Carpool.App.Extensions;
using Carpool.App.Messages;
using Carpool.App.Services;
using Carpool.App.Wrappers;
using Carpool.BL.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Carpool.App.Commands;
using Carpool.BL.Facades;
using Carpool.Common.Enums;
using Carpool.DAL.Seeds;

namespace Carpool.App.ViewModels
{
    public class CarListViewModel : ViewModelBase, ICarListViewModel
    {
        private readonly CarFacade _carFacade;

        public CarListViewModel(CarFacade carFacade)
        {
            _carFacade = carFacade;
        }

        public ObservableCollection<CarListModel> Cars { get; set; } = new();

        public async Task LoadAsync()
        {
            Cars.Clear();
            var cars = await _carFacade.GetAsync();
            Cars.AddRange(cars);
        }

        public override void LoadInDesignMode()
        {
            Cars.Add(new CarListModel(Manufacturer: Manufacturer.Dacia, CarType.Micro) { PhotoUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png" });
        }
    }
}

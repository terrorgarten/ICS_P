using System.Threading.Tasks;

namespace Carpool.App.ViewModels;

public interface IListViewModel
{
    Task LoadAsync();
}
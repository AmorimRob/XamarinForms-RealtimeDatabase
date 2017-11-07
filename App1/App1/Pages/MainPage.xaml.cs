using App1.Models;
using Xamarin.Forms;

namespace App1.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;

        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _viewModel.PedidoSelecionado = e.SelectedItem as Pedido;
        }
    }
}

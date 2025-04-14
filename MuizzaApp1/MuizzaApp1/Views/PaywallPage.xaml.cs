using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views
{
    public partial class PaywallPage : ContentPage
    {
        private readonly PaywallViewModel _viewModel;

        public PaywallPage(PaywallViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
} 
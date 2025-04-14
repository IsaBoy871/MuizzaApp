using Microsoft.Maui.Controls;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views
{
    public partial class PremiumStatusPage : ContentPage
    {
        private PremiumStatusViewModel ViewModel => BindingContext as PremiumStatusViewModel;

        public PremiumStatusPage(PremiumStatusViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.OnAppearing();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///QuotesPage");
        }
    }
} 
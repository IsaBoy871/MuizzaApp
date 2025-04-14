using Microsoft.Maui.Controls;
using System;
using MuizzaApp1.ViewModels;

namespace MuizzaApp1.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfilePageViewModel _viewModel;

        public ProfilePage(ProfilePageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("..");
        }

        private void OnTimeSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (BindingContext is ProfilePageViewModel viewModel)
            {
                // Round the value to the nearest integer
                int newValue = (int)Math.Round(e.NewValue);
                viewModel.SelectedTimeIndex = newValue;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
            return true;
        }
    }
} 
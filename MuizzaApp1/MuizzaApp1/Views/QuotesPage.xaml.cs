using MuizzaApp1.ViewModels;

namespace MuizzaApp1;

public partial class QuotesPage : ContentPage
{
    public QuotesPage(QuotesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnFeelingEntered(object sender, EventArgs e)
    {
        if (BindingContext is QuotesPageViewModel viewModel)
        {
            viewModel.SubmitFeelingCommand.Execute(null);
        }
    }
}
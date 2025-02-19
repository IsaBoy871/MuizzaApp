using Microsoft.Maui.Controls;

namespace MuizzaApp1.Extensions;

public static class PageExtensions
{
    public static Task LoadAsync(this Page page)
    {
        if (page is ContentPage contentPage)
        {
            // Initialize any heavy content loading here
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
} 
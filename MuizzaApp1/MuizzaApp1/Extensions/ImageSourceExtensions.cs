using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public static class ImageSourceExtensions
{
    public static async Task LoadAsync(this ImageSource source)
    {
        try
        {
            if (source is FileImageSource fileSource)
            {
                await Task.Run(() => File.OpenRead(fileSource.File));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error preloading image: {ex.Message}");
        }
    }
} 
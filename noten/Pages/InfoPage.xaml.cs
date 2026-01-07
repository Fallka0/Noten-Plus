using Microsoft.Maui.Controls;

namespace noten.Pages;

public partial class InfoPage : ContentPage
{
    public InfoPage()
    {
        InitializeComponent();
    }

    private async void OnFAQClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(FAQPage)}");
    }
}
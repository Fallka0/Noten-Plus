namespace noten.Pages;

public partial class FAQPage : ContentPage
{
    public FAQPage()
    {
        InitializeComponent();
    }

    private void OnFaq1Tapped(object sender, EventArgs e)
    {
        Faq1Answer.IsVisible = !Faq1Answer.IsVisible;
    }

    private void OnFaq2Tapped(object sender, EventArgs e)
    {
        Faq2Answer.IsVisible = !Faq2Answer.IsVisible;
    }
}

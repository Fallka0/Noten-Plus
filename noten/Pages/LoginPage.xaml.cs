namespace noten.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    async void OnLoginClicked(object sender, EventArgs e)
    {
        HideErrors();

        bool valid = true;

        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            EmailError.IsVisible = true;
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            PasswordError.IsVisible = true;
            valid = false;
        }

        if (!valid)
            return;

        // 🔐 FAKE LOGIN DATEN
        if (EmailEntry.Text == "admin" &&
            PasswordEntry.Text == "1234")
        {
            // Login OK → App starten
            Application.Current.MainPage = new AppShell();

        }
        else
        {
            LoginError.IsVisible = true;
        }
    }

    void HideErrors()
    {
        EmailError.IsVisible = false;
        PasswordError.IsVisible = false;
        LoginError.IsVisible = false;
    }
}
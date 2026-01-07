using Microsoft.Maui.Controls;

namespace noten.Pages;

[QueryProperty("Subject", "subject")]
[QueryProperty("Grade", "grade")]
[QueryProperty("Weight", "weight")]
[QueryProperty("Date", "date")]
public partial class SummaryPage : ContentPage
{
    private string? subject;
    public string Subject
    {
        get => subject ?? string.Empty;
        set { subject = System.Net.WebUtility.UrlDecode(value); SubjectLabel.Text = subject; }
    }

    private string? grade;
    public string Grade
    {
        get => grade ?? string.Empty;
        set { grade = System.Net.WebUtility.UrlDecode(value); GradeLabel.Text = grade; }
    }

    private string? weight;
    public string Weight
    {
        get => weight ?? string.Empty;
        set { weight = value; WeightLabel.Text = value + "%"; }
    }

    private string? date;
    public string Date
    {
        get => date ?? string.Empty;
        set { date = value; DateLabel.Text = value; }
    }

    public SummaryPage()
    {
        InitializeComponent();
    }

    private async void OnDoneClicked(object sender, EventArgs e)
    {
         await Shell.Current.GoToAsync($"/{nameof(DashboardPage)}");
    }
}
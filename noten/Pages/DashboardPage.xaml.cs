using Microsoft.Maui.Controls;
using noten.Services;
using System.Collections.ObjectModel;

namespace noten.Pages;

public partial class DashboardPage : ContentPage
{
    
    private GradesService gradesService;
    private SubjectsService subjectsService;

    public DashboardPage()
    {
        InitializeComponent();

        
        // Service initialisieren
        subjectsService = SubjectsService.Instance;
        gradesService = GradesService.Instance;

        // CollectionView an Service binden
        SubjectsCollection.ItemsSource = subjectsService.Subjects;

        

        // Auf Änderungen reagieren
        gradesService.Grades.CollectionChanged += (s, e) => LoadData();
        subjectsService.Subjects.CollectionChanged += (s, e) => LoadData();

        UpdateSubjectsView();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadData();
        UpdateSubjectsView();
    }

    private void LoadData()
    {
        // Gesamtdurchschnitt
        var overallAverage = gradesService.GetOverallAverage();
        OverallAverageLabel.Text = overallAverage > 0 ? overallAverage.ToString("0.00") : "—";
        OverallCountLabel.Text = $"Basierend auf {gradesService.Grades.Count} Noten";

    }

    private void UpdateSubjectsView()
    {
        if (subjectsService.Subjects.Count == 0)
        {
            // Zeige leeren State an
            EmptySubjectsView.IsVisible = true;
            SubjectsCollection.IsVisible = false;
        }
        else
        {
            // Zeige Fächerliste an
            EmptySubjectsView.IsVisible = false;
            SubjectsCollection.IsVisible = true;
        }
    }

    public async void OnAddSubjectClicked(object sender, EventArgs e)
    {
        // Zeige Dialog zum Fach hinzufügen
        string result = await DisplayPromptAsync(
            "Neues Fach",
            "Geben Sie den Namen des Fachs ein:",
            "Hinzufügen",
            "Abbrechen",
            "z.B. Mathematik",
            maxLength: 50);

        if (!string.IsNullOrWhiteSpace(result))
        {
            subjectsService.AddSubject(result);
            UpdateSubjectsView();
        }
    }

    public async void OnAddGradeClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(AddGradePage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not open Add Grade page: {ex.Message}", "OK");
        }
    }
    public void OnAnalyticsClicked(object sender, EventArgs e)
    {
        SwitchToTab(1, "Analytics");
    }
    public void OnFAQClicked(object sender, EventArgs e)
    {
        SwitchToTab(2, "FAQ");
    }

  

    private void SwitchToTab(int tabIndex, string tabName)
    {
        try
        {
            if (Shell.Current?.CurrentItem is TabBar tabBar)
            {
                if (tabBar.Items.Count > tabIndex)
                {
                    tabBar.CurrentItem = tabBar.Items[tabIndex];
                }
                else
                {
                    Console.WriteLine($"Tab index {tabIndex} ({tabName}) not found. Available tabs: {tabBar.Items.Count}");
                }
            }
            else
            {
                Console.WriteLine("Shell.CurrentItem is not a TabBar");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error switching to {tabName} tab: {ex.Message}");
        }
    }

    // Event für Fach löschen
    public async void OnRemoveSubjectClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string subject)
        {
            bool answer = await DisplayAlert(
                "Fach löschen",
                $"Möchten Sie das Fach '{subject}' wirklich löschen?",
                "Löschen",
                "Abbrechen");

            if (answer)
            {
                subjectsService.RemoveSubject(subject);
                UpdateSubjectsView();
            }
        }
    }
}
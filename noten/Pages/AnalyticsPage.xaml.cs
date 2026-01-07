using Microsoft.Maui.Controls;
using noten.Services;

namespace noten.Pages;

public partial class AnalyticsPage : ContentPage
{
    private GradesService gradesService;
    private SubjectsService subjectsService;
    
    public AnalyticsPage()
    {
        InitializeComponent();
        
        // Services initialisieren
        gradesService = GradesService.Instance;
        subjectsService = SubjectsService.Instance;
        
        // Auf Änderungen reagieren
        gradesService.Grades.CollectionChanged += (s, e) => LoadData();
        subjectsService.Subjects.CollectionChanged += (s, e) => LoadData();
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
    }
    
    private void LoadData()
    {
        // Gesamtdurchschnitt
        var overallAverage = gradesService.GetOverallAverage();
        OverallAverageLabel.Text = overallAverage > 0 ? overallAverage.ToString("0.00") : "—";
        OverallCountLabel.Text = $"Basierend auf {gradesService.Grades.Count} Noten";
        
        // Beste Note
        var bestGrade = gradesService.GetBestGrade();
        if (bestGrade != null)
        {
            BestGradeLabel.Text = bestGrade.Value.ToString("0.0");
            BestGradeSubjectLabel.Text = $"Fach: {bestGrade.Subject}";
        }
        else
        {
            BestGradeLabel.Text = "—";
            BestGradeSubjectLabel.Text = "Fach: —";
        }
        
        // Schlechteste Note
        var worstGrade = gradesService.GetWorstGrade();
        if (worstGrade != null)
        {
            WorstGradeLabel.Text = worstGrade.Value.ToString("0.0");
            WorstGradeSubjectLabel.Text = $"Fach: {worstGrade.Subject}";
        }
        else
        {
            WorstGradeLabel.Text = "—";
            WorstGradeSubjectLabel.Text = "Fach: —";
        }
        
        // Notenanzahl
        TotalGradesLabel.Text = gradesService.Grades.Count.ToString();
        
        // Progress Bar für Notenanzahl (bis 20 Noten = 100%)
        double progress = Math.Min(gradesService.Grades.Count / 20.0, 1.0);
        GradesProgressBar.Progress = progress;
        
        // Fächer-Durchschnitte anzeigen
        UpdateSubjectsList();
        
        // Letzte Noten anzeigen (wenn vorhanden)
        UpdateRecentGrades();
    }
    
    private void UpdateSubjectsList()
    {
        if (subjectsService.Subjects.Count == 0)
        {
            SubjectsCollection.ItemsSource = new List<string> { "Keine Fächer vorhanden" };
            return;
        }
        
        // Erstelle Liste mit Fächern und ihren Durchschnitten
        var subjectsWithAverages = new List<SubjectWithAverage>();
        
        foreach (var subject in subjectsService.Subjects)
        {
            var average = gradesService.GetAverageBySubject(subject);
            var gradeCount = gradesService.GetGradesBySubject(subject).Count;
            
            subjectsWithAverages.Add(new SubjectWithAverage
            {
                Name = subject,
                Average = average,
                Count = gradeCount
            });
        }
        
        // Nach Durchschnitt sortieren (beste zuerst)
        subjectsWithAverages = subjectsWithAverages
            .OrderBy(s => s.Average)
            .ThenBy(s => s.Name)
            .ToList();
        
        SubjectsCollection.ItemsSource = subjectsWithAverages;
    }
    
    private void UpdateRecentGrades()
    {
        if (gradesService.Grades.Count == 0)
        {
            RecentGradesFrame.IsVisible = false;
            return;
        }
        
        // Zeige die letzten 5 Noten
        var recentGrades = gradesService.Grades
            .OrderByDescending(g => g.Date)
            .Take(5)
            .ToList();
        
        RecentGradesCollection.ItemsSource = recentGrades;
        RecentGradesFrame.IsVisible = true;
    }
    
    // Event Handler
    private async void OnShowAllSubjects(object sender, EventArgs e)
    {
        var averages = gradesService.GetAveragesByAllSubjects();
        
        if (averages.Count == 0)
        {
            await DisplayAlert("Info", "Noch keine Noten vorhanden.", "OK");
            return;
        }
        
        var message = "Durchschnitte nach Fach:\n\n";
        foreach (var kvp in averages.OrderBy(a => a.Value))
        {
            var gradeCount = gradesService.GetGradesBySubject(kvp.Key).Count;
            message += $"{kvp.Key}: {kvp.Value:0.00} ({gradeCount} Noten)\n";
        }
        
        await DisplayAlert("Alle Fächer", message, "OK");
    }
    
    private void OnRefreshClicked(object sender, EventArgs e)
    {
        LoadData();
    }
    
    private async void OnClearAllClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(
            "Alle Noten löschen",
            "Möchten Sie wirklich alle Noten löschen? Diese Aktion kann nicht rückgängig gemacht werden.",
            "Löschen",
            "Abbrechen");
        
        if (answer)
        {
            gradesService.Grades.Clear();
            LoadData();
            await DisplayAlert("Erfolg", "Alle Noten wurden gelöscht.", "OK");
        }
    }
}

// Hilfsklasse für Fächer mit Durchschnitt
public class SubjectWithAverage
{
    public string Name { get; set; } = string.Empty;
    public double Average { get; set; }
    public int Count { get; set; }
    
    public string DisplayAverage => Average > 0 ? Average.ToString("0.00") : "—";
    public string DisplayCount => Count == 1 ? "1 Note" : $"{Count} Noten";
}
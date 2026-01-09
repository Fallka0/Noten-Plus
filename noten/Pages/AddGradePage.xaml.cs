using Microsoft.Maui.Controls;
using noten.Services;
using System.Globalization;

namespace noten.Pages;

public partial class AddGradePage : ContentPage
{
    private SubjectsService subjectsService;
    
    public AddGradePage()
    {
        InitializeComponent();
        
        // Service initialisieren
        subjectsService = SubjectsService.Instance;
        
        // Events
        WeightSlider.ValueChanged += (s, e) => 
            WeightLabel.Text = $"{Math.Round(WeightSlider.Value)}%";
        
        WhenPicker.Date = DateTime.Now;
        
        // Picker initialisieren
        InitializePicker();
        
        // Entry Styling
        GradeEntry.TextChanged += (s, e) =>
        {
            // Optional: Formatierung der Note
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                if (double.TryParse(e.NewTextValue, out double grade))
                {
                    if (grade > 6.0)
                        GradeEntry.Text = "6.0";
                    else if (grade < 1.0)
                        GradeEntry.Text = "1.0";
                }
            }
        };
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdatePickerItems();
    }
    
    private void InitializePicker()
    {
        // Picker Styling
        SubjectPicker.BackgroundColor = Colors.Transparent;
        SubjectPicker.TextColor = Colors.Black;
        SubjectPicker.TitleColor = Colors.Gray;
        SubjectPicker.FontSize = 16;
        
        UpdatePickerItems();
        
        // Auf Änderungen reagieren
        subjectsService.Subjects.CollectionChanged += (s, e) => 
        {
            UpdatePickerItems();
        };
    }
    
    private void UpdatePickerItems()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SubjectPicker.Items.Clear();
            
            if (subjectsService.Subjects.Count == 0)
            {
                SubjectPicker.Title = "Keine Fächer vorhanden";
                SubjectPicker.IsEnabled = false;
            }
            else
            {
                foreach (var subject in subjectsService.Subjects.OrderBy(s => s))
                {
                    SubjectPicker.Items.Add(subject);
                }
                
                SubjectPicker.Title = "";
                SubjectPicker.IsEnabled = true;
                
                if (SubjectPicker.SelectedIndex == -1 && subjectsService.Subjects.Count > 0)
                {
                    SubjectPicker.SelectedIndex = 0;
                }
            }
        });
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        var subject = SubjectPicker.SelectedItem as string;
        var gradeText = GradeEntry.Text?.Trim();

        var errors = new List<string>();
        
        // Validierung
        if (subjectsService.Subjects.Count == 0)
        {
            errors.Add("Keine Fächer vorhanden");
            errors.Add("Bitte erstellen Sie zuerst ein Fach auf der Dashboard-Seite.");
        }
        else if (string.IsNullOrEmpty(subject))
        {
            errors.Add("Bitte wählen Sie ein Fach aus");
        }
        
        if (string.IsNullOrEmpty(gradeText)) 
        {
            errors.Add("Bitte geben Sie eine Note ein");
        }
        else if (!double.TryParse(gradeText, NumberStyles.Float, CultureInfo.InvariantCulture, out var grade))
        {
            errors.Add("Die Note muss eine Zahl sein (z.B. 5.5)");
        }
        else if (grade < 1.0 || grade > 6.0)
        {
            errors.Add("Die Note muss zwischen 1.0 und 6.0 liegen");
        }

        if (errors.Any())
        {
            ErrorMessage.Text = string.Join("\n\n", errors);
            ErrorLabel.IsVisible = true;
            
            // Zum Error scrollen
            return;
        }

        var gradeValue = double.Parse(gradeText, CultureInfo.InvariantCulture);
    var weightValue = (int)Math.Round(WeightSlider.Value);
    
    var newGrade = new Grade
    {
        Subject = subject,
        Value = gradeValue,
        Weight = weightValue,
        Date = WhenPicker.Date
    };
    
    GradesService.Instance.AddGrade(newGrade);
    
    // Navigation zur Summary Page
    var gradeParam = System.Net.WebUtility.UrlEncode(GradeEntry.Text);
    var subjectParam = System.Net.WebUtility.UrlEncode(subject ?? string.Empty);
    await Shell.Current.GoToAsync($"/{nameof(SummaryPage)}?subject={subjectParam}&grade={gradeParam}&weight={weightValue}&date={WhenPicker.Date:yyyy-MM-dd}");
    }

    private bool _isSnapping;

    private void OnWeightSliderChanged(object sender, ValueChangedEventArgs e)
    {
        if (_isSnapping)
            return;

        double[] snapPoints = { 25, 50, 75, 100 };

        double nearest = snapPoints
            .OrderBy(x => Math.Abs(x - e.NewValue))
            .First();

        _isSnapping = true;
        WeightSlider.Value = nearest;
        _isSnapping = false;

        WeightLabel.Text = $"{(int)nearest}%";
    }


    // Debug-Methode
    private void AddTestSubjects()
    {
        subjectsService.AddSubject("Mathematik");
        subjectsService.AddSubject("Deutsch");
        subjectsService.AddSubject("Englisch");
        subjectsService.AddSubject("Informatik");
        UpdatePickerItems();
    }
}
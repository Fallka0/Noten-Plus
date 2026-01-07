using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace noten.Pages;

public partial class SubjectsPage : ContentPage
{
    ObservableCollection<string> subjects = new ObservableCollection<string>(new[] { "Mathematik", "Deutsch", "Englisch", "Informatik" });

    public SubjectsPage()
    {
        InitializeComponent();
        SubjectsCollection.ItemsSource = subjects;
    }

    private void OnAddSubjectClicked(object sender, EventArgs e)
    {
        var text = NewSubjectEntry.Text?.Trim();
        if (string.IsNullOrEmpty(text)) return;
        subjects.Add(text);
        NewSubjectEntry.Text = string.Empty;
    }

    private void OnRemoveSubjectClicked(object sender, EventArgs e)
    {
        if (sender is Button b && b.CommandParameter is string sub)
            subjects.Remove(sub);
    }
}
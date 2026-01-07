using System.Collections.ObjectModel;

namespace noten.Services;

public class SubjectsService
{
    private static SubjectsService _instance;
    public static SubjectsService Instance => _instance ??= new SubjectsService();
    
    public ObservableCollection<string> Subjects { get; } = new ObservableCollection<string>();
    
    private SubjectsService()
    {
        // Optional: Testdaten
        // Subjects.Add("Mathematik");
        // Subjects.Add("Deutsch");
        // Subjects.Add("Englisch");
    }
    
    public void AddSubject(string subject)
    {
        if (!string.IsNullOrWhiteSpace(subject) && !Subjects.Contains(subject))
        {
            Subjects.Add(subject);
        }
    }
    
    public void RemoveSubject(string subject)
    {
        Subjects.Remove(subject);
    }
}
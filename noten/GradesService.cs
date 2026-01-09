using System.Collections.ObjectModel;
using System.Text.Json;

namespace noten.Services;

public class Grade
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Subject { get; set; } = string.Empty;
    public double Value { get; set; }
    public int Weight { get; set; } = 10;
    public DateTime Date { get; set; } = DateTime.Now;
}

public class GradesService
{
    private static GradesService _instance;
    public static GradesService Instance => _instance ??= new GradesService();
    
    public ObservableCollection<Grade> Grades { get; } = new ObservableCollection<Grade>();
    
    private const string GradesKey = "user_grades";
    
    private GradesService()
    {
        LoadGrades();
    }
    
    public void AddGrade(Grade grade)
    {
        Grades.Add(grade);
        SaveGrades();
    }
    
    public void RemoveGrade(string id)
    {
        var grade = Grades.FirstOrDefault(g => g.Id == id);
        if (grade != null)
        {
            Grades.Remove(grade);
            SaveGrades();
        }
    }
    
    public List<Grade> GetGradesBySubject(string subject)
    {
        return Grades.Where(g => g.Subject == subject).ToList();
    }
    
    public double GetAverageBySubject(string subject)
    {
        var subjectGrades = GetGradesBySubject(subject);
        if (subjectGrades.Count == 0) return 0;
        
        double weightedSum = 0;
        double totalWeight = 0;
        
        foreach (var grade in subjectGrades)
        {
            weightedSum += grade.Value * grade.Weight;
            totalWeight += grade.Weight;
        }
        
        return totalWeight > 0 ? Math.Round(weightedSum / totalWeight, 2) : 0;
    }
    
    public double GetOverallAverage()
    {
        if (Grades.Count == 0) return 0;
        
        double weightedSum = 0;
        double totalWeight = 0;
        
        foreach (var grade in Grades)
        {
            weightedSum += grade.Value * grade.Weight;
            totalWeight += grade.Weight;
        }
        
        return totalWeight > 0 ? Math.Round(weightedSum / totalWeight, 2) : 0;
    }
    
    public Grade GetBestGrade()
    {
        return Grades.OrderByDescending(g => g.Value).FirstOrDefault();
    }
    
    public Grade GetWorstGrade()
    {
        return Grades.OrderBy(g => g.Value).FirstOrDefault();
    }
    
    public Dictionary<string, double> GetAveragesByAllSubjects()
    {
        var result = new Dictionary<string, double>();
        var subjects = Grades.Select(g => g.Subject).Distinct();
        
        foreach (var subject in subjects)
        {
            result[subject] = GetAverageBySubject(subject);
        }
        
        return result;
    }
    
    private void LoadGrades()
    {
        if (Preferences.ContainsKey(GradesKey))
        {
            try
            {
                var json = Preferences.Get(GradesKey, "");
                if (!string.IsNullOrEmpty(json))
                {
                    var grades = JsonSerializer.Deserialize<List<Grade>>(json);
                    if (grades != null)
                    {
                        foreach (var grade in grades)
                        {
                            Grades.Add(grade);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading grades: {ex.Message}");
            }
        }
    }
    
    private void SaveGrades()
    {
        try
        {
            var json = JsonSerializer.Serialize(Grades.ToList());
            Preferences.Set(GradesKey, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving grades: {ex.Message}");
        }
    }
}
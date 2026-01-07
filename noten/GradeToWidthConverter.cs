using System.Globalization;

namespace noten.Converters;

public class GradeToWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string gradeText && double.TryParse(gradeText, out double grade))
        {
            // Konvertiere Note (1.0-6.0) zu Prozent (0-100)
            // Bessere Note (niedrigere Zahl) = höherer Balken
            double percentage = ((6.0 - grade) / 5.0) * 100;
            
            // Maximal 200 Pixel Breite
            return Math.Min(percentage * 2, 200);
        }
        
        return 100; // Standardwert
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
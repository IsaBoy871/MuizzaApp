using System.Globalization;

namespace MuizzaApp1.Converters
{
    public class PremiumEmotionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPremiumUser && parameter is string emotionName)
            {
                string[] premiumEmotions = { "Restless", "Insecure", "Hopeful", "Content", "Confident", "Motivated" };
                return !isPremiumUser && premiumEmotions.Contains(emotionName);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 
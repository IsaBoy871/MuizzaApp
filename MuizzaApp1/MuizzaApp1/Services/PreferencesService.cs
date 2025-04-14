using Microsoft.Maui.Storage;
using MuizzaApp1.Contracts.Services;

namespace MuizzaApp1.Services
{
    public class PreferencesService : IPreferencesService
    {
        public void Set<T>(string key, T value)
        {
            if (value is string stringValue)
                Preferences.Set(key, stringValue);
            else if (value is bool boolValue)
                Preferences.Set(key, boolValue);
            else if (value is int intValue)
                Preferences.Set(key, intValue);
            else if (value is double doubleValue)
                Preferences.Set(key, doubleValue);
            else if (value is float floatValue)
                Preferences.Set(key, floatValue);
            else if (value is long longValue)
                Preferences.Set(key, longValue);
            else
                throw new ArgumentException($"Type {typeof(T)} is not supported");
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)Preferences.Get(key, (string)Convert.ChangeType(defaultValue, typeof(string)));
            else if (typeof(T) == typeof(bool))
                return (T)(object)Preferences.Get(key, (bool)Convert.ChangeType(defaultValue, typeof(bool)));
            else if (typeof(T) == typeof(int))
                return (T)(object)Preferences.Get(key, (int)Convert.ChangeType(defaultValue, typeof(int)));
            else if (typeof(T) == typeof(double))
                return (T)(object)Preferences.Get(key, (double)Convert.ChangeType(defaultValue, typeof(double)));
            else if (typeof(T) == typeof(float))
                return (T)(object)Preferences.Get(key, (float)Convert.ChangeType(defaultValue, typeof(float)));
            else if (typeof(T) == typeof(long))
                return (T)(object)Preferences.Get(key, (long)Convert.ChangeType(defaultValue, typeof(long)));
            else
                throw new ArgumentException($"Type {typeof(T)} is not supported");
        }

        public void Remove(string key)
        {
            Preferences.Remove(key);
        }

        public void Clear()
        {
            Preferences.Clear();
        }
    }
} 
using Service;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public sealed class ConfigurationProvider : IConfigurationProvider, IConfigurationSetter
    {
        private readonly IDictionary<string, string> configurationValues = new Dictionary<string, string>();

        public string GetConfigurationValue(string key)
        {
            if(configurationValues.TryGetValue(key, out var value))
            {
                return value;
            }
            
            // This needs to be a better error message.
            throw new IndexOutOfRangeException($"No value has been set for {key}.");
        }

        public void SetConfigurationValue(string key, string value)
        {
            configurationValues.TryAdd(key, value);
        }
    }
}

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Service
{
    public sealed class TextResourceProvider : ITextResourceProvider
    {                      
        private static readonly IDictionary<string, string> foundResources = new ConcurrentDictionary<string, string>();        

        public string GetResource(string resourceName)
        {
            if (foundResources.TryGetValue(resourceName, out var value))
            {
                return value;
            }

            var text = ReadTextFromResource(resourceName);
            
            foundResources.Add(resourceName, text);

            return text;            
        }

        private string ReadTextFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var text = string.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }
    }
}

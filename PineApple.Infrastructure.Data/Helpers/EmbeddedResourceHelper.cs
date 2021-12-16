using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PineApple.Infrastructure.Data.Contracts.Helpers
{
    public static class EmbeddedResourceHelper
    {
        private static readonly IDictionary<string, IDictionary<string, string>> _cache = new ConcurrentDictionary<string, IDictionary<string, string>>();

        public static async Task<string> GetContent(Assembly assembly, string name)
        {
            if (assembly == null || string.IsNullOrWhiteSpace(name))
                return null;

            // Get Assembly cache
            if (!_cache.TryGetValue(assembly.FullName, out IDictionary<string, string> assemblyResources))
            {
                assemblyResources = new ConcurrentDictionary<string, string>();
                _cache[assembly.FullName] = assemblyResources;
            }

            // Get or Cache resource content
            if (!assemblyResources.TryGetValue(name, out string content))
            {
                string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(name));

                if (resourceName == null)
                {
                    assemblyResources[name] = null;
                }
                else
                {
                    using (var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
                    {
                        content = await reader.ReadToEndAsync();
                        assemblyResources[name] = content;
                    }
                }
            }

            return content;
        }
    }
}

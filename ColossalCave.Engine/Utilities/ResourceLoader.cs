using System.IO;
using System.Reflection;
using System.Text;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.FileProviders;

namespace ColossalCave.Engine.Utilities
{
    public class ResourceLoader : IResourceLoader
    {
        public string LoadAsset(string assetName)
        {
            var assembly = typeof(ResourceLoader).GetTypeInfo().Assembly;
            var embeddedFileProvider = new EmbeddedFileProvider(assembly, "ColossalCave.Engine.Assets");
            var fileInfo = embeddedFileProvider.GetFileInfo(assetName);
            using (var stream = fileInfo.CreateReadStream())
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

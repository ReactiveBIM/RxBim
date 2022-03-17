namespace RxBim.Nuke.Extensions
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions for embedded resources
    /// </summary>
    public static class EmbeddedResourceExtensions
    {
        /// <summary>
        /// Reads embedded resource as string
        /// </summary>
        /// <param name="name">Embedded resource name</param>
        public static string ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = name;

            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!name.StartsWith(nameof(RxBim)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            using var reader = new StreamReader(stream!);
            return reader.ReadToEnd();
        }
    }
}
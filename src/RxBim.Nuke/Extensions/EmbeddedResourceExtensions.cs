namespace RxBim.Nuke.Extensions
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Extensions for embedded resources
    /// </summary>
    public class EmbeddedResourceExtensions
    {
        /// <summary>
        /// Reads embedded resource as string
        /// </summary>
        /// <param name="name">Embedded resource name</param>
        public static string ReadResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(name);
            if (stream == null)
                return string.Empty;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
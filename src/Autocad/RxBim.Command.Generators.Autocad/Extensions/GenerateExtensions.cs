namespace RxBim.Command.Generators.Autocad.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions.
    /// </summary>
    public static class GenerateExtensions
    {
        /// <summary>
        /// Reads source text from assembly resources.
        /// </summary>
        /// <param name="assembly">Source assembly.</param>
        /// <param name="resourceName">Resource name.</param>
        public static string ReadTextFromResource(this Assembly assembly, string resourceName)
        {
            var name = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(resourceName));

            if (name == null)
                throw new ArgumentException($"Resource '{resourceName}' not found in assembly '{assembly.FullName}'");

            using var manifestResourceStream = assembly.GetManifestResourceStream(name);

            if (manifestResourceStream is null)
                throw new InvalidOperationException($"Can't get manifest for assembly '{assembly.FullName}'");

            using var streamReader = new StreamReader(manifestResourceStream);
            return streamReader.ReadToEnd();
        }
    }
}
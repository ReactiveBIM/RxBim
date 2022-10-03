namespace RxBim.Nuke.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using Models;

    /// <summary>
    /// An assembly scanner utilities.
    /// </summary>
    public static class AssemblyScanner
    {
        /// <summary>
        /// Scans an assembly.
        /// </summary>
        /// <param name="file">The assembly file path.</param>
        public static IEnumerable<AssemblyType> Scan(string file)
        {
            if (!File.Exists(file))
                yield break;

            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fileStream);
            var metadataReader = peReader.GetMetadataReader();

            foreach (var typeDefinitionHandle in metadataReader.TypeDefinitions)
            {
                var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);
                var baseTypeName = GetBaseTypeName(metadataReader, typeDefinition);
                var fullName = GetFullName(metadataReader, typeDefinition);
                if (string.IsNullOrEmpty(fullName))
                    continue;

                yield return new AssemblyType(Path.GetFileNameWithoutExtension(file), fullName, baseTypeName);
            }
        }

        private static string? GetBaseTypeName(MetadataReader metadataReader, TypeDefinition typeDefinition)
        {
            var baseTypeDefinition = typeDefinition.BaseType;
            if (baseTypeDefinition.IsNil)
                return null;

            try
            {
                var referenceHandle = (TypeReferenceHandle)baseTypeDefinition;
                var typeReference = metadataReader.GetTypeReference(referenceHandle);
                return metadataReader.GetString(typeReference.Name);
            }
            catch
            {
                return null;
            }
        }

        private static string? GetFullName(MetadataReader metadataReader, TypeDefinition typeDefinition)
        {
            try
            {
                var nameSpace = metadataReader.GetString(typeDefinition.Namespace);
                var name = metadataReader.GetString(typeDefinition.Name);
                return $"{nameSpace}.{name}";
            }
            catch
            {
                return null;
            }
        }
    }
}
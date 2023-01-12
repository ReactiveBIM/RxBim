namespace RxBim.Nuke.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

                var types = new List<string>();
                var directory = Path.GetDirectoryName(file);

                GetBaseTypesNames(typeDefinition, metadataReader, directory, types);

                var fullName = GetFullName(metadataReader, typeDefinition);
                if (string.IsNullOrEmpty(fullName))
                    continue;

                yield return new AssemblyType(Path.GetFileNameWithoutExtension(file), fullName, types);
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

        private static void GetBaseTypesNames(
            TypeDefinition typeDefinition,
            MetadataReader metadataReader,
            string? directoryPath,
            List<string> types)
        {
            var baseTypeEntityHandle = typeDefinition.BaseType;
            if (baseTypeEntityHandle.IsNil)
                return;

            try
            {
                switch (baseTypeEntityHandle.Kind)
                {
                    case HandleKind.TypeDefinition:
                        GetFromBaseTypeDefinition(metadataReader, directoryPath, types, baseTypeEntityHandle);
                        break;
                    case HandleKind.TypeReference:
                        GetFromBaseTypeReference(metadataReader, directoryPath, types, baseTypeEntityHandle);
                        break;
                }
            }
            catch
            {
                // ignore
            }
        }

        private static void GetFromBaseTypeDefinition(
            MetadataReader metadataReader,
            string? directoryPath,
            List<string> types,
            EntityHandle baseTypeEntityHandle)
        {
            var definitionHandle = (TypeDefinitionHandle)baseTypeEntityHandle;
            var baseTypeDefinition = metadataReader.GetTypeDefinition(definitionHandle);
            var typeName = metadataReader.GetString(baseTypeDefinition.Name);

            types.Add(typeName);

            GetBaseTypesNames(baseTypeDefinition, metadataReader, directoryPath, types);
        }

        private static void GetFromBaseTypeReference(
            MetadataReader metadataReader,
            string? directoryPath,
            List<string> types,
            EntityHandle baseTypeEntityHandle)
        {
            var referenceHandle = (TypeReferenceHandle)baseTypeEntityHandle;
            var typeReference = metadataReader.GetTypeReference(referenceHandle);
            var typeName = metadataReader.GetString(typeReference.Name);
            var typeNameSpace = metadataReader.GetString(typeReference.Namespace);

            types.Add(typeName);

            if (typeReference.ResolutionScope.Kind is HandleKind.AssemblyReference)
            {
                var assemblyReference =
                    metadataReader.GetAssemblyReference((AssemblyReferenceHandle)typeReference.ResolutionScope);
                var assemblyName = assemblyReference.GetAssemblyName().Name;
                if (assemblyName != null && directoryPath != null)
                    GetBaseTypeFromAssembly(directoryPath, assemblyName, typeName, typeNameSpace, types);
            }
        }

        private static void GetBaseTypeFromAssembly(
            string directoryPath,
            string assemblyName,
            string typeName,
            string typeNameSpace,
            List<string> types)
        {
            var assemblyPath = Path.Combine(directoryPath, $"{assemblyName}.dll");

            if (!File.Exists(assemblyPath))
                return;

            using var fileStream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fileStream);
            var metadataReader = peReader.GetMetadataReader();

            var typeDefinition = metadataReader.TypeDefinitions
                .Select(x => metadataReader.GetTypeDefinition(x))
                .FirstOrDefault(x =>
                {
                    var defName = metadataReader.GetString(x.Name);
                    var defNameSpace = metadataReader.GetString(x.Namespace);
                    return defName == typeName && defNameSpace == typeNameSpace;
                });

            GetBaseTypesNames(typeDefinition, metadataReader, directoryPath, types);
        }
    }
}
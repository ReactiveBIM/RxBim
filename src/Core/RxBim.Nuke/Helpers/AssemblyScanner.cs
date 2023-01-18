namespace RxBim.Nuke.Helpers
{
    using System;
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
        /// <param name="assemblyFilePath">The assembly file path.</param>
        public static IEnumerable<AssemblyType> Scan(string assemblyFilePath)
        {
            if (!File.Exists(assemblyFilePath))
                yield break;

            var assembliesDir = Path.GetDirectoryName(assemblyFilePath);
            if (assembliesDir is null)
                yield break;

            using var fileStream =
                new FileStream(assemblyFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fileStream);
            var metadataReader = peReader.GetMetadataReader();

            foreach (var typeDefinitionHandle in metadataReader.TypeDefinitions)
            {
                var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

                var types = GetBaseTypesNames(typeDefinition, metadataReader, assembliesDir);

                var fullName = GetFullName(metadataReader, typeDefinition);
                if (string.IsNullOrEmpty(fullName))
                    continue;

                yield return new AssemblyType(Path.GetFileNameWithoutExtension(assemblyFilePath), fullName, types);
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

        private static IEnumerable<string> GetBaseTypesNames(
            TypeDefinition typeDefinition,
            MetadataReader metadataReader,
            string assembliesDir)
        {
            var baseTypeEntityHandle = typeDefinition.BaseType;

            if (baseTypeEntityHandle.IsNil)
                return Array.Empty<string>();

            try
            {
                switch (baseTypeEntityHandle.Kind)
                {
                    case HandleKind.TypeDefinition:
                        return GetTypesFromTypeDefinition(
                            (TypeDefinitionHandle)baseTypeEntityHandle,
                            metadataReader,
                            assembliesDir);
                    case HandleKind.TypeReference:
                        return GetTypesFromTypeReference(
                            (TypeReferenceHandle)baseTypeEntityHandle,
                            metadataReader,
                            assembliesDir);
                }
            }
            catch
            {
                // ignore
            }

            return Array.Empty<string>();
        }

        private static IEnumerable<string> GetTypesFromTypeDefinition(
            TypeDefinitionHandle definitionHandle,
            MetadataReader metadataReader,
            string assembliesDir)
        {
            var typeDefinition = metadataReader.GetTypeDefinition(definitionHandle);
            var typeName = metadataReader.GetString(typeDefinition.Name);

            yield return typeName;

            foreach (var name in GetBaseTypesNames(typeDefinition, metadataReader, assembliesDir))
                yield return name;
        }

        private static IEnumerable<string> GetTypesFromTypeReference(
            TypeReferenceHandle referenceHandle,
            MetadataReader metadataReader,
            string assembliesDir)
        {
            var typeReference = metadataReader.GetTypeReference(referenceHandle);
            var typeName = metadataReader.GetString(typeReference.Name);
            var typeNameSpace = metadataReader.GetString(typeReference.Namespace);

            yield return typeName;

            if (typeReference.ResolutionScope.Kind is not HandleKind.AssemblyReference)
                yield break;

            var assemblyReference =
                metadataReader.GetAssemblyReference((AssemblyReferenceHandle)typeReference.ResolutionScope);
            var assemblyName = assemblyReference.GetAssemblyName().Name;
            if (assemblyName is null)
                yield break;

            foreach (var name in GetBaseTypesForExternalType(typeName, typeNameSpace, assemblyName, assembliesDir))
                yield return name;
        }

        private static IEnumerable<string> GetBaseTypesForExternalType(
            string typeName,
            string typeNameSpace,
            string typeAssemblyName,
            string assembliesDir)
        {
            var assemblyPath = Path.Combine(assembliesDir, $"{typeAssemblyName}.dll");

            if (!File.Exists(assemblyPath))
                return Array.Empty<string>();

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

            return GetBaseTypesNames(typeDefinition, metadataReader, assembliesDir);
        }
    }
}
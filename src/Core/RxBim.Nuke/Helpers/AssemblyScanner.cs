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
            var list = new List<AssemblyType>();
            
            if (!File.Exists(assemblyFilePath))
                return list;

            var assembliesDir = Path.GetDirectoryName(assemblyFilePath);
            if (assembliesDir is null)
                return list;

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

                list.Add(new AssemblyType(
                    Path.GetFileNameWithoutExtension(assemblyFilePath), fullName, types));
            }

            return list;
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
            var list = new List<string>();
            
            var typeDefinition = metadataReader.GetTypeDefinition(definitionHandle);
            var typeName = metadataReader.GetString(typeDefinition.Name);

            list.Add(typeName);

            list.AddRange(GetBaseTypesNames(typeDefinition, metadataReader, assembliesDir));

            return list;
        }

        private static IEnumerable<string> GetTypesFromTypeReference(
            TypeReferenceHandle referenceHandle,
            MetadataReader metadataReader,
            string assembliesDir)
        {
            var list = new List<string>();
            
            var typeReference = metadataReader.GetTypeReference(referenceHandle);
            var typeName = metadataReader.GetString(typeReference.Name);
            var typeNameSpace = metadataReader.GetString(typeReference.Namespace);

            list.Add(typeName);

            if (typeReference.ResolutionScope.Kind is not HandleKind.AssemblyReference)
                return list;

            var assemblyReference =
                metadataReader.GetAssemblyReference((AssemblyReferenceHandle)typeReference.ResolutionScope);
            var assemblyName = assemblyReference.GetAssemblyName().Name;
            if (assemblyName is null)
                return list;

            list.AddRange(GetBaseTypesForExternalType(typeName, typeNameSpace, assemblyName, assembliesDir));

            return list;
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
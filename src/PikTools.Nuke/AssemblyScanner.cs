﻿﻿namespace PikTools.Nuke
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;

    public class AssemblyScanner
    {
        public IEnumerable<AssemblyType> Scan(string file)
        {
            if (File.Exists(file))
            {
                var mr = GetMetadataReader(file);

                foreach (var typeDefinitionHandle in mr.TypeDefinitions)
                {
                    var typeDefinition = mr.GetTypeDefinition(typeDefinitionHandle);
                    var baseTypeName = GetBaseTypeName(mr, typeDefinition);
                    var fullName = GetFullName(mr, typeDefinition);
                    if (string.IsNullOrEmpty(fullName))
                    {
                        continue;
                    }

                    yield return new AssemblyType(fullName, baseTypeName);
                }
            }
        }

        private MetadataReader GetMetadataReader(string file)
        {
            var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var peReader = new PEReader(fs);

            MetadataReader mr = peReader.GetMetadataReader();
            return mr;
        }

        private string GetBaseTypeName(MetadataReader mr, TypeDefinition typeDefinition)
        {
            var baseTypeDefinition = typeDefinition.BaseType;
            string baseTypeName = null;
            if (!baseTypeDefinition.IsNil)
            {
                try
                {
                    var referenceHandle = (TypeReferenceHandle)baseTypeDefinition;
                    var typeReference = mr.GetTypeReference(referenceHandle);
                    baseTypeName = mr.GetString(typeReference.Name);
                }
                catch
                {
                    //ignore
                }
            }

            return baseTypeName;
        }

        private string GetFullName(MetadataReader mr, TypeDefinition typeDefinition)
        {
            try
            {
                var ns = mr.GetString(typeDefinition.Namespace);
                var name = mr.GetString(typeDefinition.Name);
                return $"{ns}.{name}";
            }
            catch
            {
                //ignore
            }

            return null;
        }
    }
}
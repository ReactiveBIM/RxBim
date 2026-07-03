#if NETCOREAPP
namespace RxBim.Shared;

using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

/// <summary>
/// Provides methods for reading and analyzing assembly metadata without loading the assembly into the execution context.
/// </summary>
public static class AssemblyMetadataReader
{
    /// <summary>
    /// Checks if the specified assembly has specific attribute without actually loading the assembly.
    /// </summary>
    /// <param name="assemblyPath">Path to the assembly file.</param>
    /// <param name="attributeName">Name of the attribute to check for.</param>
    public static bool HasAttribute(string assemblyPath, string attributeName)
    {
        try
        {
            using var fs = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fs);

            if (!peReader.HasMetadata)
                return false;

            var reader = peReader.GetMetadataReader();

            foreach (var assemblyAttributeHandle in reader.CustomAttributes)
            {
                var attribute = reader.GetCustomAttribute(assemblyAttributeHandle);
                var ctorHandle = attribute.Constructor;

                string? currentAttrName = null;

                if (ctorHandle.Kind == HandleKind.MemberReference)
                {
                    var memberRef = reader.GetMemberReference((MemberReferenceHandle)ctorHandle);
                    var typeRef = reader.GetTypeReference((TypeReferenceHandle)memberRef.Parent);
                    currentAttrName = reader.GetString(typeRef.Name);
                }
                else if (ctorHandle.Kind == HandleKind.MethodDefinition)
                {
                    var methodDef = reader.GetMethodDefinition((MethodDefinitionHandle)ctorHandle);
                    var typeDefDetails = reader.GetTypeDefinition(methodDef.GetDeclaringType());
                    currentAttrName = reader.GetString(typeDefDetails.Name);
                }

                // 4. Сравниваем имя (без учета пространства имен для простоты)
                if (currentAttrName == attributeName || currentAttrName == attributeName + "Attribute")
                {
                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            // Доступ к файлу заблокирован
            Debug.WriteLine(e);
        }

        return false;
    }
}
#endif
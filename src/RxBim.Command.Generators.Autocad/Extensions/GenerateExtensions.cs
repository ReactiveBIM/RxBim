namespace RxBim.Command.Generators.Autocad.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using static System.String;
    using static Constants;

    /// <summary>
    /// Расширения для генерации
    /// </summary>
    public static class GenerateExtensions
    {
        /// <summary>
        /// Извлекает имя команды
        /// </summary>
        /// <param name="nodes">Токены описания атрибута</param>
        public static string ReadCommandName(this IEnumerable<SyntaxToken> nodes)
        {
            return nodes?.FirstOrDefault(x => x.Kind() == SyntaxKind.StringLiteralToken).ValueText ?? Empty;
        }

        /// <summary>
        /// Извлекает командные флаги в строитель строки
        /// </summary>
        /// <param name="nodes">Токены описания атрибута</param>
        public static string ReadCommandFlags(this IReadOnlyList<SyntaxToken> nodes)
        {
            if (nodes == null)
            {
                return Empty;
            }

            var builder = new StringBuilder();

            for (var i = 0; i < nodes.Count - 2; i++)
            {
                var current = nodes[i];
                if (current.Kind() != SyntaxKind.IdentifierToken)
                {
                    continue;
                }

                if (current.ValueText != "CommandFlags")
                {
                    continue;
                }

                if (nodes[i + 1].Kind() != SyntaxKind.DotToken)
                {
                    continue;
                }

                var currentValue = nodes[i + 2];
                if (currentValue.Kind() != SyntaxKind.IdentifierToken)
                {
                    continue;
                }

                if (builder.Length > 0)
                {
                    builder.Append(FlagsSeparator);
                }

                builder.Append($"{CommandFlags}.{currentValue.ValueText}");
            }

            return builder.ToString();
        }

        /// <summary>Выполнить скрипт из файла ресурсов</summary>
        /// /// <param name="assembly">Сборка для поиска ресурса</param>
        /// <param name="resourceName">Имя файла ресурса</param>
        public static string ReadTextFromResource(this Assembly assembly, string resourceName)
        {
            var name = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(resourceName));
            if (name == null)
            {
                throw new ArgumentException(
                    $"Resource '{resourceName}' not found in assembly '{assembly.FullName}'");
            }

            using (var manifestResourceStream = assembly.GetManifestResourceStream(name))
            {
                if (manifestResourceStream is null)
                {
                    throw new InvalidOperationException(
                        $"Can't get manifest for assembly '{assembly.FullName}'");
                }

                using (var streamReader = new StreamReader(manifestResourceStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
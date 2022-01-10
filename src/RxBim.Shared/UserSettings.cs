namespace RxBim.Shared
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Abstractions;

    /// <inheritdoc />
    public class UserSettings : IUserSettings
    {
        /// <inheritdoc />
        public void Set(object data, string nodeName = null)
        {
            var type = data.GetType();
            var typeName = type.Name;

            if (type.IsValueType && string.IsNullOrEmpty(nodeName))
                throw new ArgumentException($"Чтобы сохранить {typeName} требуется указать переменную {nameof(nodeName)}", nameof(nodeName));

            var callingAssembly = Assembly.GetCallingAssembly();
            var xDoc = LoadOrCreate(callingAssembly);

            var serializer = new XmlSerializer(type);
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, data);
                stream.Position = 0;

                using (var reader = XmlReader.Create(stream))
                {
                    var element = XElement.Load(reader);

                    // remove xmlns attribute
                    element.Attributes()
                        .Where(e => e.IsNamespaceDeclaration)
                        .Remove();

                    if (nodeName != null)
                    {
                        element.Name = nodeName;
                    }
                    else
                    {
                        nodeName = typeName;
                    }

                    var exist = xDoc.Element(nodeName);
                    exist?.Remove();

                    xDoc.Add(element);
                }
            }

            Save(xDoc, callingAssembly);
        }

        /// <inheritdoc />
        public T Get<T>(string nodeName = null)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            var type = typeof(T);
            var typeName = type.Name;
            try
            {
                if (type.IsValueType && string.IsNullOrEmpty(nodeName))
                    throw new ArgumentException($"Чтобы загрузить {typeName} требуется указать переменную {nameof(nodeName)}", nameof(nodeName));

                var element = LoadOrCreate(callingAssembly).Element(nodeName ?? typeName);
                if (element != null)
                {
                    var xmlRoot = new XmlRootAttribute
                    {
                        ElementName = nodeName ?? typeName,
                        IsNullable = !type.IsValueType
                    };

                    var serializer = new XmlSerializer(type, xmlRoot);

                    using var ms = element.CreateReader();
                    return (T)serializer.Deserialize(ms);
                }
            }
            catch
            {
                // ignore
            }

            return type == typeof(string) ? (T)(object)string.Empty : Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <param name="xDoc">XML элемент</param>
        /// <param name="callingAssembly">Сборка</param>
        /// <remarks>Метода исправлен на виртуальный, для того что бы переопределить сохранение во внутренюю переменную в тесте</remarks>
        public virtual void Save(XElement xDoc, Assembly callingAssembly)
        {
            var fileName = GetSettingsFileName(callingAssembly);
            using var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            xDoc.Save(fStream);
        }

        /// <summary>
        /// Загрузить или создать файл
        /// </summary>
        /// <param name="callingAssembly">Сборка.</param>
        /// <remarks>Метода исправлен на виртуальный, для того что бы переопределить сохранение во внутренюю переменную в тесте</remarks>
        public virtual XElement LoadOrCreate(Assembly callingAssembly)
        {
            var fileName = GetSettingsFileName(callingAssembly);
            if (File.Exists(fileName))
            {
                try
                {
                    using var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    return XElement.Load(fStream);
                }
                catch
                {
                    // ignore
                }
            }

            return new XElement("UserSettings");
        }

        private string GetSettingsFileName(Assembly callingAssembly)
        {
            var name = callingAssembly.GetName().Name;
            var dir =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PIK Tools");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, $"{name}.settings");
        }
    }
}

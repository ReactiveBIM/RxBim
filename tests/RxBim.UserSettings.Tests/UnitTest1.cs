namespace RxBim.UserSettings.Tests
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;
    using Shared;
    using Xunit;
    using Assert = Xunit.Assert;

    /// <summary>
    /// Test for UserSettings class method
    /// </summary>
    public class UserSettingsTest
    {
        /// <summary>
        /// Get not initialized properties
        /// </summary>
        [Fact]
        public void GetDefaultProperties()
        {
            var userSettingsService = new CustomUserSettingService();
            var testString = "Hello world";
            var testIntNum = 123;
            var testClass = new TestClass();
            var secondTestVariantClass = new TestClass
            {
                IntProperty = 321,
                StringProperty = "Bye world"
            };
            
            // set saved value in service
            userSettingsService.Set(testString, nameof(testString));
            userSettingsService.Set(testIntNum, nameof(testIntNum));
            userSettingsService.Set(testClass, nameof(testClass));
            userSettingsService.Set(secondTestVariantClass, nameof(secondTestVariantClass));
            
            // check save
            Assert.Equal(testString, userSettingsService.Get<string>(nameof(testString)));
            Assert.Equal(string.Empty, userSettingsService.Get<string>("defaultString"));
            Assert.Equal(testIntNum, userSettingsService.Get<int>(nameof(testIntNum)));
            Assert.Equal(0, userSettingsService.Get<int>("defaultInt"));
            Assert.Equal(testClass, userSettingsService.Get<TestClass>(nameof(testClass)));
            Assert.Equal(testClass, userSettingsService.Get<TestClass>("defaultTestClass"));
            Assert.Equal(secondTestVariantClass, userSettingsService.Get<TestClass>(nameof(secondTestVariantClass)));
        }
    }

    /// <summary>
    /// Class for test service work
    /// </summary>
    public class CustomUserSettingService : UserSettings
    {
        private XElement? _xElement;
        
        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <param name="xDoc">XML элемент</param>
        /// <param name="callingAssembly">Сборка</param>
        protected override void Save(XElement xDoc, Assembly callingAssembly)
        {
            _xElement = xDoc;
        }

        /// <summary>
        /// Загрузить или создать файл
        /// </summary>
        /// <param name="callingAssembly">Сборка.</param>
        protected override XElement LoadOrCreate(Assembly callingAssembly)
        {
            return _xElement ?? new XElement("UserSettings");
        }
    }
    
    /// <summary>
    /// Test class, with default field
    /// </summary>
    public class TestClass : IEquatable<TestClass>
    {
        /// <summary>
        /// String property
        /// </summary>
        public string StringProperty { get; set; } = "Hello world";

        /// <summary>
        /// Integer Property
        /// </summary>
        public int IntProperty { get; set; } = 123;

        /// <inheritdoc />
        public bool Equals(TestClass other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return StringProperty == other.StringProperty && IntProperty == other.IntProperty;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            if (ReferenceEquals(this, obj)) 
                return true;
            if (obj.GetType() != this.GetType()) 
                return false;
            return Equals((TestClass)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(StringProperty, IntProperty);
        }
    }
}
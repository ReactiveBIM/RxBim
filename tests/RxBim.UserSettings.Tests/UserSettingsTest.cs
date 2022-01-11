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
        private readonly CustomUserSettingService _userSettingService;
        
        /// <summary>
        /// ctor
        /// </summary>
        public UserSettingsTest()
        {
            _userSettingService = new CustomUserSettingService();
        }
        
        /// <summary>
        /// Test for existed integer node
        /// </summary>
        [Fact]
        public void IntegerExistedNodeCase()
        {
            var testIntNum = 123;

            // set saved value in service
            _userSettingService.Set(testIntNum, nameof(testIntNum));

            // check save
            Assert.Equal(testIntNum, _userSettingService.Get<int>(nameof(testIntNum)));
        }
        
        /// <summary>
        /// Test for not existed integer node
        /// </summary>
        [Fact]
        public void IntegerNotExistedNodeCase()
        {
            // check save
            Assert.Equal(0, _userSettingService.Get<int>("defaultInt"));
        }
        
        /// <summary>
        /// Test for existed string node
        /// </summary>
        [Fact]
        public void StringNodeCase()
        {
            var testString = "Hello world";

            // set saved value in service
            _userSettingService.Set(testString, nameof(testString));

            // check save
            Assert.Equal(testString, _userSettingService.Get<string>(nameof(testString)));
        }
        
        /// <summary>
        /// Test for not existed string node
        /// </summary>
        [Fact]
        public void NotExistedStringNodeCase()
        {
            // check save
            Assert.Equal(string.Empty, _userSettingService.Get<string>("defaultString"));
        }
        
        /// <summary>
        /// Test standard exemplar of test class
        /// </summary>
        [Fact]
        public void StandardClassNodeCase()
        {
            var testClass = new TestClass();

            // set saved value in service
            _userSettingService.Set(testClass, nameof(testClass));

            // check save
            Assert.Equal(testClass, _userSettingService.Get<TestClass>(nameof(testClass)));
        }
        
        /// <summary>
        /// Test edited exemplar of test class
        /// </summary>
        [Fact]
        public void EditedClassNodeCase()
        {
            var secondTestVariantClass = new TestClass
            {
                IntProperty = 321,
                StringProperty = "Bye world"
            };
            
            // set saved value in service
            _userSettingService.Set(secondTestVariantClass, nameof(secondTestVariantClass));
            
            // check save
            Assert.Equal(secondTestVariantClass, _userSettingService.Get<TestClass>(nameof(secondTestVariantClass)));
        }
        
        /// <summary>
        /// Test not exist exemplar of test class
        /// </summary>
        [Fact]
        public void ClassNotExistNodeCase()
        {
            // check save
            Assert.Equal(new TestClass(), _userSettingService.Get<TestClass>("defaultTestClass"));
        }
    }

    /// <summary>
    /// Class for test service work
    /// </summary>
    public class CustomUserSettingService : UserSettings
    {
        private XElement? _xElement;

        /// <inheritdoc />
        protected override void Save(XElement xDoc, Assembly callingAssembly)
        {
            _xElement = xDoc;
        }

        /// <inheritdoc />
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
        public bool Equals(TestClass? other)
        {
            if (ReferenceEquals(null, other)) 
                return false;
            if (ReferenceEquals(this, other)) 
                return true;
            return StringProperty == other.StringProperty && IntProperty == other.IntProperty;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
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
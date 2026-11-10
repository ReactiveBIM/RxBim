namespace RxBim.Application.Ribbon.Tests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using ItemFromConfigStrategies;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    public class CommandButtonFromConfigStrategyTests
    {
        private readonly CommandButtonFromConfigStrategy _commandStrategy = new();
        private readonly ToggleCommandButtonFromConfigStrategy _toggleStrategy = new();

        [Theory]
        [InlineData(null, true, false)]
        [InlineData("false", true, false)]
        [InlineData("true", false, true)]
        public void IsApplicableRespectsIsToggleValue(
            string? isToggleValue,
            bool expectCommand,
            bool expectToggle)
        {
            var values = new Dictionary<string, string?>
            {
                ["Item:CommandType"] = "Some.Command"
            };
            if (isToggleValue is not null)
                values["Item:IsToggle"] = isToggleValue;

            var section = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build()
                .GetSection("Item");

            _commandStrategy.IsApplicable(section).Should().Be(expectCommand);
            _toggleStrategy.IsApplicable(section).Should().Be(expectToggle);
        }
    }
}
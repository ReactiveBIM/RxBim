namespace RxBim.Application.Ribbon.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class RibbonConfigurationTests
    {
        [Theory]
        [InlineData("Items")]
        [InlineData("Panels")]
        public void ShouldLoadPanelsFromSupportedSection(string panelsSectionName)
        {
            var values = new Dictionary<string, string?>
            {
                ["Ribbon:Tabs:0:Name"] = "Tab",
                [$"Ribbon:Tabs:0:{panelsSectionName}:0:Name"] = "Panel"
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();
            var services = new ServiceCollection();
            services.AddMenu<RibbonMenuBuilderStub>(configuration, typeof(RibbonConfigurationTests).Assembly);

            using var provider = services.BuildServiceProvider();
            var ribbon = provider.GetRequiredService<Ribbon>();

            ribbon.Tabs.Should().ContainSingle();
            ribbon.Tabs[0].Items.Should().ContainSingle()
                .Which.Should().BeOfType<Panel>()
                .Which.Name.Should().Be("Panel");
        }

        private sealed class RibbonMenuBuilderStub : IRibbonMenuBuilder
        {
            public event EventHandler? MenuCreated;

            public void BuildRibbonMenu(Ribbon? ribbonConfig = null)
            {
                MenuCreated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
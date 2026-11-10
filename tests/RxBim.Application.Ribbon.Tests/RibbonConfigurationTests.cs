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

        [Theory]
        [InlineData("Ribbon:Tabs:0:Name", "Ribbon:Tabs:0")]
        [InlineData("Ribbon:Tabs:0:Items:0:Name", "Ribbon:Tabs:0:Items:0")]
        public void ShouldRejectMissingName(string missingKey, string sectionPath)
        {
            var values = new Dictionary<string, string?>
            {
                ["Ribbon:Tabs:0:Name"] = "Tab",
                ["Ribbon:Tabs:0:Items:0:Name"] = "Panel",
                ["Ribbon:Tabs:0:Items:0:Visible"] = "true"
            };
            values.Remove(missingKey);
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(values).Build();
            var services = new ServiceCollection();
            services.AddMenu<RibbonMenuBuilderStub>(configuration, typeof(RibbonConfigurationTests).Assembly);
            using var provider = services.BuildServiceProvider();

            Action resolve = () => provider.GetRequiredService<Ribbon>();

            resolve.Should().Throw<InvalidOperationException>().WithMessage($"*'{sectionPath}'*");
        }

        [Fact]
        public void ShouldRequireConfigurationWhenNotProvidedExplicitly()
        {
            var services = new ServiceCollection();
            services.AddMenu<RibbonMenuBuilderStub>((IConfiguration?)null, typeof(RibbonConfigurationTests).Assembly);
            using var provider = services.BuildServiceProvider();

            Action resolve = () => provider.GetRequiredService<Ribbon>();

            resolve.Should().Throw<InvalidOperationException>().WithMessage("*IConfiguration*");
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
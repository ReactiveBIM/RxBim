namespace RxBim.Application.Ribbon.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using ConfigurationBuilders;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using Xunit;

    public class EnhancedToolTipTests
    {
        [Fact]
        public void ConcreteButtonBuilderSupportsEnhancedToolTipAsFirstFluentCall()
        {
            var builder = new CommandButtonBuilder("Command", typeof(AttributedCommand));

            var configuredBuilder = builder.EnhancedToolTip("**Details**");

            configuredBuilder.Should().BeSameAs(builder);
        }

        [Fact]
        public void FluentEnhancedToolTipDoesNotRequireNativeText()
        {
            var button = BuildButton(builder => builder.EnhancedToolTip("**Details**").Text("Command"));

            button.EnhancedToolTip.Should().Be("**Details**");
            button.ToolTip.Should().BeNull();
            button.Text.Should().Be("Command");
        }

        [Fact]
        public void AttributeLoadsEnhancedContentAlongsideNativeText()
        {
            var button = new CommandButton { CommandType = typeof(AttributedCommand).FullName };

            button.LoadFromAttribute(typeof(AttributedCommand).Assembly);

            button.EnhancedToolTip.Should().Be("**Attribute details**");
            button.ToolTip.Should().Be("Native summary");
        }

        [Theory]
        [InlineData("**Configured details**")]
        [InlineData("")]
        public void ExplicitEnhancedContentTakesPriorityOverAttribute(string configuredContent)
        {
            var button = BuildButton(builder => builder.EnhancedToolTip(configuredContent));

            button.LoadFromAttribute(typeof(AttributedCommand).Assembly);

            button.EnhancedToolTip.Should().Be(configuredContent);
        }

        [Fact]
        public void ConfigurationLoadsEnhancedContentWithoutNativeText()
        {
            var values = new Dictionary<string, string?>
            {
                ["Button:CommandType"] = typeof(AttributedCommand).FullName,
                ["Button:EnhancedToolTip"] = "[Help](https://example.com/help)"
            };
            var section = new ConfigurationBuilder().AddInMemoryCollection(values).Build().GetSection("Button");

            var button = section.Get<CommandButton>()!;

            button.EnhancedToolTip.Should().Be("[Help](https://example.com/help)");
            button.ToolTip.Should().BeNull();
        }

        [Fact]
        public void EnhancedVersionDoesNotRequireNativeText()
        {
            var menu = CreateMenuData();
            var button = new CommandButton { CommandType = typeof(AttributedCommand).FullName };

            var version = menu.GetEnhancedToolTipVersion(button);

            version.Should().Be("Version: " + typeof(AttributedCommand).Assembly.GetName().Version);
            button.ToolTip.Should().BeNull();
        }

        [Fact]
        public void EnhancedVersionHonorsDisabledVersionDisplay()
        {
            var menu = CreateMenuData();
            menu.RibbonConfiguration!.DisplayVersion = false;

            var version = menu.GetEnhancedToolTipVersion(
                new CommandButton { CommandType = typeof(AttributedCommand).FullName });

            version.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("v")]
        [InlineData("Version: ")]
        public void NativeAndEnhancedTooltipsUseSameVersionFormatting(string prefix)
        {
            var menu = CreateMenuData();
            menu.RibbonConfiguration!.VersionPrefix = prefix;
            var button = new CommandButton
            {
                CommandType = typeof(AttributedCommand).FullName,
                ToolTip = "Native",
                EnhancedToolTip = "**Details**"
            };

            var nativeText = menu.GetTooltipContent(button, typeof(AttributedCommand));
            var enhancedVersion = menu.GetEnhancedToolTipVersion(button);

            enhancedVersion.Should().Be(prefix + typeof(AttributedCommand).Assembly.GetName().Version);
            nativeText.Should().Be("Native" + Environment.NewLine + enhancedVersion);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  \r\n ")]
        public void EmptyEnhancedContentDoesNotRegisterPopup(string? markdown)
        {
            RunOnUiThread(() =>
            {
                using var service = new EnhancedToolTipService(() => null, _ => { });

                var registered = service.Register(new object(), new CommandButton { EnhancedToolTip = markdown });

                registered.Should().BeFalse();
                service.TryOpenHelp().Should().BeFalse();
            });
        }

        [Fact]
        public void RegisteredPopupDoesNotOpenHelpBeforeItIsVisible()
        {
            RunOnUiThread(() =>
            {
                var helpCalls = 0;
                using var service = new EnhancedToolTipService(() => null, _ => helpCalls++);
                service.Register(new object(), new CommandButton
                {
                    EnhancedToolTip = "Details",
                    HelpUrl = "https://example.com/help"
                });

                service.TryOpenHelp().Should().BeFalse();

                helpCalls.Should().Be(0);
                service.Clear();
            });
        }

        private static MenuData CreateMenuData()
        {
            return new MenuData
            {
                MenuAssembly = typeof(AttributedCommand).Assembly,
                RibbonConfiguration = new Ribbon { DisplayVersion = true, VersionPrefix = "Version: " }
            };
        }

        private static CommandButton BuildButton(Action<ICommandButtonBuilder> configure)
        {
            var services = new ServiceCollection();
            services.AddMenu<MenuBuilderStub>(ribbon => ribbon.Tab("Tab", tab => tab.Panel("Panel", panel =>
                panel.CommandButton<AttributedCommand>("Command", configure))), typeof(AttributedCommand).Assembly);
            using var provider = services.BuildServiceProvider();
            var ribbon = provider.GetRequiredService<Ribbon>();
            return (CommandButton)((Panel)ribbon.Tabs[0].Items[0]).Items[0];
        }

        private static void RunOnUiThread(Action action)
        {
            Exception? failure = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    failure = exception;
                }
            }) { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(TimeSpan.FromSeconds(10)).Should().BeTrue("the WPF check must complete");

            if (failure is not null)
                ExceptionDispatchInfo.Capture(failure).Throw();
        }

        [RxBimCommand(ToolTip = "Native summary", EnhancedToolTip = "**Attribute details**")]
        private sealed class AttributedCommand;

        private sealed class MenuBuilderStub : IRibbonMenuBuilder
        {
            public event EventHandler? MenuCreated;

            public void BuildRibbonMenu(Ribbon? ribbonConfig = null)
            {
                MenuCreated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
namespace RxBim.Application.Ribbon.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ConfigurationBuilders;
    using FluentAssertions;
    using Markdown;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using Xunit;
    using Border = System.Windows.Controls.Border;
    using FlowDocumentScrollViewer = System.Windows.Controls.FlowDocumentScrollViewer;
    using Image = System.Windows.Controls.Image;

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

        [Fact]
        public void DefaultRendererDisplaysMarkupLiterally()
        {
            RunOnUiThread(() =>
            {
                using var provider = CreateMenuServices().BuildServiceProvider();
                var renderer = provider.GetRequiredService<IEnhancedToolTipRenderer>();
                const string content = "**Details** &amp; [Help](https://example.com/help)";

                var card = (Border)EnhancedToolTipContent.Create(string.Empty, content, null, null, renderer);

                var document = ((FlowDocumentScrollViewer)card.Child).Document;
                GetDocumentText(document).Should().Be(content);
                var paragraph = document.Blocks.OfType<Paragraph>().Should().ContainSingle().Subject;
                paragraph.Inlines.Should().ContainSingle().Which.Should().BeOfType<Run>();
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MarkdownRegistrationWorksBeforeOrAfterMenuRegistration(bool registerMarkdownFirst)
        {
            RunOnUiThread(() =>
            {
                var services = new ServiceCollection();
                if (registerMarkdownFirst)
                    services.AddMarkdownToolTips();

                services.AddMenu<MenuBuilderStub>(_ => { }, typeof(AttributedCommand).Assembly);
                if (!registerMarkdownFirst)
                    services.AddMarkdownToolTips();

                using var provider = services.BuildServiceProvider();
                var renderer = provider.GetRequiredService<IEnhancedToolTipRenderer>();

                var document = renderer.Render("**Details**", 360);

                provider.GetServices<IEnhancedToolTipRenderer>().Should().ContainSingle();
                var paragraph = document.Blocks.OfType<Paragraph>().Should().ContainSingle().Subject;
                paragraph.Inlines.Should().Contain(inline => inline.FontWeight == FontWeights.Bold);
                GetDocumentText(document).Should().Be("Details");
            });
        }

        [Theory]
        [InlineData("A &amp; B", "A & B")]
        [InlineData("&lt;b&gt;", "<b>")]
        [InlineData("&#38; &#x26;", "& &")]
        [InlineData("&amp;lt;", "&lt;")]
        [InlineData("`&amp;`", "&amp;")]
        public void MarkdownEntitiesPreserveExpectedText(string markdown, string expectedText)
        {
            RunOnUiThread(() =>
            {
                var document = RenderMarkdown(markdown);

                GetDocumentText(document).Should().Be(expectedText);
            });
        }

        [Theory]
        [InlineData("<https://example.com/help>", "https://example.com/help", "https://example.com/help")]
        [InlineData("<http://example.com/help>", "http://example.com/help", "http://example.com/help")]
        [InlineData("[Help](https://example.com/help)", "Help", "https://example.com/help")]
        public void MarkdownWebLinksRenderClickableText(string markdown, string expectedText, string expectedAddress)
        {
            RunOnUiThread(() =>
            {
                var document = RenderMarkdown(markdown);

                GetDocumentText(document).Should().Be(expectedText);
                var paragraph = document.Blocks.OfType<Paragraph>().Should().ContainSingle().Subject;
                var link = paragraph.Inlines.OfType<Hyperlink>().Should().ContainSingle().Subject;
                link.NavigateUri.AbsoluteUri.Should().Be(expectedAddress);
            });
        }

        [Theory]
        [InlineData("user@example.com", "mailto:user@example.com")]
        [InlineData("ftp://example.com/file", "ftp://example.com/file")]
        public void MarkdownOtherAutolinksRenderClickableText(string address, string expectedAddress)
        {
            RunOnUiThread(() =>
            {
                var document = RenderMarkdown("<" + address + ">");

                GetDocumentText(document).Should().Be(address);
                var paragraph = document.Blocks.OfType<Paragraph>().Should().ContainSingle().Subject;
                var link = paragraph.Inlines.OfType<Hyperlink>().Should().ContainSingle().Subject;
                link.NavigateUri.OriginalString.Should().Be(expectedAddress);
            });
        }

        [Fact]
        public void SharedMarkdownRendererCreatesTooltipTables()
        {
            RunOnUiThread(() =>
            {
                var document = RenderMarkdown("| Name | Value |\n| --- | --- |\n| Width | 900 |");

                var table = document.Blocks.OfType<Table>().Should().ContainSingle().Subject;
                table.Columns.Count.Should().Be(2);
                var rows = table.RowGroups.Single().Rows;
                rows.Count.Should().Be(2);
                GetDocumentText(document).Should().Contain("Width").And.Contain("900");
            });
        }

        [Fact]
        public void SharedMarkdownRendererCreatesTooltipImagesWithinCardWidth()
        {
            RunOnUiThread(() =>
            {
                var bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgra32, null, new byte[4], 4);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using var stream = new MemoryStream();
                encoder.Save(stream);
                var markdown = "![Plan](data:image/png;base64," + Convert.ToBase64String(stream.ToArray()) + ")";

                var document = RenderMarkdown(markdown);

                var paragraph = document.Blocks.OfType<Paragraph>().Should().ContainSingle().Subject;
                var container = paragraph.Inlines.OfType<InlineUIContainer>().Should().ContainSingle().Subject;
                var image = container.Child.Should().BeOfType<Image>().Subject;
                image.Source.Should().BeAssignableTo<BitmapSource>().Subject.PixelWidth.Should().Be(1);
                image.MaxWidth.Should().Be(360);
            });
        }

        [Fact]
        public void SharedMarkdownRendererKeepsTitleHelpAndVersionSeparateFromBody()
        {
            RunOnUiThread(() =>
            {
                const string title = "**Command**";
                const string version = "Version **literal**";

                var card = (Border)EnhancedToolTipContent.Create(
                    title, "**Details**", "https://example.com/help", version, new MarkdigEnhancedToolTipRenderer());

                var document = ((FlowDocumentScrollViewer)card.Child).Document;
                var heading = document.Blocks.FirstBlock.Should().BeOfType<Paragraph>().Subject;
                new TextRange(heading.ContentStart, heading.ContentEnd).Text.TrimEnd('\r', '\n').Should().Be(title);
                heading.FontWeight.Should().Be(FontWeights.Bold);
                var footer = document.Blocks.LastBlock.Should().BeOfType<Paragraph>().Subject;
                var help = footer.Inlines.OfType<Hyperlink>().Should().ContainSingle().Subject;
                help.NavigateUri.AbsoluteUri.Should().Be("https://example.com/help");
                help.Tag.Should().BeSameAs(EnhancedToolTipContent.HelpLinkTag);
                footer.Inlines.OfType<Run>().Should().ContainSingle().Subject.Text.Should().Be(version);
                GetDocumentText(document).Should().Contain("Details");
            });
        }

        [Theory]
        [InlineData("mailto:user@example.com")]
        [InlineData("ftp://example.com/file")]
        [InlineData("file:///C:/manual.pdf")]
        [InlineData("bimteam:help")]
        [InlineData("manual.md")]
        [InlineData("#section")]
        public void MarkdownNonWebLinksKeepTheirLabelsAndNavigation(string address)
        {
            RunOnUiThread(() =>
            {
                var document = RenderMarkdown("[**Help**](" + address + ")");

                GetDocumentText(document).Should().Be("Help");
                var paragraph = document.Blocks.OfType<Paragraph>().Should().ContainSingle().Subject;
                var link = paragraph.Inlines.OfType<Hyperlink>().Should().ContainSingle().Subject;
                link.NavigateUri.OriginalString.Should().Be(address);
            });
        }

        private static FlowDocument RenderMarkdown(string markdown)
        {
            var services = CreateMenuServices();
            services.AddMarkdownToolTips();
            using var provider = services.BuildServiceProvider();
            var renderer = provider.GetRequiredService<IEnhancedToolTipRenderer>();
            var card = (Border)EnhancedToolTipContent.Create(string.Empty, markdown, null, null, renderer);
            return ((FlowDocumentScrollViewer)card.Child).Document;
        }

        private static IServiceCollection CreateMenuServices()
        {
            var services = new ServiceCollection();
            services.AddMenu<MenuBuilderStub>(_ => { }, typeof(AttributedCommand).Assembly);
            return services;
        }

        private static string GetDocumentText(FlowDocument document) =>
            new TextRange(document.ContentStart, document.ContentEnd).Text.TrimEnd('\r', '\n');

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
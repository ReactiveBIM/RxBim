namespace RxBim.Application.Ribbon.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class ThemedRibbonButtonServiceTests
    {
        [Fact]
        public void RegisterAppliesImagesAndIgnoresSameButtonInstance()
        {
            var context = new TestContext();
            var button = new TestButton();
            var config = new CommandButton();

            context.Service.Register(button, config);
            context.Service.Register(button, config);

            context.Adapter.ApplyCallCount.Should().Be(1);
        }

        [Fact]
        public void ThemeChangedAppliesImagesToAllRegisteredButtons()
        {
            var context = new TestContext();
            context.Service.Register(new TestButton(), new CommandButton());
            context.Service.Register(new TestButton(), new CommandButton());
            context.Service.Run();

            context.ThemeService.RaiseThemeChanged();

            context.Adapter.ApplyCallCount.Should().Be(4);
        }

        [Fact]
        public void ClearRemovesRegisteredButtons()
        {
            var context = new TestContext();
            context.Service.Register(new TestButton(), new CommandButton());
            context.Service.Run();
            context.Service.Clear();

            context.ThemeService.RaiseThemeChanged();

            context.Adapter.ApplyCallCount.Should().Be(1);
        }

        [Fact]
        public void RunIsIdempotent()
        {
            var context = new TestContext();

            context.Service.Run();
            context.Service.Run();

            context.ThemeService.RunCallCount.Should().Be(1);
            context.ThemeService.SubscriptionCount.Should().Be(1);
        }

        [Fact]
        public void DisposeStopsThemeUpdates()
        {
            var context = new TestContext();
            context.Service.Register(new TestButton(), new CommandButton());
            context.Service.Run();

            context.Service.Dispose();
            context.ThemeService.RaiseThemeChanged();

            context.Adapter.ApplyCallCount.Should().Be(1);
            context.ThemeService.SubscriptionCount.Should().Be(0);
        }

        private sealed class TestContext
        {
            public TestContext()
            {
                Service = new ThemedRibbonButtonService<TestButton>(ThemeService, ImageProvider, Adapter);
            }

            public TestColorThemeService ThemeService { get; } = new();

            public TestButtonImageProvider ImageProvider { get; } = new();

            public TestRibbonButtonImageAdapter Adapter { get; } = new();

            public ThemedRibbonButtonService<TestButton> Service { get; }
        }

        private sealed class TestButton;

        private sealed class TestColorThemeService : IColorThemeService
        {
            private EventHandler? _themeChanged;

            public event EventHandler? ThemeChanged
            {
                add
                {
                    _themeChanged += value;
                    SubscriptionCount++;
                }

                remove
                {
                    _themeChanged -= value;
                    SubscriptionCount--;
                }
            }

            public int RunCallCount { get; private set; }

            public int SubscriptionCount { get; private set; }

            public void Run()
            {
                RunCallCount++;
            }

            public ThemeType GetCurrentTheme()
            {
                return ThemeType.Dark;
            }

            public void RaiseThemeChanged()
            {
                _themeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private sealed class TestButtonImageProvider : IButtonImageProvider
        {
            public ButtonImages GetImages(Button buttonConfig)
            {
                return new ButtonImages(null, null);
            }
        }

        private sealed class TestRibbonButtonImageAdapter : IRibbonButtonImageAdapter<TestButton>
        {
            public int ApplyCallCount { get; private set; }

            public void ApplyImages(TestButton button, ButtonImages images)
            {
                ApplyCallCount++;
            }
        }
    }
}
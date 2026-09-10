namespace RxBim.Application.Ribbon.Tests
{
    using FluentAssertions;
    using Xunit;

    public class ButtonTests
    {
        [Theory]
        [InlineData(ThemeType.All, "Image.png", "LargeImage.png")]
        [InlineData(ThemeType.Dark, "Image.png", "LargeImage.png")]
        [InlineData(ThemeType.Light, "ImageLight.png", "LargeImageLight.png")]
        public void ResolveImagePathsReturnsThemeImages(
            ThemeType themeType,
            string expectedImage,
            string expectedLargeImage)
        {
            var button = new CommandButton
            {
                Image = "Image.png",
                ImageLight = "ImageLight.png",
                LargeImage = "LargeImage.png",
                LargeImageLight = "LargeImageLight.png"
            };

            button.ResolveImagePath(themeType).Should().Be(expectedImage);
            button.ResolveLargeImagePath(themeType).Should().Be(expectedLargeImage);
        }

        [Fact]
        public void ResolveImagePathsUsesDefaultImagesWhenLightImagesAreMissing()
        {
            var button = new CommandButton
            {
                Image = "Image.png",
                LargeImage = "LargeImage.png"
            };

            button.ResolveImagePath(ThemeType.Light).Should().Be("Image.png");
            button.ResolveLargeImagePath(ThemeType.Light).Should().Be("LargeImage.png");
        }

        [Fact]
        public void ResolveImagePathsReturnsNullWhenImagesAreMissing()
        {
            var button = new CommandButton();

            button.ResolveImagePath(ThemeType.Light).Should().BeNull();
            button.ResolveLargeImagePath(ThemeType.Light).Should().BeNull();
        }
    }
}
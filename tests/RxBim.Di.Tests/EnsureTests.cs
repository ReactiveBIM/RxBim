namespace RxBim.Di.Tests
{
    using System;
    using FluentAssertions;
    using Nuke.Helpers;
    using Xunit;

    public class EnsureTests
    {
        public string? Name { get; set; }

        public string? Description { get; set; } = "Description";

        [Fact]
        public void ThrowTest()
        {
            Action action = () => Name.Ensure();
            action.Should().Throw<NullReferenceException>().WithMessage($"Value of {nameof(Name)} cannot be null.");
        }
        
        [Fact]
        public void NotThrowTest()
        {
            Action action = () => Description.Ensure();
            action.Should().NotThrow();
        }
    }
}
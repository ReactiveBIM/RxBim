namespace RxBim.Nuke.Tests;

using System.Collections.Generic;
using FluentAssertions;
using Revit.Generators.Extensions;
using Revit.Generators.Models;
using Xunit;

public class ManifestSettingsTests
{
    [Fact]
    public void ShouldSerializeAllManifestSettings()
    {
        var document = CreateManifest(new ManifestSettings
        {
            UseRevitContext = false,
            ContextName = "SampleContextName"
        }).ToXDocument();

        var settings = document.Root!.Element(nameof(ManifestSettings));

        settings.Should().NotBeNull();
        settings!.Element(nameof(ManifestSettings.UseRevitContext))!.Value.Should().Be("False");
        settings.Element(nameof(ManifestSettings.ContextName))!.Value.Should().Be("SampleContextName");
    }

    [Fact]
    public void ShouldOmitUnspecifiedManifestSettings()
    {
        var document = CreateManifest(new ManifestSettings()).ToXDocument();

        document.Root!.Element(nameof(ManifestSettings)).Should().BeNull();
    }

    [Fact]
    public void ShouldSerializeContextNameWithoutUseRevitContext()
    {
        var document = CreateManifest(new ManifestSettings
        {
            ContextName = "SharedContext"
        }).ToXDocument();

        var settings = document.Root!.Element(nameof(ManifestSettings));

        settings.Should().NotBeNull();
        settings!.Element(nameof(ManifestSettings.UseRevitContext)).Should().BeNull();
        settings.Element(nameof(ManifestSettings.ContextName))!.Value.Should().Be("SharedContext");
    }

    private static RevitAddIns CreateManifest(ManifestSettings? manifestSettings)
    {
        return new RevitAddIns
        {
            AddIn = new List<AddIn>(),
            ManifestSettings = manifestSettings
        };
    }
}
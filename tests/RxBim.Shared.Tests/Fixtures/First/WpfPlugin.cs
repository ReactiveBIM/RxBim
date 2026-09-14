namespace RxBim.Shared.Tests.First;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Dependency;

/// <summary>
/// Reproduces a template lookup against a private dependency type.
/// </summary>
public sealed class WpfPlugin
{
    /// <summary>
    /// The dependency type selected by the executing C# code.
    /// </summary>
    public Type ModelType => typeof(ViewModel);

    /// <summary>
    /// Parses XAML and reports the template type and implicit lookup result.
    /// </summary>
    public object[] ReadTemplate()
    {
        // Parse directly to isolate type resolution from pack URI resolution of the view assembly.
        const string xaml = """
            <ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                                xmlns:sample="urn:rxbim:context-tests">
                <DataTemplate DataType="{x:Type sample:ViewModel}">
                    <TextBlock Text="Matched template" />
                </DataTemplate>
            </ResourceDictionary>
            """;
        var view = new ContentControl
        {
            Resources = (ResourceDictionary)XamlReader.Parse(xaml),
            Content = new ViewModel()
        };
        var key = view.Resources.Keys.OfType<DataTemplateKey>().Single();
        return [key.DataType, view.TryFindResource(new DataTemplateKey(view.Content.GetType())) is DataTemplate];
    }

    /// <summary>
    /// Casts a model retained by the host from a previous invocation.
    /// </summary>
    /// <param name="model">The previously created model.</param>
    public object CastModel(object model) => (ViewModel)model;
}
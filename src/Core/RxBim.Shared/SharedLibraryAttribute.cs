namespace RxBim.Shared;

using System;

/// <summary>
/// When isolated contexts are enabled, an assembly marked with this attribute will be loaded into the shared (default) context.
/// </summary>
/// <remarks>
/// Primary purpose - shared dlls with UI. Winforms and WPF are not compatible with contexts isolation.
/// Marked library MUST NOT have dependencies, that will be loaded into isolated context.
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly)]
public class SharedLibraryAttribute : Attribute;
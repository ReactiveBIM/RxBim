using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public record ItemAttribute(string Name, string Value);

public record ProjectItem(string Name, string Value, bool IsItem, params ItemAttribute[] ItemAttributes);

public record TargetFramework(string Value) : ProjectItem(nameof(TargetFramework), Value, false, null);

public record ApplicationVersion(string Value) : ProjectItem(nameof(ApplicationVersion), Value, false, null);

public record PackageReference(string Name, string Version)
    : ProjectItem(nameof(PackageReference), string.Empty, true, new("Include", Name), new(nameof(Version), Version));

public record RuntimePackageReference(string Name, string Version)
    : ProjectItem(nameof(PackageReference), string.Empty, true, new("Include", Name), new(nameof(Version), Version), new("ExcludeAssets", "runtime"));

public abstract record Enumeration
{
    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
}

public record AppVersion(string Name, string FullName, params ProjectItem[] Properties)
    : Enumeration()
{
    public static AppVersion Revit2019 = new("Revit", "Autodesk Revit 2019",
        new ApplicationVersion("2019"),
        new TargetFramework("net472"),
        new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2019.0.1")
    );

    public static AppVersion Revit2020 = new("Revit", "Autodesk Revit 2020",
        new ApplicationVersion("2020"),
        new TargetFramework("net472"),
        new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2020.0.1")
    );

    public static AppVersion Revit2021 = new("Revit", "Autodesk Revit 2021",
        new ApplicationVersion("2021"),
        new TargetFramework("net48"),
        new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2021.1.4")
    );

    public static AppVersion Revit2022 = new("Revit", "Autodesk Revit 2022",
        new ApplicationVersion("2022"),
        new TargetFramework("net48"),
        new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2022.1.0")
    );

    public static AppVersion Revit2023 = new("Revit", "Autodesk Revit 2023",
        new ApplicationVersion("2023"),
        new TargetFramework("net48"),
        new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2023.0.0")
    );

    public static AppVersion Autocad2019 = new("Autocad", "Autodesk Autocad 2019",
        new ApplicationVersion("2019"),
        new TargetFramework("net472"),
        new RuntimePackageReference("AutoCAD2019.Base", "1.0.3")
    );

    public static AppVersion Autocad2020 = new("Autocad", "Autodesk Autocad 2020",
        new ApplicationVersion("2020"),
        new TargetFramework("net472"),
        new RuntimePackageReference("AutoCAD2020.Base", "1.0.0")
    );

    public static AppVersion Autocad2021 = new("Autocad", "Autodesk Autocad 2021",
        new ApplicationVersion("2021"),
        new TargetFramework("net48"),
        new RuntimePackageReference("AutoCAD2021.Base", "1.0.0")
    );

    public override string ToString() => FullName;
}
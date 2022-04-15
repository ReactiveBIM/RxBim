using System.Collections.Generic;

public record AppVersion(string Name, string Version, string TargetFramework, IEnumerable<PackageReference> PackageReferences) : Enumeration(Name, Version)
{
    public static AppVersion Revit2019 = new("Autodesk Revit", "2019", "net47", new PackageReference[]
    {
        new("Revit_All_Main_Versions_API_x64", "2019.0.1")
    });

    public static AppVersion Revit2020 = new("Autodesk Revit", "2020", "net47", new PackageReference[]
    {
        new("Revit_All_Main_Versions_API_x64", "2020.0.1")
    });

    public static AppVersion Revit2021 = new("Autodesk Revit", "2021", "net48", new PackageReference[]
    {
        new("Revit_All_Main_Versions_API_x64", "2021.1.4")
    });

    public static AppVersion Revit2022 = new("Autodesk Revit", "2022", "net48", new PackageReference[]
    {
        new("Revit_All_Main_Versions_API_x64", "2022.1.0")
    });

    public static AppVersion Revit2023 = new("Autodesk Revit", "2023", "net48", new PackageReference[]
    {
        new("Revit_All_Main_Versions_API_x64", "2023.0.0")
    });

    public static AppVersion Autocad2019 = new("Autodesk Autocad", "2019", "net47", new PackageReference[]
    {
        new("AutoCAD2019.Base", "1.0.3")
    });

    public static AppVersion Autocad2020 = new("Autodesk Autocad", "2020", "net472", new PackageReference[]
    {
        new("AutoCAD2020.Base", "1.0.0")
    });

    public static AppVersion Autocad2021 = new("Autodesk Autocad", "2021", "net48", new PackageReference[]
    {
        new("AutoCAD2021.Base", "1.0.0")
    });

    public override string ToString() => $"{Name} {Version}";
}
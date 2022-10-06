namespace Versions;

public record AppVersion(string AppName, string AppFullName, params ProjectItem[] Items) : Enumeration
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

    public override string ToString() => AppFullName;
}
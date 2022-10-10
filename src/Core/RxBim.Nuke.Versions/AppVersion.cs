#pragma warning disable CS1591
#pragma warning disable SA1401
#pragma warning disable SA1600

namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Settings for a particular version of the CAD application.
    /// </summary>
    /// <param name="Description">Description of the version.</param>
    /// <param name="AppType">Type of the CAD application.</param>
    /// <param name="Settings">Settings collection.</param>
    public record AppVersion(string Description, CadAppType AppType, params ProjectSetting[] Settings) : Enumeration
    {
        public static AppVersion Revit2019 = new(
            "Autodesk Revit 2019",
            CadAppType.Revit,
            new ApplicationVersion("2019"),
            new TargetFramework("net472"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2019.0.1"),
            new DefineConstants("RVT2019"));

        public static AppVersion Revit2020 = new(
            "Autodesk Revit 2020",
            CadAppType.Revit,
            new ApplicationVersion("2020"),
            new TargetFramework("net472"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2020.0.1"),
            new DefineConstants("RVT2020"));

        public static AppVersion Revit2021 = new(
            "Autodesk Revit 2021",
            CadAppType.Revit,
            new ApplicationVersion("2021"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2021.1.4"),
            new DefineConstants("RVT2021"));

        public static AppVersion Revit2022 = new(
            "Autodesk Revit 2022",
            CadAppType.Revit,
            new ApplicationVersion("2022"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2022.1.0"),
            new DefineConstants("RVT2022"));

        public static AppVersion Revit2023 = new(
            "Autodesk Revit 2023",
            CadAppType.Revit,
            new ApplicationVersion("2023"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2023.0.0"),
            new DefineConstants("RVT2023"));

        public static AppVersion Autocad2019 = new(
            "Autodesk Autocad 2019",
            CadAppType.Autocad,
            new ApplicationVersion("2019"),
            new TargetFramework("net472"),
            new RuntimePackageReference("AutoCAD2019.Base", "1.0.3"),
            new DefineConstants("ACAD2019"));

        public static AppVersion Autocad2020 = new(
            "Autodesk Autocad 2020",
            CadAppType.Autocad,
            new ApplicationVersion("2020"),
            new TargetFramework("net472"),
            new RuntimePackageReference("AutoCAD2020.Base", "1.0.0"),
            new DefineConstants("ACAD2020"));

        public static AppVersion Autocad2021 = new(
            "Autodesk Autocad 2021",
            CadAppType.Autocad,
            new ApplicationVersion("2021"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD2021.Base", "1.0.0"),
            new DefineConstants("ACAD2021"));

        public static AppVersion Autocad2022 = new(
            "Autodesk Autocad 2022",
            CadAppType.Autocad,
            new ApplicationVersion("2022"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD2021.Base", "1.0.0"),
            new DefineConstants("ACAD2022"));

        public static AppVersion Autocad2023 = new(
            "Autodesk Autocad 2023",
            CadAppType.Autocad,
            new ApplicationVersion("2023"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD2021.Base", "1.0.0"),
            new DefineConstants("ACAD2023"));

        /// <inheritdoc />
        public override string ToString() => Description;
    }
}
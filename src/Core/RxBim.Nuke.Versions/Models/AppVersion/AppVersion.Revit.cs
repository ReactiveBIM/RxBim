#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211

namespace RxBim.Nuke.Versions
{
    public partial class AppVersion
    {
        public static AppVersion Revit2019 = new(
            "Autodesk Revit 2019",
            AppType.Revit,
            new ApplicationVersion("2019"),
            new TargetFramework("net472"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2019.0.1"),
            new DefineConstants("RVT2019"));

        public static AppVersion Revit2020 = new(
            "Autodesk Revit 2020",
            AppType.Revit,
            new ApplicationVersion("2020"),
            new TargetFramework("net472"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2020.0.1"),
            new DefineConstants("RVT2020"));

        public static AppVersion Revit2021 = new(
            "Autodesk Revit 2021",
            AppType.Revit,
            new ApplicationVersion("2021"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2021.1.4"),
            new DefineConstants("RVT2021"));

        public static AppVersion Revit2022 = new(
            "Autodesk Revit 2022",
            AppType.Revit,
            new ApplicationVersion("2022"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2022.1.0"),
            new DefineConstants("RVT2022"));

        public static AppVersion Revit2023 = new(
            "Autodesk Revit 2023",
            AppType.Revit,
            new ApplicationVersion("2023"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2023.0.0"),
            new DefineConstants("RVT2023"));

        public static AppVersion Revit2024 = new(
            "Autodesk Revit 2024",
            AppType.Revit,
            new ApplicationVersion("2024"),
            new TargetFramework("net48"),
            new RuntimePackageReference("Revit_All_Main_Versions_API_x64", "2024.0.0"),
            new DefineConstants("RVT2024"));
    }
}
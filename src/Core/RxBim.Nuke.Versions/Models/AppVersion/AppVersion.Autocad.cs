#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211

namespace RxBim.Nuke.Versions
{
    public partial class AppVersion
    {
        public static AppVersion Autocad2019 = new(
            "Autodesk Autocad 2019",
            AppType.Autocad,
            new RuntimeVersion("23.0"),
            new ApplicationVersion("2019"),
            new TargetFramework("net472"),
            new RuntimePackageReference("AutoCAD.NET", "23.0.0"),
            new DefineConstants("ACAD2019"));

        public static AppVersion Autocad2020 = new(
            "Autodesk Autocad 2020",
            AppType.Autocad,
            new RuntimeVersion("23.1"),
            new ApplicationVersion("2020"),
            new TargetFramework("net472"),
            new RuntimePackageReference("AutoCAD.NET", "23.1.0"),
            new DefineConstants("ACAD2020"));

        public static AppVersion Autocad2021 = new(
            "Autodesk Autocad 2021",
            AppType.Autocad,
            new RuntimeVersion("24.0"),
            new ApplicationVersion("2021"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD.NET", "24.0.0"),
            new DefineConstants("ACAD2021"));

        public static AppVersion Autocad2022 = new(
            "Autodesk Autocad 2022",
            AppType.Autocad,
            new RuntimeVersion("24.1"),
            new ApplicationVersion("2022"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD.NET", "24.1.51000"),
            new DefineConstants("ACAD2022"));

        public static AppVersion Autocad2023 = new(
            "Autodesk Autocad 2023",
            AppType.Autocad,
            new RuntimeVersion("24.2"),
            new ApplicationVersion("2023"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD.NET", "24.2.0"),
            new DefineConstants("ACAD2023"));

        public static AppVersion Autocad2024 = new(
            "Autodesk Autocad 2024",
            AppType.Autocad,
            new RuntimeVersion("24.3"),
            new ApplicationVersion("2024"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD.NET", "24.3.0"),
            new DefineConstants("ACAD2024"));
    }
}
#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211
namespace RxBim.Nuke.Versions;

public partial class AppVersion
{
    public static AppVersion Civil2019 = new(
        "Autodesk Civil 2019",
        AppType.Civil,
        new RuntimeVersion("23.0"),
        new ApplicationVersion("2019"),
        new TargetFramework("net472"),
        new RuntimePackageReference("AutoCAD.NET", "23.0.0"),
        new RuntimePackageReference("Civil3D2019.Base", "1.0.0"),
        new DefineConstants("ACAD2019;CIVIL2019"));

    public static AppVersion Civil2020 = new(
        "Autodesk Civil 2020",
        AppType.Civil,
        new RuntimeVersion("23.1"),
        new ApplicationVersion("2020"),
        new TargetFramework("net472"),
        new RuntimePackageReference("AutoCAD.NET", "23.1.0"),
        new RuntimePackageReference("Civil3D2020.Base", "1.0.0"),
        new DefineConstants("ACAD2020;CIVIL2020"));

    public static AppVersion Civil2021 = new(
        "Autodesk Civil 2021",
        AppType.Civil,
        new RuntimeVersion("24.0"),
        new ApplicationVersion("2021"),
        new TargetFramework("net48"),
        new RuntimePackageReference("AutoCAD.NET", "24.0.0"),
        new RuntimePackageReference("Civil3D2021.Base", "1.0.0"),
        new DefineConstants("ACAD2021;CIVIL2021"));

    public static AppVersion Civil2022 = new(
        "Autodesk Civil 2022",
        AppType.Civil,
        new RuntimeVersion("24.1"),
        new ApplicationVersion("2022"),
        new TargetFramework("net48"),
        new RuntimePackageReference("AutoCAD.NET", "24.1.51000"),
        new RuntimePackageReference("Civil3D2022.Base", "1.0.0"),
        new DefineConstants("ACAD2022;CIVIL2022"));

    public static AppVersion Civil2023 = new(
        "Autodesk Civil 2023",
        AppType.Civil,
        new RuntimeVersion("24.2"),
        new ApplicationVersion("2023"),
        new TargetFramework("net48"),
        new RuntimePackageReference("AutoCAD.NET", "24.2.0"),
        new RuntimePackageReference("Civil3D2023.Base", "1.0.0"),
        new DefineConstants("ACAD2023;CIVIL2023"));

    public static AppVersion Civil2024 = new(
        "Autodesk Civil 2024",
        AppType.Civil,
        new RuntimeVersion("24.3"),
        new ApplicationVersion("2024"),
        new TargetFramework("net48"),
        new RuntimePackageReference("AutoCAD.NET", "24.3.0"),
        new RuntimePackageReference("Civil3D2024.Base", "1.0.0"),
        new DefineConstants("ACAD2024;CIVIL2024"));

    public static AppVersion Civil2025 = new(
        "Autodesk Civil 2025",
        AppType.Civil,
        new RuntimeVersion("25.0"),
        new ApplicationVersion("2025"),
        new TargetFramework("net8.0-windows"),
        new RuntimePackageReference("AutoCAD.NET", "25.0.1"),
        new RuntimePackageReference("Civil3D.NET", "13.7.765"),
        new DefineConstants("ACAD2025;CIVIL2025"));
}
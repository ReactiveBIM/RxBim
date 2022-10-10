#pragma warning disable CS1591
#pragma warning disable SA1401
#pragma warning disable SA1600

namespace RxBim.Nuke.Versions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

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

        public static AppVersion Autocad2019 = new(
            "Autodesk Autocad 2019",
            AppType.Autocad,
            new ApplicationVersion("2019"),
            new TargetFramework("net472"),
            new RuntimePackageReference("AutoCAD2019.Base", "1.0.3"),
            new DefineConstants("ACAD2019"));

        public static AppVersion Autocad2020 = new(
            "Autodesk Autocad 2020",
            AppType.Autocad,
            new ApplicationVersion("2020"),
            new TargetFramework("net472"),
            new RuntimePackageReference("AutoCAD2020.Base", "1.0.0"),
            new DefineConstants("ACAD2020"));

        public static AppVersion Autocad2021 = new(
            "Autodesk Autocad 2021",
            AppType.Autocad,
            new ApplicationVersion("2021"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD2021.Base", "1.0.0"),
            new DefineConstants("ACAD2021"));

        public static AppVersion Autocad2022 = new(
            "Autodesk Autocad 2022",
            AppType.Autocad,
            new ApplicationVersion("2022"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD2021.Base", "1.0.0"),
            new DefineConstants("ACAD2022"));

        public static AppVersion Autocad2023 = new(
            "Autodesk Autocad 2023",
            AppType.Autocad,
            new ApplicationVersion("2023"),
            new TargetFramework("net48"),
            new RuntimePackageReference("AutoCAD2021.Base", "1.0.0"),
            new DefineConstants("ACAD2023"));

        /// <summary>
        /// Returns all members of the enumeration.
        /// </summary>
        public static IEnumerable<AppVersion> GetAll() =>
            typeof(AppVersion).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<AppVersion>();
    }
}
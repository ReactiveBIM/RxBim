namespace PikTools.MsiBuilder.Bin
{
    using System;
    using CommandLine;
    using Figgle;
    using WixSharp;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(
                FiggleFonts.Slant.Render("PikTools Msi Builder"));

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var wixInstaller = new WixInstaller();
                    var project = wixInstaller.CreateProject(o);
                    Compiler.BuildMsi(project);
                });
        }
    }
}
namespace RxBim.MsiBuilder.GlobalTool
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// sddsfdf
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var binPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "RxBim.MsiBuilder.Bin.exe");
            var process = Process.Start(binPath, string.Join(" ", args));
            process.WaitForExit();
        }
    }
}
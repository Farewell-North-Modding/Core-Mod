using System.Reflection;
using MelonLoader;
using BI = FarewellCore.BuildInfo;

[assembly: AssemblyTitle(BI.Title)]
[assembly: AssemblyProduct(BI.Title)]
[assembly: AssemblyDescription(BI.Description)]
[assembly: AssemblyVersion(BI.Version)]
[assembly: AssemblyFileVersion(BI.Version)]

[assembly: MelonInfo(typeof(FarewellCore.FarewellCore), name: BI.Title, version: BI.Version, author: BI.Author)]
[assembly: MelonGame("Kyle Banks", "Farewell North")]

namespace FarewellCore
{
    /// <summary>
    /// Custom attributes for your mod - modify these to change how your mod appears on the title screen
    /// </summary>
    public static class BuildInfo
    {
        public const string Title = "FarewellCore";
        public const string Description = "A core mod for the game Farewell North";
        public const string Version = "0.1.0";
        public const string Author = "Limo";
    }
}
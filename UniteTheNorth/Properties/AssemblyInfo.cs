using System.Reflection;
using MelonLoader;
using BI = UniteTheNorth.BuildInfo;

[assembly: AssemblyTitle(BI.Title)]
[assembly: AssemblyProduct(BI.Title)]
[assembly: AssemblyDescription(BI.Description)]
[assembly: AssemblyVersion(BI.Version)]
[assembly: AssemblyFileVersion(BI.Version)]

[assembly: MelonInfo(typeof(FarewellCore.FarewellCore), name: BI.Title, version: BI.Version, author: BI.Author)]
[assembly: MelonGame("Kyle Banks", "Farewell North")]

namespace UniteTheNorth
{
    /// <summary>
    /// Custom attributes for your mod - modify these to change how your mod appears on the title screen
    /// </summary>
    public static class BuildInfo
    {
        public const string Title = "UniteTheNorth";
        public const string Description = "A mod aiming at adding a multiplayer to Farewell North";
        public const string Version = "0.1.0";
        public const string Author = "Limo";
    }
}
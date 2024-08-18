using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarewellCore
{
    /// <summary>
    /// Base class for Farewell North mods.
    /// </summary>
    public abstract class FarewellMod : MelonMod, IFarewellMod
    { 
        
    }

    public interface IFarewellMod
    {
        public static MelonLogger.Instance Logger { get { return Melon<FarewellCore>.Logger; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IM = UniverseLib.Input.InputManager;

namespace FarewellCore.Input
{
    /// <summary>
    /// Callback-based bindings for downstream mods.
    /// </summary>
    public static class InputHelper
    {
        private static readonly Dictionary<KeyEvent, List<Action>> keybinds = new();
        
        // private static bool rebinding = false;

        // NB: rebinding is broken in UniverseLib, so implementation will have to wait until we leave it behind

        /// <summary>
        /// Adds a callback to a KeyEvent
        /// </summary>
        public static void AddBind(KeyEvent keybind, Action callback)
        { 
            if (!keybinds.TryGetValue(keybind, out var actions))
            {
                actions = new();
                keybinds[keybind] = actions;
            }
            actions.Add(callback);
        }

        /// <summary>
        /// Removes a callback from a KeyEvent
        /// </summary>
        public static void RemoveBind(KeyEvent keybind, Action callback)
        {
            if (keybinds.TryGetValue(keybind, out var actions))
            {
                actions.Remove(callback);
                if (actions.Count == 0) keybinds.Remove(keybind);
            }
        }

        /*

        /// <summary>
        /// Start a rebind event; will suspend all keyevents until a key is pressed, and pass the keycode to the callback.
        /// </summary>
        public static void Rebind(Action<KeyCode?> callback)
        {
            rebinding = true;
            IM.BeginRebind((keycode) => { }, (keycode) => { rebinding = false; callback(keycode); });
        }

        /// <summary>
        /// Cancel a current rebind; will cause the caller to receive null instead of a KeyCode.
        /// </summary>
        public static void CancelRebind()
        {
            IM.EndRebind();
        }

        */

        // Private implementation: for now deferring to UniverseLib as a dependency, though this is not ideal.
        internal static void UpdateCallback()
        {
            /*
            if (rebinding)
            {
                if (IM.GetKeyDown(KeyCode.Escape)) { CancelRebind(); }
                return;
            }
            */
            foreach (var (keyEvent, actions) in keybinds) 
            {
                bool performAction = false;
                switch (keyEvent.type) 
                {
                    case KeyEventType.Down:
                        performAction = keyEvent.MouseEvent 
                                ? IM.GetMouseButtonDown((int)keyEvent.mouseButton!) 
                                : IM.GetKeyDown((KeyCode)keyEvent.keyCode!);
                        break;
                    case KeyEventType.Up:
                        performAction = keyEvent.MouseEvent
                                ? IM.GetMouseButtonUp((int)keyEvent.mouseButton!)
                                : IM.GetKeyUp((KeyCode)keyEvent.keyCode!);
                        break;
                    case KeyEventType.Held:
                        performAction = keyEvent.MouseEvent
                                ? IM.GetMouseButton((int)keyEvent.mouseButton!)
                                : IM.GetKey((KeyCode)keyEvent.keyCode!);
                        break;
                }
                if (performAction) actions.ForEach(action => action());
            }
        }
    }
}

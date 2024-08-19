using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FarewellCore.Input
{
    public enum KeyEventType
    {
        Down = 0, Up = 1, Held = 2
    }

    /// <summary>
    /// Generic Key or Mouse event
    /// </summary>
    public struct KeyEvent
    {
        public KeyEventType type;
        public KeyCode? keyCode = null;
        public int? mouseButton = null;

        public readonly bool MouseEvent => mouseButton != null;

        public KeyEvent(KeyEventType type, KeyCode keyCode)
        {
            this.type = type;
            this.keyCode = keyCode;
        }

        public KeyEvent(KeyEventType type, int mouseButton)
        {
            this.type = type;
            this.mouseButton = mouseButton;
        }
    }
}

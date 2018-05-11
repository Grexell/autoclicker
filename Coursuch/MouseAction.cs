using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Coursuch
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MouseAction:AbstractAction
    {
        public MouseAction() {
            MouseFlag = MouseFlags.Move;
        }

        [JsonProperty]
        public int X { get; set; }
        [JsonProperty]
        public int Y { get; set; }

        [JsonProperty]
        public MouseFlags MouseFlag { get; set; }

        [DllImport("User32.dll")]
        private static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
        
        public override void Execute()
        {
            mouse_event(MouseFlag, X, Y, 0, new UIntPtr());
        }
    }

    [Flags]
    public enum MouseFlags
    {
        [Description("Передвижение")]
        Move = 0x0001,
        [Description("Нажать левую клавишу")]
        LeftDown = 0x0002,
        [Description("Отпустить левую клавишу")]
        LeftUp = 0x0004,
        [Description("Нажать правую клавишу")]
        RightDown = 0x0008,
        [Description("Отпустить правую клавишу")]
        RightUp = 0x0010,
        [Description("Абсолютное")]
        Absolute = 0x8000
    }
}
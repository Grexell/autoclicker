using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Coursuch
{
    [JsonObject(MemberSerialization.OptIn)]
    public class KeyBoardAction:AbstractAction
    {

        public KeyBoardAction() {
            Flag = KeyFlag.KeyDown;
        }

        [JsonProperty]
        public KeyFlag Flag { get; set; }

        [JsonProperty]
        public Key Key { get; set; }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        
        public override void Execute()
        {
            keybd_event((byte)Key, (byte)Flag, 0, 0);
        }
    }

    public enum KeyFlag
    {
        [Description("Нажать клавишу")]
        KeyDown = 0x0,
        [Description("Отпустить клавишу")]
        KeyUp = 0x2
    }

}
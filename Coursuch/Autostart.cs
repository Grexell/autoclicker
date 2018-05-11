using Microsoft.Win32;

namespace Coursuch
{
    class Autostart
    {
        
	    private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string VALUE_NAME = "Automation";

        public static void EnableAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.SetValue(VALUE_NAME, System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static bool IsAutoStartEnabled
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
                if (key == null)
                    return false;

                string value = (string)key.GetValue(VALUE_NAME);
                if (value == null)
                    return false;
                return (value == System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }

        public static void DisableSetAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.DeleteValue(VALUE_NAME);
        }
    }
}

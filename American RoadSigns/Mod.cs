using ColossalFramework.Plugins;
using ICities;
using System;

namespace AmericanRoadSigns
{
    public class Mod : IUserMod
    {
        public const ulong workshop_id = 690066392;
        public const string version = "2.1.1";

        public string Name
        {
            get { return "American Roadsigns " + version; }
        }

        public string Description
        {
            get { return "Take control over highway gantries, road signs and road-side props."; }
        }

        //  Select Options:
        private void OnKeyboardShortcutChanged(int c)
        {
            AmericanRoadsignsTool.config.keyboardshortcut = c;
            AmericanRoadsignsTool.SaveConfig();
        }

        private void OnEnableDebugChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_debug = c;
            AmericanRoadsignsTool.SaveConfig();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                AmericanRoadsignsTool.LoadConfig();

                //  Mod options:
                UIHelperBase group = helper.AddGroup(Name);
                group.AddSpace(10);
                //  Select Options (Road signs):
                group.AddDropdown("Select your preferred keyboard shortcut for toggling the mod panel", new[] { "Shift + A", "Ctrl + A", "Alt + A" }, AmericanRoadsignsTool.config.keyboardshortcut, OnKeyboardShortcutChanged);
                group.AddSpace(10);
                //  Toggle Options (Misc.):
                group.AddCheckbox("Write additional data to debug log", AmericanRoadsignsTool.config.enable_debug, new OnCheckChanged(OnEnableDebugChanged));
                group.AddSpace(5);
                group.AddGroup("WARNING: enabling debug data may increase loading times!\nEnabling this setting is only recommended when you experience problems.");
                group.AddSpace(20);
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
            }
        }
    }
}
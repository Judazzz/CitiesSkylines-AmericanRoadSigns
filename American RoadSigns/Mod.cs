using ColossalFramework.Plugins;
using ICities;
using System;

namespace AmericanRoadSigns
{
    public class Mod : IUserMod
    {
        public const ulong workshop_id = 690066392;
        public const string version = "2.0.0";

        public string Name
        {
            get { return "American Roadsigns " + version; }
        }

        public string Description
        {
            get { return "Americanizes Road and Highway Signs."; }
        }

        //  Select Options:
        private void OnKeyboardShortcutChanged(int c)
        {
            AmericanRoadsignsTool.config.keyboardshortcut = c;
            AmericanRoadsignsTool.SaveConfig();
        }

        private void OnHighwayGantryChanged(int c)
        {
            AmericanRoadsignsTool.config.rendermode_highwaygantry = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnHighwaySignChanged(int c)
        {
            AmericanRoadsignsTool.config.rendermode_highwaysign = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnNoParkingChanged(int c)
        {
            AmericanRoadsignsTool.config.rendermode_noparking = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnNoTurningChanged(int c)
        {
            AmericanRoadsignsTool.config.rendermode_noturnings = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnSpeedLimitChanged(int c)
        {
            AmericanRoadsignsTool.config.rendermode_speedlimits = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnStreetNameChanged(int c)
        {
            AmericanRoadsignsTool.config.rendermode_streetname = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        //  Toggle Options:
        private void OnAltHighwayGantryChanged(bool c)
        {
            AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleManholesHighwayChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_manholes_highway = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleManholesElevatedChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_manholes_elevated = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleStreetPropElectricityboxChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_streetprops_electricitybox = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleStreetPropFirehydrantChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_streetprops_firehydrant = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleStreetPropInfoterminalChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_streetprops_infoterminal = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleStreetPropParkingmeterChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_streetprops_parkingmeter = c;
            AmericanRoadsignsTool.SaveConfig();
        }
        private void OnToggleStreetPropRandomChanged(bool c)
        {
            AmericanRoadsignsTool.config.enable_streetprops_random = c;
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
                //  Select Options (Road signs):
                group.AddDropdown("Highway overhead gantries", new[] { "American", "Vanilla", "Hide" }, AmericanRoadsignsTool.config.rendermode_highwaygantry, OnHighwayGantryChanged);
                group.AddCheckbox("Use alternative American highway overhead gantry texture (WIP).", AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt, new OnCheckChanged(OnAltHighwayGantryChanged));
                group.AddSpace(10);
                group.AddDropdown("Highway route signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadsignsTool.config.rendermode_highwaysign, OnHighwaySignChanged);
                group.AddDropdown("'No parking' signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadsignsTool.config.rendermode_noparking, OnNoParkingChanged);
                group.AddDropdown("No left/right turn' signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadsignsTool.config.rendermode_noturnings, OnNoTurningChanged);
                group.AddDropdown("Speed limit signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadsignsTool.config.rendermode_speedlimits, OnSpeedLimitChanged);
                group.AddDropdown("Streetname signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadsignsTool.config.rendermode_streetname, OnStreetNameChanged);
                group.AddSpace(10);
                //  Toggle Options (Street props):
                group.AddCheckbox("Show manhole covers on highways", AmericanRoadsignsTool.config.enable_manholes_highway, new OnCheckChanged(OnToggleManholesHighwayChanged));
                group.AddCheckbox("Show manhole covers on elevated roads and bridges", AmericanRoadsignsTool.config.enable_manholes_elevated, new OnCheckChanged(OnToggleManholesElevatedChanged));
                group.AddSpace(10);
                group.AddCheckbox("Show road-side electricity box props", AmericanRoadsignsTool.config.enable_streetprops_electricitybox, new OnCheckChanged(OnToggleStreetPropElectricityboxChanged));
                group.AddCheckbox("Show road-side fire hydrant props", AmericanRoadsignsTool.config.enable_streetprops_firehydrant, new OnCheckChanged(OnToggleStreetPropFirehydrantChanged));
                group.AddCheckbox("Show road-side info terminal props", AmericanRoadsignsTool.config.enable_streetprops_infoterminal, new OnCheckChanged(OnToggleStreetPropInfoterminalChanged));
                group.AddCheckbox("Show road-side parking meter props", AmericanRoadsignsTool.config.enable_streetprops_parkingmeter, new OnCheckChanged(OnToggleStreetPropParkingmeterChanged));
                group.AddCheckbox("Show road-side random street props", AmericanRoadsignsTool.config.enable_streetprops_random, new OnCheckChanged(OnToggleStreetPropRandomChanged));
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
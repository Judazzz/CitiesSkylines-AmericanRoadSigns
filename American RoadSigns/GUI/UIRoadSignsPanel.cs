using ColossalFramework.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    public class UIRoadSignsPanel : UIPanel
    {
        private bool isInitialized = false;

        private UICheckBox _gantryCheckbox;
        public UICheckBox gantryCheckbox
        {
            get { return _gantryCheckbox; }
        }

        private UICheckBox _textureCheckbox;
        public UICheckBox textureCheckbox
        {
            get { return _textureCheckbox; }
        }

        private UICheckBox _highwayCheckbox;
        public UICheckBox highwayCheckbox
        {
            get { return _highwayCheckbox; }
        }

        private UICheckBox _noparkingCheckbox;
        public UICheckBox noparkingCheckbox
        {
            get { return _noparkingCheckbox; }
        }

        private UICheckBox _turningCheckbox;
        public UICheckBox turningCheckbox
        {
            get { return _turningCheckbox; }
        }

        private UICheckBox _speedlimitCheckbox;
        public UICheckBox speedlimitCheckbox
        {
            get { return _speedlimitCheckbox; }
        }

        private UICheckBox _streetnameCheckbox;
        public UICheckBox streetnameCheckbox
        {
            get { return _streetnameCheckbox; }
        }

        private static UIRoadSignsPanel _instance;
        public static UIRoadSignsPanel instance
        {
            get { return _instance; }
        }

        public override void Start()
        {
            base.Start();
            _instance = this;
            canFocus = true;
            isInteractive = true;
            //  
            SetupControls();
            isInitialized = true;
        }

        private void SetupControls()
        {
            //  Highway gantry signs:
            var gantryContainer = UIUtils.CreateFormElement(this, "top");
            gantryContainer.name = "gantryContainer";
            _gantryCheckbox = UIUtils.CreateCheckBox(gantryContainer);
            _gantryCheckbox.relativePosition = new Vector3(5, 17);
            _gantryCheckbox.label.text = "Highway gantry signs";
            _gantryCheckbox.isChecked = AmericanRoadsignsTool.config.enable_gantrysigns;
            _gantryCheckbox.eventCheckChanged += CheckboxChanged;
            
            //  Alternative highway gantry sign texture:
            var textureContainer = UIUtils.CreateFormElement(this, "center");
            textureContainer.name = "textureContainer";
            textureContainer.relativePosition = new Vector3(0, 70);
            _textureCheckbox = UIUtils.CreateCheckBox(textureContainer);
            _textureCheckbox.relativePosition = new Vector3(5, 17);
            _textureCheckbox.label.text = "Use alternative gantry texture";
            _textureCheckbox.isChecked = AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt;
            _textureCheckbox.eventCheckChanged += TextureCheckboxChanged;

            //  Highway signs:
            var highwayContainer = UIUtils.CreateFormElement(this, "center");
            highwayContainer.name = "highwaysignContainer";
            highwayContainer.relativePosition = new Vector3(0, 120);
            _highwayCheckbox = UIUtils.CreateCheckBox(highwayContainer);
            _highwayCheckbox.relativePosition = new Vector3(5, 17);
            _highwayCheckbox.label.text = "Highway signs";
            _highwayCheckbox.isChecked = AmericanRoadsignsTool.config.enable_highwaysigns;
            _highwayCheckbox.eventCheckChanged += CheckboxChanged;

            //  No parking signs:
            var noparkingContainer = UIUtils.CreateFormElement(this, "center");
            noparkingContainer.name = "noparkingContainer";
            noparkingContainer.relativePosition = new Vector3(0, 170);
            _noparkingCheckbox = UIUtils.CreateCheckBox(noparkingContainer);
            _noparkingCheckbox.relativePosition = new Vector3(5, 17);
            _noparkingCheckbox.label.text = "No parking signs";
            _noparkingCheckbox.isChecked = AmericanRoadsignsTool.config.enable_noparkingsigns;
            _noparkingCheckbox.eventCheckChanged += CheckboxChanged;

            //  Turning signs:
            var turningContainer = UIUtils.CreateFormElement(this, "center");
            turningContainer.name = "turningContainer";
            turningContainer.relativePosition = new Vector3(0, 220);
            _turningCheckbox = UIUtils.CreateCheckBox(turningContainer);
            _turningCheckbox.relativePosition = new Vector3(5, 17);
            _turningCheckbox.label.text = "No turning signs";
            _turningCheckbox.isChecked = AmericanRoadsignsTool.config.enable_turningsigns;
            _turningCheckbox.eventCheckChanged += CheckboxChanged;

            //  Speed limit signs:
            var speedlimitContainer = UIUtils.CreateFormElement(this, "center");
            speedlimitContainer.name = "speedlimitContainer";
            speedlimitContainer.relativePosition = new Vector3(0, 270);
            _speedlimitCheckbox = UIUtils.CreateCheckBox(speedlimitContainer);
            _speedlimitCheckbox.relativePosition = new Vector3(5, 17);
            _speedlimitCheckbox.label.text = "Speed limit signs";
            _speedlimitCheckbox.isChecked = AmericanRoadsignsTool.config.enable_speedlimitsigns;
            _speedlimitCheckbox.eventCheckChanged += CheckboxChanged;

            //  Street name signs:
            var streetnameContainer = UIUtils.CreateFormElement(this, "center");
            streetnameContainer.name = "streetnameContainer";
            streetnameContainer.relativePosition = new Vector3(0, 320);
            _streetnameCheckbox = UIUtils.CreateCheckBox(streetnameContainer);
            _streetnameCheckbox.relativePosition = new Vector3(5, 17);
            _streetnameCheckbox.label.text = "Street name signs";
            _streetnameCheckbox.isChecked = AmericanRoadsignsTool.config.enable_streetnamesigns;
            _streetnameCheckbox.eventCheckChanged += CheckboxChanged;
        }

        private void CheckboxChanged(UIComponent trigger, bool isChecked)
        {
            if (!isInitialized)
            {
                return;
            }
            DebugUtils.Log($"CheckboxChanged: name = {trigger.name}, visible = {isChecked}.");
            //  
            if (trigger == _gantryCheckbox)
            {
                AmericanRoadsignsTool.ChangeRoadsignPropVisibility("gantry", isChecked, true);
                AmericanRoadsignsTool.config.enable_gantrysigns = isChecked;
            }
            if (trigger == _highwayCheckbox)
            {
                AmericanRoadsignsTool.ChangeRoadsignPropVisibility("highway", isChecked, true);
                AmericanRoadsignsTool.config.enable_highwaysigns = isChecked;
            }
            if (trigger == _noparkingCheckbox)
            {
                AmericanRoadsignsTool.ChangeRoadsignPropVisibility("noparking", isChecked, true);
                AmericanRoadsignsTool.config.enable_speedlimitsigns = isChecked;
            }
            if (trigger == _turningCheckbox)
            {
                AmericanRoadsignsTool.ChangeRoadsignPropVisibility("turning", isChecked, true);
                AmericanRoadsignsTool.config.enable_turningsigns = isChecked;
            }
            if (trigger == _speedlimitCheckbox)
            {
                AmericanRoadsignsTool.ChangeRoadsignPropVisibility("speedlimit", isChecked, true);
                AmericanRoadsignsTool.config.enable_speedlimitsigns = isChecked;
            }
            if (trigger == _streetnameCheckbox)
            {
                AmericanRoadsignsTool.ChangeRoadsignPropVisibility("streetname", isChecked, true);
                AmericanRoadsignsTool.config.enable_streetnamesigns = isChecked;
            }
            //  
            AmericanRoadsignsTool.SaveConfig();
        }
        
        private void TextureCheckboxChanged(UIComponent trigger, bool isChecked)
        {
            if (!isInitialized)
            {
                return;
            }
            DebugUtils.Log($"CheckboxChanged: name = {trigger.name}, use alternative texture = {isChecked}.");
            //  
            if (trigger == _textureCheckbox)
            {
                CustomizableRoadsignItem affectedSign = AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway overroad signs").FirstOrDefault();
                AmericanRoadsignsTool.RetextureRoadSignProp("Motorway Overroad Signs", isChecked);
                AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt = isChecked;
            }
            AmericanRoadsignsTool.SaveConfig();
            //  
        }
    }
}
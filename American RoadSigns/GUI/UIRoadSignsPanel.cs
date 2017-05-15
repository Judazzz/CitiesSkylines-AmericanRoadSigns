using ColossalFramework.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    public class UIRoadSignsPanel : UIPanel
    {
        private bool isInitialized = false;

        private UIDropDown _gantryDropdown;
        public UIDropDown gantryDropdown
        {
            get { return _gantryDropdown; }
        }

        private UICheckBox _textureCheckbox;
        public UICheckBox textureCheckbox
        {
            get { return _textureCheckbox; }
        }

        private UIDropDown _highwayDropdown;
        public UIDropDown highwayDropdown
        {
            get { return _highwayDropdown; }
        }

        private UIDropDown _noparkingDropdown;
        public UIDropDown noparkingDropdown
        {
            get { return _noparkingDropdown; }
        }

        private UIDropDown _turningDropdown;
        public UIDropDown turningDropdown
        {
            get { return _turningDropdown; }
        }

        private UIDropDown _speedlimitDropdown;
        public UIDropDown speedlimitDropdown
        {
            get { return _speedlimitDropdown; }
        }

        private UIDropDown _streetnameDropdown;
        public UIDropDown streetnameDropdown
        {
            get { return _streetnameDropdown; }
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
            //  Highway gantries:
            var gantryContainer = UIUtils.CreateFormElement(this, "top");
            gantryContainer.name = "gantryContainer";
            gantryContainer.relativePosition = new Vector3(0, 23);
            gantryContainer.autoLayout = false;
            var gantryLabel = gantryContainer.AddUIComponent<UILabel>();
            gantryLabel.width = 140f;
            gantryLabel.relativePosition = new Vector3(5, 0);
            gantryLabel.text = "Highway gantries";
            gantryLabel.textColor = new Color(187, 187, 187, 255);
            gantryLabel.textScale = 0.8f;
            gantryLabel.padding = new RectOffset(0, 0, 0, 5);
            _gantryDropdown = UIUtils.CreateStyledDropDown(gantryContainer);
            _gantryDropdown.name = "gantryDropdown";
            _gantryDropdown.width = 105f;
            _gantryDropdown.height = 24f;
            _gantryDropdown.relativePosition = new Vector3(145, -6);
            _gantryDropdown.AddItem("American");
            _gantryDropdown.AddItem("Vanilla");
            _gantryDropdown.AddItem("Hide");
            _gantryDropdown.selectedIndex = AmericanRoadsignsTool.config.rendermode_highwaygantry;

            _gantryDropdown.eventSelectedIndexChanged += (c, i) =>
            {
                if (!isInitialized)
                {
                    return;
                }
                DropdownChanged(c, i);
            };

            //  Alternative highway gantry sign texture:
            var textureContainer = UIUtils.CreateFormElement(this, "center");
            textureContainer.name = "textureContainer";
            textureContainer.relativePosition = new Vector3(0, 55);
            _textureCheckbox = UIUtils.CreateCheckBox(textureContainer);
            _textureCheckbox.relativePosition = new Vector3(5, 17);
            _textureCheckbox.label.text = "Use alternative gantry texture";
            _textureCheckbox.isChecked = AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt;
            _textureCheckbox.eventCheckChanged += CheckboxChanged;

            //  Highway signs:
            var highwayContainer = UIUtils.CreateFormElement(this, "center");
            highwayContainer.name = "highwayContainer";
            highwayContainer.relativePosition = new Vector3(0, 90);
            highwayContainer.autoLayout = false;
            var highwayLabel = highwayContainer.AddUIComponent<UILabel>();
            highwayLabel.width = 140f;
            highwayLabel.relativePosition = new Vector3(5, 0);
            highwayLabel.text = "Highway signs";
            highwayLabel.textColor = new Color(187, 187, 187, 255);
            highwayLabel.textScale = 0.8f;
            highwayLabel.padding = new RectOffset(0, 0, 0, 5);
            _highwayDropdown = UIUtils.CreateStyledDropDown(highwayContainer);
            _highwayDropdown.name = "highwayDropdown";
            _highwayDropdown.width = 105f;
            _highwayDropdown.height = 24f;
            _highwayDropdown.relativePosition = new Vector3(145, -6);
            _highwayDropdown.AddItem("American");
            _highwayDropdown.AddItem("Vanilla");
            _highwayDropdown.AddItem("Hide");
            _highwayDropdown.selectedIndex = AmericanRoadsignsTool.config.rendermode_highwaysign;
            _highwayDropdown.eventSelectedIndexChanged += (c, i) =>
            {
                if (!isInitialized)
                {
                    return;
                }
                DropdownChanged(c, i);
            };

            //  No parking signs:
            var noparkingContainer = UIUtils.CreateFormElement(this, "center");
            noparkingContainer.name = "noparkingContainer";
            noparkingContainer.relativePosition = new Vector3(0, 125);
            noparkingContainer.autoLayout = false;
            var noparkingLabel = noparkingContainer.AddUIComponent<UILabel>();
            noparkingLabel.width = 140f;
            noparkingLabel.relativePosition = new Vector3(5, 0);
            noparkingLabel.text = "No parking signs";
            noparkingLabel.textColor = new Color(187, 187, 187, 255);
            noparkingLabel.textScale = 0.8f;
            noparkingLabel.padding = new RectOffset(0, 0, 0, 5);
            _noparkingDropdown = UIUtils.CreateStyledDropDown(noparkingContainer);
            _noparkingDropdown.name = "noparkingDropdown";
            _noparkingDropdown.width = 105f;
            _noparkingDropdown.height = 24f;
            _noparkingDropdown.relativePosition = new Vector3(145, -6);
            _noparkingDropdown.AddItem("American");
            _noparkingDropdown.AddItem("Vanilla");
            _noparkingDropdown.AddItem("Hide");
            _noparkingDropdown.selectedIndex = AmericanRoadsignsTool.config.rendermode_noparking;
            _noparkingDropdown.eventSelectedIndexChanged += (c, i) =>
            {
                if (!isInitialized)
                {
                    return;
                }
                DropdownChanged(c, i);
            };

            //  No turning signs:
            var turningContainer = UIUtils.CreateFormElement(this, "center");
            turningContainer.name = "turningContainer";
            turningContainer.relativePosition = new Vector3(0, 160);
            turningContainer.autoLayout = false;
            var turningLabel = turningContainer.AddUIComponent<UILabel>();
            turningLabel.width = 140f;
            turningLabel.relativePosition = new Vector3(5, 0);
            turningLabel.text = "No turning signs";
            turningLabel.textColor = new Color(187, 187, 187, 255);
            turningLabel.textScale = 0.8f;
            turningLabel.padding = new RectOffset(0, 0, 0, 5);
            _turningDropdown = UIUtils.CreateStyledDropDown(turningContainer);
            _turningDropdown.name = "turningDropdown";
            _turningDropdown.width = 105f;
            _turningDropdown.height = 24f;
            _turningDropdown.relativePosition = new Vector3(145, -6);
            _turningDropdown.AddItem("American");
            _turningDropdown.AddItem("Vanilla");
            _turningDropdown.AddItem("Hide");
            _turningDropdown.selectedIndex = AmericanRoadsignsTool.config.rendermode_noturnings;
            _turningDropdown.eventSelectedIndexChanged += (c, i) =>
            {
                if (!isInitialized)
                {
                    return;
                }
                DropdownChanged(c, i);
            };

            //  Speed limit signs:
            var speedlimitContainer = UIUtils.CreateFormElement(this, "center");
            speedlimitContainer.name = "speedlimitContainer";
            speedlimitContainer.relativePosition = new Vector3(0, 195);
            speedlimitContainer.autoLayout = false;
            var speedlimitLabel = speedlimitContainer.AddUIComponent<UILabel>();
            speedlimitLabel.width = 140f;
            speedlimitLabel.relativePosition = new Vector3(5, 0);
            speedlimitLabel.text = "Speed limit signs";
            speedlimitLabel.textColor = new Color(187, 187, 187, 255);
            speedlimitLabel.textScale = 0.8f;
            speedlimitLabel.padding = new RectOffset(0, 0, 0, 5);
            _speedlimitDropdown = UIUtils.CreateStyledDropDown(speedlimitContainer);
            _speedlimitDropdown.name = "speedlimitDropdown";
            _speedlimitDropdown.width = 105f;
            _speedlimitDropdown.height = 24f;
            _speedlimitDropdown.relativePosition = new Vector3(145, -6);
            _speedlimitDropdown.AddItem("American");
            _speedlimitDropdown.AddItem("Vanilla");
            _speedlimitDropdown.AddItem("Hide");
            _speedlimitDropdown.selectedIndex = AmericanRoadsignsTool.config.rendermode_speedlimits;
            _speedlimitDropdown.eventSelectedIndexChanged += (c, i) =>
            {
                if (!isInitialized)
                {
                    return;
                }
                DropdownChanged(c, i);
            };

            //  Street name signs:
            var streetnameContainer = UIUtils.CreateFormElement(this, "center");
            streetnameContainer.name = "streetnameContainer";
            streetnameContainer.relativePosition = new Vector3(0, 230);
            streetnameContainer.autoLayout = false;
            var streetnameLabel = streetnameContainer.AddUIComponent<UILabel>();
            streetnameLabel.width = 140f;
            streetnameLabel.relativePosition = new Vector3(5, 0);
            streetnameLabel.text = "Street name signs";
            streetnameLabel.textColor = new Color(187, 187, 187, 255);
            streetnameLabel.textScale = 0.8f;
            streetnameLabel.padding = new RectOffset(0, 0, 0, 5);
            _streetnameDropdown = UIUtils.CreateStyledDropDown(streetnameContainer);
            _streetnameDropdown.name = "_streetnameDropdown";
            _streetnameDropdown.width = 105f;
            _streetnameDropdown.height = 24f;
            _streetnameDropdown.relativePosition = new Vector3(145, -6);
            _streetnameDropdown.AddItem("American");
            _streetnameDropdown.AddItem("Vanilla");
            _streetnameDropdown.AddItem("Hide");
            _streetnameDropdown.selectedIndex = AmericanRoadsignsTool.config.rendermode_streetname;
            _streetnameDropdown.eventSelectedIndexChanged += (c, i) =>
            {
                if (!isInitialized)
                {
                    return;
                }
                DropdownChanged(c, i);
            };
        }

        private void CheckboxChanged(UIComponent trigger, bool isChecked)
        {
            if (!isInitialized)
            {
                return;
            }
            //  
            if (trigger == _textureCheckbox)
            {
                //  Only for Americanized texture:
                //if (AmericanRoadsignsTool.config.rendermode_highwaygantry != 0)
                //{
                //    return;
                //}
                AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt = isChecked;
                CustomizableRoadsignItem affectedSign = AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway overroad signs").FirstOrDefault();
                AmericanRoadsignsTool.ChangeRoadsignPropWithoutCustomAsset(affectedSign, 0);
                //  
                AmericanRoadsignsTool.SaveConfig();
                //  
                _gantryDropdown.selectedIndex = 0;
            }
        }

        private void DropdownChanged(UIComponent trigger, int selectedValue)
        {
            if (!isInitialized)
            {
                return;
            }
            DebugUtils.Log($"DropdownChanged: name = {trigger.name}, selected value is {selectedValue}.");
            //  
            CustomizableRoadsignItem affectedSign = new CustomizableRoadsignItem();
            List<CustomizableRoadsignItem> affectedSigns = new List<CustomizableRoadsignItem>();
            if (trigger == _gantryDropdown)
            {
                affectedSign = AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway overroad signs").FirstOrDefault();
                AmericanRoadsignsTool.ChangeRoadsignPropWithoutCustomAsset(affectedSign, selectedValue);
                AmericanRoadsignsTool.config.rendermode_highwaygantry = selectedValue;
            }
            else if (trigger == _highwayDropdown)
            {
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway sign").FirstOrDefault());
                AmericanRoadsignsTool.ChangeRoadsignPropWithCustomAsset(affectedSigns, selectedValue);
                AmericanRoadsignsTool.config.rendermode_highwaysign = selectedValue;
            }
            else if (trigger == _noparkingDropdown)
            {
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no parking sign").FirstOrDefault());
                AmericanRoadsignsTool.ChangeRoadsignPropWithCustomAsset(affectedSigns, selectedValue);
                AmericanRoadsignsTool.config.rendermode_noparking = selectedValue;
            }
            else if (trigger == _turningDropdown)
            {
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no left turn sign").FirstOrDefault());
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no right turn sign").FirstOrDefault());
                AmericanRoadsignsTool.ChangeRoadsignPropWithCustomAsset(affectedSigns, selectedValue);
                AmericanRoadsignsTool.config.rendermode_noturnings = selectedValue;
            }
            else if (trigger == _speedlimitDropdown)
            {
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "30 speed limit").FirstOrDefault());
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "40 speed limit").FirstOrDefault());
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "50 speed limit").FirstOrDefault());
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "60 speed limit").FirstOrDefault());
                affectedSigns.Add(AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "100 speed limit").FirstOrDefault());
                AmericanRoadsignsTool.ChangeRoadsignPropWithCustomAsset(affectedSigns, selectedValue);
                AmericanRoadsignsTool.config.rendermode_speedlimits = selectedValue;
            }
            else if (trigger == _streetnameDropdown)
            {
                affectedSign = AmericanRoadsignsTool.CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "street name sign").FirstOrDefault();
                AmericanRoadsignsTool.ChangeRoadsignPropWithoutCustomAsset(affectedSign, selectedValue);
                AmericanRoadsignsTool.config.rendermode_streetname = selectedValue;
            }
            //  
            AmericanRoadsignsTool.SaveConfig();
        }
    }
}
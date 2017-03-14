using System.Linq;
using ColossalFramework.UI;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    public class UIRoadPropsPanel : UIPanel
    {
        private bool isInitialized = false;

        private UICheckBox _manholeHighwayCheckbox;
        public UICheckBox manholeHighwayCheckbox
        {
            get { return _manholeHighwayCheckbox; }
        }

        private UICheckBox _manholeElevatedCheckbox;
        public UICheckBox manholeElevatedCheckbox
        {
            get { return _manholeElevatedCheckbox; }
        }

        private UICheckBox _electricityBoxCheckbox;
        public UICheckBox electricityBoxCheckbox
        {
            get { return _electricityBoxCheckbox; }
        }

        private UICheckBox _fireHydrantCheckbox;
        public UICheckBox fireHydrantCheckbox
        {
            get { return _fireHydrantCheckbox; }
        }

        private UICheckBox _infoTerminalCheckbox;
        public UICheckBox infoTerminalCheckbox
        {
            get { return _infoTerminalCheckbox; }
        }

        private UICheckBox _parkingMeterCheckbox;
        public UICheckBox parkingMeterCheckbox
        {
            get { return _parkingMeterCheckbox; }
        }

        private UICheckBox _randomPropCheckbox;
        public UICheckBox randomPropCheckbox
        {
            get { return _randomPropCheckbox; }
        }

        private static UIRoadPropsPanel _instance;
        public static UIRoadPropsPanel instance
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
            //  Manhole covers on highways:
            var manholeHighwayContainer = UIUtils.CreateFormElement(this, "top");
            manholeHighwayContainer.name = "manholeHighwayContainer";
            _manholeHighwayCheckbox = UIUtils.CreateCheckBox(manholeHighwayContainer);
            _manholeHighwayCheckbox.relativePosition = new Vector3(5, 17);
            _manholeHighwayCheckbox.label.text = "Manhole covers on highways";
            _manholeHighwayCheckbox.isChecked = AmericanRoadsignsTool.config.enable_manholes_highway;
            _manholeHighwayCheckbox.eventCheckChanged += CheckboxChanged;

            //  Manhole covers on elevated roads:
            var manholeElevatedContainer = UIUtils.CreateFormElement(this, "center");
            manholeElevatedContainer.name = "manholeElevatedContainer";
            manholeElevatedContainer.relativePosition = new Vector3(0, 55);
            _manholeElevatedCheckbox = UIUtils.CreateCheckBox(manholeElevatedContainer);
            _manholeElevatedCheckbox.relativePosition = new Vector3(5, 17);
            _manholeElevatedCheckbox.label.text = "Manhole covers on bridges";
            _manholeElevatedCheckbox.isChecked = AmericanRoadsignsTool.config.enable_manholes_elevated;
            _manholeElevatedCheckbox.eventCheckChanged += CheckboxChanged;

            //  Electricity boxes:
            var electricityBoxContainer = UIUtils.CreateFormElement(this, "center");
            electricityBoxContainer.name = "electricityBoxContainer";
            electricityBoxContainer.relativePosition = new Vector3(0, 90);
            _electricityBoxCheckbox = UIUtils.CreateCheckBox(electricityBoxContainer);
            _electricityBoxCheckbox.relativePosition = new Vector3(5, 17);
            _electricityBoxCheckbox.label.text = "Electricity box props";
            _electricityBoxCheckbox.isChecked = AmericanRoadsignsTool.config.enable_streetprops_electricitybox;
            _electricityBoxCheckbox.eventCheckChanged += CheckboxChanged;

            //  Fire hydrants:
            var fireHydrantContainer = UIUtils.CreateFormElement(this, "center");
            fireHydrantContainer.name = "fireHydrantContainer";
            fireHydrantContainer.relativePosition = new Vector3(0, 125);
            _fireHydrantCheckbox = UIUtils.CreateCheckBox(fireHydrantContainer);
            _fireHydrantCheckbox.relativePosition = new Vector3(5, 17);
            _fireHydrantCheckbox.label.text = "Fire hydrant props";
            _fireHydrantCheckbox.isChecked = AmericanRoadsignsTool.config.enable_streetprops_firehydrant;
            _fireHydrantCheckbox.eventCheckChanged += CheckboxChanged;

            //  Information terminals:
            var infoTerminalContainer = UIUtils.CreateFormElement(this, "center");
            infoTerminalContainer.name = "infoTerminalContainer";
            infoTerminalContainer.relativePosition = new Vector3(0, 160);
            _infoTerminalCheckbox = UIUtils.CreateCheckBox(infoTerminalContainer);
            _infoTerminalCheckbox.relativePosition = new Vector3(5, 17);
            _infoTerminalCheckbox.label.text = "Info terminal props";
            _infoTerminalCheckbox.isChecked = AmericanRoadsignsTool.config.enable_streetprops_infoterminal;
            _infoTerminalCheckbox.eventCheckChanged += CheckboxChanged;

            //  Parking meters:
            var parkingMeterContainer = UIUtils.CreateFormElement(this, "center");
            parkingMeterContainer.name = "parkingMeterContainer";
            parkingMeterContainer.relativePosition = new Vector3(0, 195);
            _parkingMeterCheckbox = UIUtils.CreateCheckBox(parkingMeterContainer);
            _parkingMeterCheckbox.relativePosition = new Vector3(5, 17);
            _parkingMeterCheckbox.label.text = "Parking meter props";
            _parkingMeterCheckbox.isChecked = AmericanRoadsignsTool.config.enable_streetprops_parkingmeter;
            _parkingMeterCheckbox.eventCheckChanged += CheckboxChanged;

            //  Random street props:
            var randomPropContainer = UIUtils.CreateFormElement(this, "center");
            randomPropContainer.name = "randomPropContainer";
            randomPropContainer.relativePosition = new Vector3(0, 230);
            _randomPropCheckbox = UIUtils.CreateCheckBox(randomPropContainer);
            _randomPropCheckbox.relativePosition = new Vector3(5, 17);
            _randomPropCheckbox.label.text = "Random street props";
            _randomPropCheckbox.isChecked = AmericanRoadsignsTool.config.enable_streetprops_random;
            _randomPropCheckbox.eventCheckChanged += CheckboxChanged;
        }

        private void CheckboxChanged(UIComponent trigger, bool isChecked)
        {
            if (!isInitialized)
            {
                return;
            }
            //  
            if (trigger == _manholeHighwayCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "manhole").FirstOrDefault(), isChecked, "highway");
                AmericanRoadsignsTool.config.enable_manholes_highway = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
            else if (trigger == _manholeElevatedCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "manhole").FirstOrDefault(), isChecked, "elevated");
                AmericanRoadsignsTool.config.enable_manholes_elevated = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
            else if (trigger == _electricityBoxCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "electricity box").FirstOrDefault(), isChecked);
                AmericanRoadsignsTool.config.enable_streetprops_electricitybox = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
            else if (trigger == _fireHydrantCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "fire hydrant").FirstOrDefault(), isChecked);
                AmericanRoadsignsTool.config.enable_streetprops_firehydrant = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
            else if (trigger == _infoTerminalCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "info terminal").FirstOrDefault(), isChecked);
                AmericanRoadsignsTool.config.enable_streetprops_infoterminal = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
            else if (trigger == _parkingMeterCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "parking meter").FirstOrDefault(), isChecked);
                AmericanRoadsignsTool.config.enable_streetprops_parkingmeter = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
            else if (trigger == _randomPropCheckbox)
            {
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (pre).");
                AmericanRoadsignsTool.ChangeLanePropVisibility(AmericanRoadsignsTool.vanillaAssets.Where(x => x.name.ToLower() == "random street prop").FirstOrDefault(), isChecked);
                AmericanRoadsignsTool.config.enable_streetprops_random = isChecked;
                AmericanRoadsignsTool.SaveConfig();
                DebugUtils.Log($"[DEBUG] - CHECKCHANGED VALUE = {isChecked} (post).");
            }
        }
    }
}
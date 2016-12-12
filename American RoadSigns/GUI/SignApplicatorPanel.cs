using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    public class SignApplicatorPanel : UIPanel
    {
        private UILabel _signLabel;
        private UIFastList _signFastList;

        private UIButton _applySignButton;
        private UIButton _cancelButton;

        public UIFastList signFastList
        {
            get { return _signFastList; }
        }
        public UIButton applySignButton
        {
            get { return _applySignButton; }
        }
        public UIButton cancelButton
        {
            get { return _cancelButton; }
        }

        public UISignItem _selectedSign;
        public UISignItem[] _signs;

        private static SignApplicatorPanel _instance;
        public static SignApplicatorPanel instance
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
            PopulateSignFastList();
        }

        private void SetupControls()
        {
            //  Signs:
            var signContainer = UIUtils.CreateFormElement(this, "top");

            _signLabel = signContainer.AddUIComponent<UILabel>();
            _signLabel.text = "Select prop";
            _signLabel.textScale = 0.8f;
            _signLabel.padding = new RectOffset(0, 0, 0, 5);
            // FastList
            _signFastList = UIFastList.Create<UISignItem>(signContainer);
            _signFastList.backgroundSprite = "UnlockingPanel";
            _signFastList.width = parent.width - (3 * AmericanRoadsignsTool.SPACING) - 12;
            _signFastList.height = 90;
            _signFastList.canSelect = true;
            _signFastList.eventSelectedIndexChanged += OnSelectedSignItemChanged;

            //  Buttons:
            var applyCancelContainer = UIUtils.CreateFormElement(this, "bottom");

            _applySignButton = UIUtils.CreateButton(applyCancelContainer);
            _applySignButton.opacity = 0.25f;
            _applySignButton.isEnabled = false;
            _applySignButton.relativePosition = new Vector3(5, 10);
            _applySignButton.name = "applySignButton";
            _applySignButton.text = "Apply sign";
            _applySignButton.tooltip = "Apply selected sign to node/prop/whatever.";
            _applySignButton.eventClicked += (c, e) =>
            {
                //  
                if (AmericanRoadsignsTool.config.enable_debug)
                {
                    DebugUtils.Log($"SignApplicatorPanel: 'Apply sign' clicked: sign '{_selectedSign.name}', target node/prop/whatever.");
                }
                //AmericanRoadsignsTool.ApplySign(_selectedSign.name);
                //  Button appearance:
                updateButtons(true);
            };

            _cancelButton = UIUtils.CreateButton(applyCancelContainer);
            _cancelButton.opacity = 0.25f;
            _cancelButton.isEnabled = true;
            _cancelButton.relativePosition = new Vector3(160, 10);
            _cancelButton.name = "cancelButton";
            _cancelButton.text = "Cancel";
            _cancelButton.tooltip = "Close this panel.";
            _cancelButton.eventClicked += (c, e) =>
            {
                //  Close panel:

            };
        }

        public void PopulateSignFastList()
        {
            //_signFastList.rowsData.Clear();
            //_signFastList.selectedIndex = -1;
            ////  
            //List<SignItem> allSigns = AmericanRoadsignsTool.GetAllSigns();
            //if (allSigns.Count > 0)
            //{
            //    for (int i = 0; i < allSigns.Count; i++)
            //    {
            //        if (allSigns[i] != null)
            //        {
            //            _signFastList.rowsData.Add(allSigns[i]);
            //        }
            //    }
            //    //  
            //    _signFastList.rowHeight = 26f;
            //    _signFastList.DisplayAt(0);
            //}
        }

        protected void OnSelectedSignItemChanged(UIComponent component, int i)
        {
            _selectedSign = _signFastList.rowsData[i] as UISignItem;
            if (AmericanRoadsignsTool.config.enable_debug)
            {
                DebugUtils.Log($"PresetsPanel: FastListItem selected: preset '{_selectedSign.name}'.");
            }
            //  Button appearance:
            updateButtons(false);
        }

        public void updateButtons(bool disableAll)
        {
            //_applySignButton.opacity = (disableAll) ? 0.25f : 1f;
            //_applySignButton.isEnabled = (disableAll) ? false : true;
            //_cancelButton.opacity = (disableAll) ? 0.25f : 1f;
            //_cancelButton.isEnabled = (disableAll) ? false : true;
            //_overwritePresetButton.opacity = (disableAll) ? 0.25f : 1f;
            //_overwritePresetButton.isEnabled = (disableAll) ? false : true;
        }
    }
}

using ColossalFramework.UI;
using UnityEngine;
using System.Collections.Generic;

namespace AmericanRoadSigns.GUI
{
    public class SignCreatorPanel : UIPanel
    {
        private UILabel _propLabel;
        private UIFastList _propFastList;
        private UILabel _textureLabel;
        private UIFastList _textureFastList;

        private UIButton _createSignButton;
        private UIButton _cancelButton;

        private UIButton _resetAmbientButton;
        
        public UIFastList propFastList
        {
            get { return _propFastList; }
        }
        public UIFastList textureFastList
        {
            get { return _textureFastList; }
        }
        public UIButton createSignButton
        {
            get { return _createSignButton; }
        }
        public UIButton cancelButton
        {
            get { return _cancelButton; }
        }

        public UIPropItem _selectedProp;
        public UIPropItem[] _props;

        public UITextureItem _selectedTexture;
        public UITextureItem[] _textures;

        private static SignCreatorPanel _instance;
        public static SignCreatorPanel instance
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
            PopulatePropFastList();
        }

        private void SetupControls()
        {
            //  Props:
            var propContainer = UIUtils.CreateFormElement(this, "top");

            _propLabel = propContainer.AddUIComponent<UILabel>();
            _propLabel.text = "Select prop";
            _propLabel.textScale = 0.8f;
            _propLabel.padding = new RectOffset(0, 0, 0, 5);
            // FastList
            _propFastList = UIFastList.Create<UIPropItem>(propContainer);
            _propFastList.backgroundSprite = "UnlockingPanel";
            _propFastList.width = parent.width - (3 * AmericanRoadsignsTool.SPACING) - 12;
            _propFastList.height = 90;
            _propFastList.canSelect = true;
            _propFastList.eventSelectedIndexChanged += OnSelectedPropItemChanged;

            // Textures:
            var textureContainer = UIUtils.CreateFormElement(this, "center");
            textureContainer.relativePosition = new Vector3(0, 165);

            _textureLabel = propContainer.AddUIComponent<UILabel>();
            _textureLabel.text = "Select texture";
            _textureLabel.textScale = 0.8f;
            _textureLabel.padding = new RectOffset(0, 0, 0, 5);

            _textureFastList = UIFastList.Create<UITextureItem>(textureContainer);
            _textureFastList.backgroundSprite = "UnlockingPanel";
            _textureFastList.width = parent.width - (3 * AmericanRoadsignsTool.SPACING) - 12;
            _textureFastList.height = 90;
            _textureFastList.canSelect = true;
            _textureFastList.eventSelectedIndexChanged += OnSelectedTextureItemChanged;

            //  Buttons:
            var createCancelContainer = UIUtils.CreateFormElement(this, "bottom");

            _createSignButton = UIUtils.CreateButton(createCancelContainer);
            _createSignButton.opacity = 0.25f;
            _createSignButton.isEnabled = false;
            _createSignButton.relativePosition = new Vector3(5, 10);
            _createSignButton.name = "createSignButton";
            _createSignButton.text = "Create sign";
            _createSignButton.tooltip = "Create sign with selected prop and texture.";
            _createSignButton.eventClicked += (c, e) =>
            {
                //  
                if (AmericanRoadsignsTool.config.enable_debug)
                {
                    DebugUtils.Log($"SignCreatorPanel: 'Create Sign' clicked'.");
                }
                //AmericanRoadsignsTool.CreateSign(_selectedProp, _selectedTexture);
                //  Button appearance:
                updateButtons(true);
            };

            _cancelButton = UIUtils.CreateButton(createCancelContainer);
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
        
        public void PopulatePropFastList()
        {
            _propFastList.rowsData.Clear();
            _propFastList.selectedIndex = -1;
            ////    List .crp files in resources folder:
            List<PropInfo> allProps = new List<PropInfo>(); //AmericanRoadsignsTool.GetSignProps();
            if (allProps.Count > 0)
            {
                for (int i = 0; i < allProps.Count; i++)
                {
                    if (allProps[i] != null)
                    {
                        _propFastList.rowsData.Add(allProps[i]);
                    }
                }
                //  
                _propFastList.rowHeight = 26f;
                _propFastList.DisplayAt(0);
            }
        }
        public void PopulateTextureFastList(string propName)
        {
            //_textureFastList.rowsData.Clear();
            //_textureFastList.selectedIndex = -1;
            ////    List texture files (.png) in selected prop folder (child of resources folder):
            //List<Configuration.TextureItem> allTextures = AmericanRoadsignsTool.GetTexturesForProp(_selectedProp);
            //if (allTextures.Count > 0)
            //{
            //    for (int i = 0; i < allTextures.Count; i++)
            //    {
            //        if (allTextures[i] != null)
            //        {
            //            _textureFastList.rowsData.Add(allTextures[i]);
            //        }
            //    }
            //    //  
            //    _textureFastList.rowHeight = 26f;
            //    _textureFastList.DisplayAt(0);
            //}
        }

        protected void OnSelectedPropItemChanged(UIComponent component, int i)
        {
            //  Set selected prop:
            _selectedProp = _propFastList.rowsData[i] as UIPropItem;
            //  Load textures for selected prop in FastList:
            PopulateTextureFastList(_selectedProp.name);
            if (AmericanRoadsignsTool.config.enable_debug)
            {
                DebugUtils.Log($"PresetsPanel: FastListItem selected: preset '{_selectedProp.name}'.");
            }
            //  Enable Apply button (todo: only if Texture FastList pre-selects first texture):
            updateButtons(true);
        }

        protected void OnSelectedTextureItemChanged(UIComponent component, int i)
        {
            _selectedTexture = _textureFastList.rowsData[i] as UITextureItem;
            if (AmericanRoadsignsTool.config.enable_debug)
            {
                DebugUtils.Log($"PresetsPanel: FastListItem selected: preset '{_selectedTexture.name}'.");
            }
            //  Enable Apply button (todo: only if Texture FastList does not pre-select first texture):
            updateButtons(true);
        }

        public void updateButtons(bool enableApply)
        {
            _createSignButton.opacity = (enableApply) ? 1f : 0.25f;
            _createSignButton.isEnabled = enableApply;
        }
    }
}

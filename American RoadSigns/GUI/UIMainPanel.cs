using ColossalFramework.UI;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    class UIMainPanel : UIPanel
    {
        public UIMainTitleBar m_title;

        public UITabstrip panelTabs;
        public UIButton roadSignsButton;
        public UIButton roadPropsButton;

        public UIRoadSignsPanel roadSignsPanel;
        public UIRoadPropsPanel roadPropsPanel;

        private static UIMainPanel _instance;
        public static UIMainPanel instance
        {
            get { return _instance; }
        }

        public static void Initialize()
        {
        }

        public override void Start()
        {
            base.Start();
            _instance = this;
            //  
            backgroundSprite = "LevelBarBackground";
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            name = "modMainPanel";
            padding = new RectOffset(10, 10, 4, 4);
            width = UIUtils.c_modPanelWidth;
            height = UIUtils.c_modPanelHeight;
            relativePosition = new Vector3(10, 60);
            //  
            SetupControls();
        }

        public void SetupControls()
        {
            //  Title Bar:
            m_title = AddUIComponent<UIMainTitleBar>();
            m_title.title = "American RoadSigns " + Mod.version;

            //  Tabs:
            panelTabs = AddUIComponent<UITabstrip>();
            panelTabs.size = new Vector2(UIUtils.c_modPanelInnerWidth, UIUtils.c_tabButtonHeight);
            panelTabs.relativePosition = new Vector2(UIUtils.c_spacing, UIUtils.c_titleBarHeight + UIUtils.c_spacing);

            //  Tab Buttons:
            //  Road signs:
            roadSignsButton = UIUtils.CreateTab(panelTabs, "Road signs", true);
            roadSignsButton.tooltip = "";
            roadSignsButton.width = UIUtils.c_tabButtonWidth;
            roadSignsButton.height = UIUtils.c_tabButtonHeight;
            roadSignsButton.textScale = 0.9f;
            //  Roadside props:
            roadPropsButton = UIUtils.CreateTab(panelTabs, "Road props");
            roadPropsButton.tooltip = "";
            roadPropsButton.width = UIUtils.c_tabButtonWidth;
            roadPropsButton.height = UIUtils.c_tabButtonHeight;
            roadPropsButton.textScale = 0.9f;
            //  Tab Button Events:
            roadSignsButton.eventClick += (c, e) => TabClicked(c, e);
            roadPropsButton.eventClick += (c, e) => TabClicked(c, e);

            //  Main Panel:
            UIPanel body = AddUIComponent<UIPanel>();
            body.width = UIUtils.c_modPanelInnerWidth;
            body.height = UIUtils.c_modPanelInnerHeight;
            //  ScrollRect
            body.relativePosition = new Vector3(5, 36 + 28 + 5);

            //  Section Panels:
            //  Road signs Panel:
            roadSignsPanel = body.AddUIComponent<UIRoadSignsPanel>();
            roadSignsPanel.name = "roadSignsPanel";
            roadSignsPanel.width = UIUtils.c_modPanelInnerWidth;
            roadSignsPanel.height = UIUtils.c_modPanelInnerHeight;
            roadSignsPanel.relativePosition = Vector3.zero;
            roadSignsPanel.isVisible = true;
            //  Roadside props Panel:
            roadPropsPanel = body.AddUIComponent<UIRoadPropsPanel>();
            roadPropsPanel.name = "roadPropsPanel";
            roadPropsPanel.width = UIUtils.c_modPanelInnerWidth;
            roadPropsPanel.height = UIUtils.c_modPanelInnerHeight;
            roadPropsPanel.relativePosition = Vector3.zero;
            roadPropsPanel.isVisible = false;
        }

        private void TabClicked(UIComponent trigger, UIMouseEventParameter e)
        {
            if (AmericanRoadsignsTool.config.enable_debug)
            {
                DebugUtils.Log($"MainPanel: Tab '{trigger.name}' clicked");
            }
            //  
            roadSignsPanel.isVisible = false;
            roadPropsPanel.isVisible = false;

            if (trigger == roadSignsButton)
            {
                roadSignsPanel.isVisible = true;
            }
            if (trigger == roadPropsButton)
            {
                roadPropsPanel.isVisible = true;
            }
        }

        public void Toggle()
        {
            if (_instance.isVisible)
            {
                //  Hide MainPanel:
                _instance.isVisible = false;
                UIMainButton.instance.state = UIButton.ButtonState.Normal;
            }
            else
            {
                //  Show MainPanel:
                _instance.isVisible = true;
                UIMainButton.instance.state = UIButton.ButtonState.Focused;

            }
        }


        //public void AddGuiToggle()
        //{
        //    const int size = 36;

        //    //  Positioned relative to Freecamera Button:
        //    var freeCameraButton = UIView.GetAView().FindUIComponent<UIButton>("Freecamera");
        //    toggleAmericanRoadSignsButton = UIView.GetAView().FindUIComponent<UIPanel>("InfoPanel").AddUIComponent<UIButton>();
        //    toggleAmericanRoadSignsButton.verticalAlignment = UIVerticalAlignment.Middle;
        //    toggleAmericanRoadSignsButton.relativePosition = toggleButtonPositionConflicted(freeCameraButton);
        //    //  
        //    toggleAmericanRoadSignsButton.size = new Vector2(36f, 36f);
        //    toggleAmericanRoadSignsButton.playAudioEvents = true;
        //    toggleAmericanRoadSignsButton.tooltip = "American RoadSigns " + Mod.version;
        //    //  Create custom atlas:
        //    if (toggleButtonAtlas == null)
        //    {
        //        toggleButtonAtlas = UIUtils.CreateAtlas(AR, size, size, "ToolbarAtlas.png", new[]
        //                                    {
        //                                        "RoadSignsNormalBg",
        //                                        "RoadSignsHoveredBg",
        //                                        "RoadSignsPressedBg",
        //                                        "RoadSignsNormalFg",
        //                                        "RoadSignsHoveredFg",
        //                                        "RoadSignsPressedFg",
        //                                        "RoadSignsButtonNormal",
        //                                        "RoadSignsButtonHover",
        //                                        "RoadSignsInfoTextBg",
        //                                    });
        //    }
        //    //  Apply custom sprite:
        //    toggleAmericanRoadSignsButton.atlas = toggleButtonAtlas;
        //    toggleAmericanRoadSignsButton.normalFgSprite = "RoadSignsNormalBg";
        //    toggleAmericanRoadSignsButton.normalBgSprite = null;
        //    toggleAmericanRoadSignsButton.hoveredFgSprite = "RoadSignsHoveredBg";
        //    toggleAmericanRoadSignsButton.hoveredBgSprite = "RoadSignsHoveredFg";
        //    toggleAmericanRoadSignsButton.pressedFgSprite = "RoadSignsPressedBg";
        //    toggleAmericanRoadSignsButton.pressedBgSprite = "RoadSignsPressedFg";
        //    toggleAmericanRoadSignsButton.focusedFgSprite = "RoadSignsPressedBg";
        //    toggleAmericanRoadSignsButton.focusedBgSprite = "RoadSignsPressedFg";
        //    //  Event handling:
        //    toggleAmericanRoadSignsButton.eventClicked += (c, e) =>
        //    {
        //        isVisible = !isVisible;
        //        if (!isVisible)
        //        {
        //            toggleAmericanRoadSignsButton.Unfocus();
        //        }
        //    };

        //}

        ////  Check if Ultimate Eyecandy mod (and by extention Enhanced Mouse Light mod) is present (to determine toggle button position)
        //private static Vector3 toggleButtonPositionConflicted(UIButton freeCameraButton)
        //{
        //    bool MouseLightPresent = PluginManager.instance.GetPluginsInfo().Any(mod => (mod.publishedFileID.AsUInt64 == 527036685 && mod.isEnabled));
        //    bool EyecandyPresent = PluginManager.instance.GetPluginsInfo().Any(mod => (mod.publishedFileID.AsUInt64 == 672248733 && mod.isEnabled));

        //    if (MouseLightPresent && !EyecandyPresent)
        //    {
        //        return new Vector3(freeCameraButton.absolutePosition.x - 76, freeCameraButton.relativePosition.y);
        //    }
        //    else if (!MouseLightPresent && EyecandyPresent)
        //    {
        //        return new Vector3(freeCameraButton.absolutePosition.x - 76, freeCameraButton.relativePosition.y);
        //    }
        //    else if (MouseLightPresent && EyecandyPresent)
        //    {
        //        return new Vector3(freeCameraButton.absolutePosition.x - 110, freeCameraButton.relativePosition.y);
        //    }
        //    else
        //    {
        //        return new Vector3(freeCameraButton.absolutePosition.x - 42, freeCameraButton.relativePosition.y);
        //    }
        //}

        //  Toggle main panel and update button state:
    }
}
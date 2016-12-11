using System;
using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;
using ColossalFramework.Plugins;
using System.Linq;

namespace AmericanRoadSigns.GUI
{

    class ModMainPanel : UIPanel
    {
        public UIMainTitleBar m_title;

        public UITabstrip panelTabs;
        public UIButton signCreatorButton;
        public UIButton signApplicatorButton;

        public SignCreatorPanel signCreatorPanel;
        public SignApplicatorPanel signApplicatorPanel;

        public UIButton toggleAmericanRoadSignsButton;
        public UITextureAtlas toggleButtonAtlas = null;
        static readonly string AR = "AmericanRoadSigns";

        private static GameObject _gameObject;

        private static ModMainPanel _instance;
        public static ModMainPanel instance

        {
            get { return _instance; }
        }

        public static void Initialize()
        {
        }

        public override void Start()
        {
            base.Start();

            backgroundSprite = "LevelBarBackground";
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            name = "modMainPanel";
            padding = new RectOffset(10, 10, 4, 4);
            width = 275;
            height = 36 + 350 + 5;
            relativePosition = new Vector3(10, 60);
            //  
            SetupControls();
        }

        public static void Destroy()
        {
            try
            {
                if (_gameObject != null)
                    GameObject.Destroy(_gameObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void SetupControls()
        {
            //  Title Bar:
            m_title = AddUIComponent<UIMainTitleBar>();
            m_title.title = "American RoadSigns";

            //  Tabs:
            panelTabs = AddUIComponent<UITabstrip>();
            panelTabs.relativePosition = new Vector2(10, AmericanRoadSigns.TITLE_HEIGHT + AmericanRoadSigns.SPACING);
            panelTabs.size = new Vector2(AmericanRoadSigns.WIDTH - (3 * AmericanRoadSigns.SPACING), AmericanRoadSigns.TABS_HEIGHT);

            //  Tab Buttons:
            signCreatorButton = UIUtils.CreateTab(panelTabs, "Create signs", true);
            signCreatorButton.tooltip = "";
            signCreatorButton.textScale = 0.8f;
            signApplicatorButton = UIUtils.CreateTab(panelTabs, "Apply signs");
            signApplicatorButton.tooltip = "";
            signApplicatorButton.textScale = 0.8f;

            //  Main Panel:
            UIPanel body = AddUIComponent<UIPanel>();
            body.width = AmericanRoadSigns.WIDTH;
            body.height = AmericanRoadSigns.HEIGHT;
            //  ScrollRect
            body.relativePosition = new Vector3(5, 36 + 28 + 5);

            //  Section Panels:
            //  Sign Creator Panel:
            signCreatorPanel = body.AddUIComponent<SignCreatorPanel>();
            signCreatorPanel.name = "SignCreatorPanel";
            signCreatorPanel.width = AmericanRoadSigns.WIDTH - (3 * AmericanRoadSigns.SPACING);
            signCreatorPanel.height = AmericanRoadSigns.HEIGHT;
            signCreatorPanel.relativePosition = new Vector3(5, 0);
            signCreatorPanel.isVisible = true;
            //  Sign Applicator Panel:
            signApplicatorPanel = body.AddUIComponent<SignApplicatorPanel>();
            signApplicatorPanel.name = "signApplicatorPanel";
            signApplicatorPanel.width = AmericanRoadSigns.WIDTH - 3 * AmericanRoadSigns.SPACING;
            signApplicatorPanel.height = AmericanRoadSigns.HEIGHT;
            signApplicatorPanel.relativePosition = new Vector3(5, 0);

            signApplicatorPanel.isVisible = false;
            //  Tab Button Events:
            signCreatorPanel.eventClick += (c, e) => TabClicked(c, e);
            signApplicatorPanel.eventClick += (c, e) => TabClicked(c, e);
        }

        private void TabClicked(UIComponent trigger, UIMouseEventParameter e)
        {
            if (AmericanRoadSigns.config.enable_debug)
            {
                DebugUtils.Log($"MainPanel: Tab '{trigger.name}' clicked");
            }
            //  
            signCreatorPanel.isVisible = false;
            signApplicatorPanel.isVisible = false;

            if (trigger == signCreatorButton)
            {
                signCreatorPanel.isVisible = true;
                //  Disable node/prop/whatever selection tool
            }
            if (trigger == signApplicatorButton)
            {
                signApplicatorPanel.isVisible = true;
                //  Enable node/prop/whatever selection tool
            }
        }

        public void AddGuiToggle()
        {
            const int size = 36;

            //  Positioned relative to Freecamera Button:
            var freeCameraButton = UIView.GetAView().FindUIComponent<UIButton>("Freecamera");
            toggleAmericanRoadSignsButton = UIView.GetAView().FindUIComponent<UIPanel>("InfoPanel").AddUIComponent<UIButton>();
            toggleAmericanRoadSignsButton.verticalAlignment = UIVerticalAlignment.Middle;
            toggleAmericanRoadSignsButton.relativePosition = toggleButtonPositionConflicted(freeCameraButton);
            //  
            toggleAmericanRoadSignsButton.size = new Vector2(36f, 36f);
            toggleAmericanRoadSignsButton.playAudioEvents = true;
            toggleAmericanRoadSignsButton.tooltip = "American RoadSigns " + ModInfo.version;
            //  Create custom atlas:
            if (toggleButtonAtlas == null)
            {
                toggleButtonAtlas = UIUtils.CreateAtlas(AR, size, size, "ToolbarAtlas.png", new[]
                                            {
                                                "RoadSignsNormalBg",
                                                "RoadSignsHoveredBg",
                                                "RoadSignsPressedBg",
                                                "RoadSignsNormalFg",
                                                "RoadSignsHoveredFg",
                                                "RoadSignsPressedFg",
                                                "RoadSignsButtonNormal",
                                                "RoadSignsButtonHover",
                                                "RoadSignsInfoTextBg",
                                            });
            }
            //  Apply custom sprite:
            toggleAmericanRoadSignsButton.atlas = toggleButtonAtlas;
            toggleAmericanRoadSignsButton.normalFgSprite = "RoadSignsNormalBg";
            toggleAmericanRoadSignsButton.normalBgSprite = null;
            toggleAmericanRoadSignsButton.hoveredFgSprite = "RoadSignsHoveredBg";
            toggleAmericanRoadSignsButton.hoveredBgSprite = "RoadSignsHoveredFg";
            toggleAmericanRoadSignsButton.pressedFgSprite = "RoadSignsPressedBg";
            toggleAmericanRoadSignsButton.pressedBgSprite = "RoadSignsPressedFg";
            toggleAmericanRoadSignsButton.focusedFgSprite = "RoadSignsPressedBg";
            toggleAmericanRoadSignsButton.focusedBgSprite = "RoadSignsPressedFg";
            //  Event handling:
            toggleAmericanRoadSignsButton.eventClicked += (c, e) =>
            {
                isVisible = !isVisible;
                if (!isVisible)
                {
                    toggleAmericanRoadSignsButton.Unfocus();
                }
            };

        }

        //  Check if Ultimate Eyecandy mod (and by extention Enhanced Mouse Light mod) is present (to determine toggle button position)
        private static Vector3 toggleButtonPositionConflicted(UIButton freeCameraButton)
        {
            bool MouseLightPresent = PluginManager.instance.GetPluginsInfo().Any(mod => (mod.publishedFileID.AsUInt64 == 527036685 && mod.isEnabled));
            bool EyecandyPresent = PluginManager.instance.GetPluginsInfo().Any(mod => (mod.publishedFileID.AsUInt64 == 672248733 && mod.isEnabled));

            if (MouseLightPresent && !EyecandyPresent)
            {
                return new Vector3(freeCameraButton.absolutePosition.x - 76, freeCameraButton.relativePosition.y);
            }
            else if (!MouseLightPresent && EyecandyPresent)
            {
                return new Vector3(freeCameraButton.absolutePosition.x - 76, freeCameraButton.relativePosition.y);
            }
            else if (MouseLightPresent && EyecandyPresent)
            {
                return new Vector3(freeCameraButton.absolutePosition.x - 110, freeCameraButton.relativePosition.y);
            }
            else
            {
                return new Vector3(freeCameraButton.absolutePosition.x - 42, freeCameraButton.relativePosition.y);
            }
        }
    }
}
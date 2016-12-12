using ColossalFramework.UI;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    public class UIMainPanel : UIPanel
    {
        public UIMainTitleBar m_title;

        public UITabstrip panelTabs;
        public UIButton signCreatorButton;
        public UIButton signApplicatorButton;

        public SignCreatorPanel signCreatorPanel;
        public SignApplicatorPanel signApplicatorPanel;

        public static UIMainPanel instance;

        public static void Initialize()
        {
        }

        public override void Start()
        {
            base.Start();
            instance = this;
            //  
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

        public void SetupControls()
        {
            //  Title Bar:
            m_title = AddUIComponent<UIMainTitleBar>();
            m_title.title = "American Roadsigns ";// + AmericanRoadsignsTool.config.version;

            //  Tabs:
            panelTabs = AddUIComponent<UITabstrip>();
            panelTabs.relativePosition = new Vector2(10, AmericanRoadsignsTool.TITLE_HEIGHT + AmericanRoadsignsTool.SPACING);
            panelTabs.size = new Vector2(AmericanRoadsignsTool.WIDTH - (3 * AmericanRoadsignsTool.SPACING), AmericanRoadsignsTool.TABS_HEIGHT);

            //  Tab Buttons:
            signCreatorButton = UIUtils.CreateTab(panelTabs, "Create signs", true);
            signCreatorButton.tooltip = "";
            signCreatorButton.textScale = 0.8f;
            signApplicatorButton = UIUtils.CreateTab(panelTabs, "Apply signs");
            signApplicatorButton.tooltip = "";
            signApplicatorButton.textScale = 0.8f;

            //  Main Panel:
            UIPanel body = AddUIComponent<UIPanel>();
            body.width = AmericanRoadsignsTool.WIDTH;
            body.height = AmericanRoadsignsTool.HEIGHT;
            //  ScrollRect
            body.relativePosition = new Vector3(5, 36 + 28 + 5);

            //  Section Panels:
            //  Sign Creator Panel:
            signCreatorPanel = body.AddUIComponent<SignCreatorPanel>();
            signCreatorPanel.name = "SignCreatorPanel";
            signCreatorPanel.width = AmericanRoadsignsTool.WIDTH - (3 * AmericanRoadsignsTool.SPACING);
            signCreatorPanel.height = AmericanRoadsignsTool.HEIGHT;
            signCreatorPanel.relativePosition = new Vector3(5, 0);
            signCreatorPanel.isVisible = true;
            //  Sign Applicator Panel:
            signApplicatorPanel = body.AddUIComponent<SignApplicatorPanel>();
            signApplicatorPanel.name = "signApplicatorPanel";
            signApplicatorPanel.width = AmericanRoadsignsTool.WIDTH - 3 * AmericanRoadsignsTool.SPACING;
            signApplicatorPanel.height = AmericanRoadsignsTool.HEIGHT;
            signApplicatorPanel.relativePosition = new Vector3(5, 0);

            signApplicatorPanel.isVisible = false;
            //  Tab Button Events:
            signCreatorPanel.eventClick += (c, e) => TabClicked(c, e);
            signApplicatorPanel.eventClick += (c, e) => TabClicked(c, e);
        }

        private void TabClicked(UIComponent trigger, UIMouseEventParameter e)
        {
            if (AmericanRoadsignsTool.config.enable_debug)
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


        //  Toggle main panel and update button state:
        public void Toggle()
        {
            if (instance.isVisible)
            {
                instance.isVisible = false;
                UIMainButton.instance.state = UIButton.ButtonState.Normal;
            }
            else
            {
                instance.isVisible = true;
                UIMainButton.instance.state = UIButton.ButtonState.Focused;
            }
        }
    }
}
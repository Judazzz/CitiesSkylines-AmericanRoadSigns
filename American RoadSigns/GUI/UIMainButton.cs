using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace AmericanRoadSigns.GUI
{
    public class UIMainButton : UIButton
    {
        public static UIMainButton instance;
        private bool dragging = false;

        public override void Start()
        {
            base.Start();
            instance = this;

            const int buttonSize = 36;
            UITextureAtlas toggleButtonAtlas = null;
            string AR = "AmericanRoadSigns";

            //  Positioned relative to Freecamera Button:
            var freeCameraButton = UIView.GetAView().FindUIComponent<UIButton>("Freecamera");
            verticalAlignment = UIVerticalAlignment.Middle;
            //  
            if (AmericanRoadsignsTool.config.buttonposition.y == -9999)
            {
                absolutePosition = new Vector2(freeCameraButton.absolutePosition.x - (4 * buttonSize) - 5, freeCameraButton.absolutePosition.y);
            }
            else
            {
                absolutePosition = AmericanRoadsignsTool.config.buttonposition;
            }
            //  
            size = new Vector2(36f, 36f);
            playAudioEvents = true;
            tooltip = "American Roadsigns " + Mod.version;
            //  Create custom atlas:
            if (toggleButtonAtlas == null)
            {
                toggleButtonAtlas = UIUtils.CreateAtlas(AR, buttonSize, buttonSize, "ToolbarAtlas.png", new[]
                                            {
                                                "RoadsignsNormalBg",
                                                "RoadsignsHoveredBg",
                                                "RoadsignsPressedBg",
                                                "RoadsignsNormalFg",
                                                "RoadsignsHoveredFg",
                                                "RoadsignsPressedFg",
                                                "RoadsignsButtonNormal",
                                                "RoadsignsButtonHover",
                                                "RoadsignsInfoTextBg",
                                            });
            }
            //  Apply custom sprite:
            atlas = toggleButtonAtlas;
            normalFgSprite = "RoadsignsNormalBg";
            normalBgSprite = null;
            hoveredFgSprite = "RoadsignsHoveredBg";
            hoveredBgSprite = "RoadsignsHoveredFg";
            pressedFgSprite = "RoadsignsPressedBg";
            pressedBgSprite = "RoadsignsPressedFg";
            focusedFgSprite = "RoadsignsPressedBg";
            focusedBgSprite = "RoadsignsPressedFg";
        }

        protected override void OnClick(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Left))
            {
                UIMainPanel.instance.Toggle();
            }

            base.OnClick(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                dragging = true;
            }
            base.OnMouseDown(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                dragging = false;
            }
            base.OnMouseUp(p);
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                var ratio = UIView.GetAView().ratio;
                position = new Vector3(position.x + (p.moveDelta.x * ratio), position.y + (p.moveDelta.y * ratio), position.z);
                //  
                AmericanRoadsignsTool.config.buttonposition = absolutePosition;
                AmericanRoadsignsTool.SaveConfig();
                //  
                if (AmericanRoadsignsTool.config.enable_debug)
                {
                    DebugUtils.Log($"Button position changed to {absolutePosition}.");
                }
            }
            base.OnMouseMove(p);
        }
    }
}
using ICities;
using System;
using AmericanRoadSigns.GUI;

namespace AmericanRoadSigns
{
    public class ThreadingExtension : ThreadingExtensionBase
    {
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            try
            {
                //  Execute code only when in-game:
                if (AmericanRoadsignsTool.isGameLoaded)
                {
                    //  Register Hotkey:
                    bool flag = InputUtils.HotkeyPressed();
                    if (flag)
                    {
                        if (AmericanRoadsignsTool.config.enable_debug)
                        {
                            DebugUtils.Log($"Hotkey pressed.");
                        }
                        UIMainPanel.instance.Toggle();
                    }
                }
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
            }
        }
    }
}
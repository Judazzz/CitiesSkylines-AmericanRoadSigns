using ICities;
using System;

namespace AmericanRoadSigns
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            try
            {
                // Create backup:
                AmericanRoadsignsTool.SaveBackup();
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            // Check if in-game:
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                DebugUtils.Log($"Mod not loaded: only available in-game, not in editors.");
                return;
            }
            // 
            AmericanRoadsignsTool.Initialize();
            AmericanRoadsignsTool.LoadConfig();

            //  Set assets path:
            AmericanRoadsignsTool.SetModPath();
            //  Check assets presence:
            //  AmericanRoadsignsTool.CheckProps();
            //  Init. assets:
            AmericanRoadsignsTool.InitProps();
            //  Replace vanilla assets:
            AmericanRoadsignsTool.ReplaceVanillaProps();
            //  Change vanilla assets:
            AmericanRoadsignsTool.ChangeVanillaProps();
            //  
            base.OnLevelLoaded(mode);
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            AmericanRoadsignsTool.isGameLoaded = false;
            AmericanRoadsignsTool.Reset();
        }
    }
}
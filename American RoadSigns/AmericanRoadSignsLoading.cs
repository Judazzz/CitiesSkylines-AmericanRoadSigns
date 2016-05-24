using ColossalFramework.IO;
using ColossalFramework.Steamworks;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AmericanRoadSigns
{
    public class ModLoader : LoadingExtensionBase
    {
        public static string getModPath()
        {
            string workshopPath = ".";
            foreach (PublishedFileId mod in Steam.workshop.GetSubscribedItems())
            {
                if (mod.AsUInt64 == Mod.workshop_id)
                {
                    workshopPath = Steam.workshop.GetSubscribedItemPath(mod) + "\\Assets";
                    break;
                }
            }
            string localPath = DataLocation.modsPath + "\\American RoadSigns\\Assets";
            if (Directory.Exists(localPath))
            {
                workshopPath = localPath;
            }
            //DebugUtils.Log($"workPath: {workshopPath}");
            return workshopPath;
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            AmericanRoadSignsPropReplacer.config = Configuration.Deserialize(AmericanRoadSignsPropReplacer.configPath);
            if (AmericanRoadSignsPropReplacer.config == null)
            {
                AmericanRoadSignsPropReplacer.config = new Configuration();
            }
            AmericanRoadSignsPropReplacer.SaveConfig();

            string path = getModPath();

            AmericanRoadSignsPropReplacer.FindProps();
            AmericanRoadSignsPropReplacer.ReplaceProps();
            AmericanRoadSignsPropReplacer.ChangeProps(path);

            base.OnLevelLoaded(mode);
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
        }
    }
}

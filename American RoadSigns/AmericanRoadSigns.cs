using ColossalFramework.Steamworks;
using ColossalFramework.IO;
using ICities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Diagnostics;

namespace AmericanRoadSigns
{
    public class Mod : IUserMod
    {
        public const UInt64 workshop_id = 690066392;
        public const string version = "1.0.0";

        public string Name
        {
            get { return "American RoadSigns"; }
        }

        public string Description
        {
            get { return "Americanizes Road and Highway Signs."; }
        }

        private void EventEnableDebug(bool c)
        {
            AmericanRoadSignsPropReplacer.config.enable_debug = c;
            AmericanRoadSignsPropReplacer.SaveConfig();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            AmericanRoadSignsPropReplacer.config = Configuration.Deserialize(AmericanRoadSignsPropReplacer.configPath);
            bool flag = AmericanRoadSignsPropReplacer.config == null;
            if (flag)
            {
                AmericanRoadSignsPropReplacer.config = new Configuration();
            }
            AmericanRoadSignsPropReplacer.SaveConfig();


            UIHelperBase group = helper.AddGroup(Name);
            group.AddSpace(10);
            group.AddCheckbox("Write data to debug log", AmericanRoadSignsPropReplacer.config.enable_debug, new OnCheckChanged(EventEnableDebug));
            group.AddSpace(10);
            group.AddGroup("WARNING: enabling debug data may increase loading times considerably!\nEnable this setting is only recommended when you experience problems with this mod.");
            group.AddSpace(20);
        }
    }

    public class Configuration
    {
        public bool disable_optional_arrows = true;
        public bool use_alternate_pavement_texture = false;
        public bool use_cracked_roads = false;
        public float crackIntensity = 1f;
        public bool enable_debug = false;

        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }

        public static void Serialize(string filename, Configuration config)
        {
            var serializer = new XmlSerializer(typeof(Configuration));

            using (var writer = new StreamWriter(filename))
            {
                config.OnPreSerialize();
                serializer.Serialize(writer, config);
            }
        }

        public static Configuration Deserialize(string filename)
        {
            var serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                using (var reader = new StreamReader(filename))
                {
                    var config = (Configuration)serializer.Deserialize(reader);
                    config.OnPostDeserialize();
                    return config;
                }
            }
            catch { }

            return null;
        }
    }

}
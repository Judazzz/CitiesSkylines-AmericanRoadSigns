using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace AmericanRoadSigns
{
    public class Configuration
    {

        //  Road signs:
        public int rendermode_highwaygantry = 0;
        public int rendermode_highwaysign = 0;
        public int rendermode_noparking = 0;
        public int rendermode_noturnings = 0;
        public int rendermode_speedlimits = 0;
        public int rendermode_streetname = 0;
        public bool rendermode_highwaygantry_usealt = false;
        //  Street props:
        public bool enable_manholes_highway = true;
        public bool enable_manholes_elevated = true;
        public bool enable_streetprops_electricitybox = true;
        public bool enable_streetprops_firehydrant = true;
        public bool enable_streetprops_infoterminal = true;
        public bool enable_streetprops_parkingmeter = true;
        public bool enable_streetprops_random = true;
        //  Misc.
        public bool enable_localassets = false;
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



        //public string version;

        //[XmlArray(ElementName = "Signs")]
        //[XmlArrayItem(ElementName = "Sign")]
        //public List<SignItem> signs = new List<SignItem>();

        //public SignItem getSign(string name)
        //{
        //    foreach (SignItem sign in signs)
        //    {
        //        if (sign.name == name) return sign;
        //    }
        //    return null;
        //}

        //public class SignItem
        //{
        //    [XmlAttribute("name")]
        //    public string name;

        //    [XmlElement("prop")]
        //    public PropInfo prop;

        //    [XmlElement("texture")]
        //    public Texture2D texture;

        //    public SignItem(string name)
        //    {
        //        this.name = name;
        //    }

        //    public SignItem(SignItem builtInPreset)
        //    {
        //        name = builtInPreset.name;
        //        prop = builtInPreset.prop;
        //        texture = builtInPreset.texture;
        //    }

        //    public SignItem()
        //    {
        //    }
        //}

        //public static Configuration Deserialize(string filename)
        //{
        //    if (!File.Exists(filename)) return null;

        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
        //    try
        //    {
        //        using (StreamReader streamReader = new StreamReader(filename))
        //        {
        //            return (Configuration)xmlSerializer.Deserialize(streamReader);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Log("Couldn't load configuration (XML malformed?)");
        //        throw e;
        //    }
        //}

        //public static void Serialize(string filename, Configuration config, bool reloadUI)
        //{
        //    string FileName = "CSL_UltimateEyecandy.xml";
        //    string FileNameLocal = "CSL_UltimateEyecandy_local.xml";

        //    var fileName = (PluginManager.noWorkshop) ? FileNameLocal : FileName;
        //    try
        //    {
        //        //  Todo: move serialization code to Serialize method in Config.cs!
        //        var xmlSerializer = new XmlSerializer(typeof(Configuration));
        //        using (var streamWriter = new StreamWriter(fileName))
        //        {
        //            UltimateEyecandy.config.version = ModInfo.version;

        //            var configCopy = new Configuration();
        //            configCopy.version = UltimateEyecandy.config.version;
        //            configCopy.outputDebug = UltimateEyecandy.config.outputDebug;
        //            configCopy.enableAdvanced = UltimateEyecandy.config.enableAdvanced;
        //            configCopy.loadLastPresetOnStart = UltimateEyecandy.config.loadLastPresetOnStart;
        //            configCopy.lastPreset = UltimateEyecandy.config.lastPreset;

        //            foreach (var preset in UltimateEyecandy.config.presets)
        //            {
        //                //  Skip Temporary Preset:
        //                if (preset.name == string.Empty)
        //                    continue;
        //                //  Existing presets:
        //                var newPreset = new Preset
        //                {
        //                    name = preset.name,
        //                    ambient_height = preset.ambient_height,
        //                    ambient_rotation = preset.ambient_rotation,
        //                    ambient_intensity = preset.ambient_intensity,
        //                    ambient_ambient = preset.ambient_ambient,
        //                    //ambient_fov = preset.ambient_fov,
        //                    weather = preset.weather,
        //                    weather_rainintensity = preset.weather_rainintensity,
        //                    weather_rainmotionblur = preset.weather_rainmotionblur,
        //                    weather_fogintensity = preset.weather_fogintensity,
        //                    weather_snowintensity = preset.weather_snowintensity,
        //                    color_selectedlut = preset.color_selectedlut,
        //                    color_lut = preset.color_lut,
        //                    color_tonemapping = preset.color_tonemapping,
        //                    color_bloom = preset.color_bloom
        //                };
        //                configCopy.presets.Add(newPreset);
        //            }
        //            xmlSerializer.Serialize(streamWriter, configCopy);
        //            UltimateEyecandy.config = configCopy;
        //            if (reloadUI)
        //            {
        //                PresetsPanel.instance.PopulatePresetsFastList();
        //            }
        //            //  
        //            if (config.outputDebug)
        //            {
        //                DebugUtils.Log("Configuration saved.");
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        DebugUtils.LogException(e);
        //    }
        //}
    }
}
using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace AmericanRoadSigns
{
    public class Configuration
    {
        //  Misc.
        public string version;
        public int keyboardshortcut = 0;
        public Vector3 buttonposition = new Vector3(-9999, -9999, -9999);
        public bool enable_debug = false;
        public bool enable_localassets = false;
        //  Road signs:
        public int rendermode_highwaygantry = 0;
        public bool rendermode_highwaygantry_usealt = false;
        public int rendermode_highwaysign = 0;
        public int rendermode_noparking = 0;
        public int rendermode_noturnings = 0;
        public int rendermode_speedlimits = 0;
        public int rendermode_streetname = 0;
        //  Street props:
        public bool enable_manholes_highway = true;
        public bool enable_manholes_elevated = true;
        public bool enable_streetprops_electricitybox = true;
        public bool enable_streetprops_firehydrant = true;
        public bool enable_streetprops_infoterminal = true;
        public bool enable_streetprops_parkingmeter = true;
        public bool enable_streetprops_random = true;

        public static void Save()
        {
            var fileName = (PluginManager.noWorkshop) ? AmericanRoadsignsTool.ConfigFileNameLocal : AmericanRoadsignsTool.ConfigFileNameOnline;

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(Configuration));
                using (var streamWriter = new StreamWriter(fileName))
                {
                    AmericanRoadsignsTool.config.version = Mod.version;

                    var configCopy = new Configuration();
                    configCopy.version = AmericanRoadsignsTool.config.version;
                    configCopy.keyboardshortcut = AmericanRoadsignsTool.config.keyboardshortcut;
                    configCopy.buttonposition = AmericanRoadsignsTool.config.buttonposition;
                    configCopy.enable_debug = AmericanRoadsignsTool.config.enable_debug;
                    configCopy.enable_localassets = AmericanRoadsignsTool.config.enable_localassets;
                    //  
                    configCopy.rendermode_highwaygantry = AmericanRoadsignsTool.config.rendermode_highwaygantry;
                    configCopy.rendermode_highwaygantry_usealt = AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt;
                    configCopy.rendermode_highwaysign = AmericanRoadsignsTool.config.rendermode_highwaysign;
                    configCopy.rendermode_noparking = AmericanRoadsignsTool.config.rendermode_noparking;
                    configCopy.rendermode_noturnings = AmericanRoadsignsTool.config.rendermode_noturnings;
                    configCopy.rendermode_speedlimits = AmericanRoadsignsTool.config.rendermode_speedlimits;
                    configCopy.rendermode_streetname = AmericanRoadsignsTool.config.rendermode_streetname;
                    //  
                    configCopy.enable_manholes_highway = AmericanRoadsignsTool.config.enable_manholes_highway;
                    configCopy.enable_manholes_elevated = AmericanRoadsignsTool.config.enable_manholes_elevated;
                    configCopy.enable_streetprops_electricitybox = AmericanRoadsignsTool.config.enable_streetprops_electricitybox;
                    configCopy.enable_streetprops_firehydrant = AmericanRoadsignsTool.config.enable_streetprops_firehydrant;
                    configCopy.enable_streetprops_infoterminal = AmericanRoadsignsTool.config.enable_streetprops_infoterminal;
                    configCopy.enable_streetprops_parkingmeter = AmericanRoadsignsTool.config.enable_streetprops_parkingmeter;
                    configCopy.enable_streetprops_random = AmericanRoadsignsTool.config.enable_streetprops_random;

                    xmlSerializer.Serialize(streamWriter, configCopy);

                    //  
                    if (AmericanRoadsignsTool.config.enable_debug)
                    {
                        DebugUtils.Log("Configuration saved.");
                    }
                }
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
            }

            //var serializer = new XmlSerializer(typeof(Configuration));

            //using (var writer = new StreamWriter(fileName))
            //{
            //    config.OnPreSerialize();
            //    serializer.Serialize(writer, config);
            //}
        }

        public static Configuration Load(string filename)
        {
            if (!File.Exists(filename)) return null;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
            try
            {
                using (StreamReader streamReader = new StreamReader(filename))
                {
                    return (Configuration)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't load configuration (XML malformed?)");
                throw e;
            }
            //var serializer = new XmlSerializer(typeof(Configuration));

            //try
            //{
            //    using (var reader = new StreamReader(filename))
            //    {
            //        var config = (Configuration)serializer.Deserialize(reader);
            //        config.OnPostDeserialize();
            //        return config;
            //    }
            //}
            //catch { }

            //return null;
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
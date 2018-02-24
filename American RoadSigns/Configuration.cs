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
        //  Road sign pack:
        public int roadsignpack = 1;
        //  Road signs:
        public bool enable_gantrysigns = true;
        public bool enable_highwaysigns = true;
        public bool enable_noparkingsigns = true;
        public bool enable_turningsigns = true;
        public bool enable_speedlimitsigns = true;
        public bool enable_streetnamesigns = true;
        //  Textures:
        public bool rendermode_highwaygantry_usealt = false;
        //  Street props:
        public bool enable_manholes_highway = false;
        public bool enable_manholes_elevated = false;
        public bool enable_streetprops_electricitybox = true;
        public bool enable_streetprops_firehydrant = true;
        public bool enable_streetprops_infoterminal = true;
        public bool enable_streetprops_parkingmeter = true;
        public bool enable_streetprops_random = true;

        //  Obsolete
        //public int rendermode_highwaygantry = 0;
        //public bool rendermode_highwaygantry_usealt = false;
        //public int rendermode_highwaysign = 0;
        //public int rendermode_noparking = 0;
        //public int rendermode_noturnings = 0;
        //public int rendermode_speedlimits = 0;
        //public int rendermode_streetname = 0;

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
                    //  Road sign pack:
                    configCopy.roadsignpack = AmericanRoadsignsTool.config.roadsignpack;
                    //  Road signs::
                    configCopy.enable_gantrysigns = AmericanRoadsignsTool.config.enable_gantrysigns;
                    configCopy.enable_highwaysigns = AmericanRoadsignsTool.config.enable_highwaysigns;
                    configCopy.enable_noparkingsigns = AmericanRoadsignsTool.config.enable_noparkingsigns;
                    configCopy.enable_turningsigns = AmericanRoadsignsTool.config.enable_turningsigns;
                    configCopy.enable_speedlimitsigns = AmericanRoadsignsTool.config.enable_speedlimitsigns;
                    configCopy.enable_streetnamesigns = AmericanRoadsignsTool.config.enable_streetnamesigns;
                    //  Textures:
                    configCopy.rendermode_highwaygantry_usealt = AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt;
                    //  Street props:
                    configCopy.enable_manholes_highway = AmericanRoadsignsTool.config.enable_manholes_highway;
                    configCopy.enable_manholes_elevated = AmericanRoadsignsTool.config.enable_manholes_elevated;
                    configCopy.enable_streetprops_electricitybox = AmericanRoadsignsTool.config.enable_streetprops_electricitybox;
                    configCopy.enable_streetprops_firehydrant = AmericanRoadsignsTool.config.enable_streetprops_firehydrant;
                    configCopy.enable_streetprops_infoterminal = AmericanRoadsignsTool.config.enable_streetprops_infoterminal;
                    configCopy.enable_streetprops_parkingmeter = AmericanRoadsignsTool.config.enable_streetprops_parkingmeter;
                    configCopy.enable_streetprops_random = AmericanRoadsignsTool.config.enable_streetprops_random;

                    //  Obsolete:
                    //configCopy.rendermode_highwaygantry = AmericanRoadsignsTool.config.rendermode_highwaygantry;
                    //configCopy.rendermode_highwaygantry_usealt = AmericanRoadsignsTool.config.rendermode_highwaygantry_usealt;
                    //configCopy.rendermode_highwaysign = AmericanRoadsignsTool.config.rendermode_highwaysign;
                    //configCopy.rendermode_noparking = AmericanRoadsignsTool.config.rendermode_noparking;
                    //configCopy.rendermode_noturnings = AmericanRoadsignsTool.config.rendermode_noturnings;
                    //configCopy.rendermode_speedlimits = AmericanRoadsignsTool.config.rendermode_speedlimits;
                    //configCopy.rendermode_streetname = AmericanRoadsignsTool.config.rendermode_streetname;

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
        }
    }
}
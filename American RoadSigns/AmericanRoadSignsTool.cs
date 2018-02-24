using AmericanRoadSigns.GUI;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AmericanRoadSigns
{
    public class AmericanRoadsignsTool : MonoBehaviour
    {
        public static AmericanRoadsignsTool instance;
        private static UIMainButton m_mainbutton;
        private static UIMainPanel m_mainpanel;

        public static Configuration config;
        public static bool isGameLoaded;
        public static bool isNoWorkshop;
        public static string propPrefix;

        //  I/O:
        public static string ConfigFileName;
        public static readonly string ConfigFileNameOnline = "CSL_AmericanRoadSigns.xml";
        public static readonly string ConfigFileNameLocal = "CSL_AmericanRoadSigns_local.xml";
        public static readonly string CustomFileNameOnline = "CSL_AmericanRoadSigns_Custom.xml";
        public static readonly string CustomFileNameLocal = "CSL_AmericanRoadSigns_Custom_local.xml";

        public static string ModPath;
        public static string strModLocation;

        //  'Reference Library' Members:
        //  Included assets and textures:
        public static string[] USRoadsignsDependencies = new string[] {
            "motorway-overroad-signs.dds",
            "motorway-overroad-signs-motorway-overroad-signs-aci.dds",
            "motorway-overroad-signs-motorway-overroad-signs-xys.dds",
            "street-name-sign.dds",
            "speed limit 15.crp",
            "speed limit 25.crp",
            "speed limit 30.crp",
            "speed limit 45.crp",
            "speed limit 65.crp",
            "us interstate sign.crp",
            "us no left turn.crp",
            "us no right turn.crp",
            "us no parking.crp"
        };

        //public static string[] UKRoadsignsDependencies = new string[] {
        //    "motorway-overroad-signs.dds",
        //    "motorway-overroad-signs-motorway-overroad-signs-aci.dds",
        //    "motorway-overroad-signs-motorway-overroad-signs-xys.dds",
        //    "street-name-sign.dds",
        //    "speed limit 15.crp",
        //    "speed limit 25.crp",
        //    "speed limit 30.crp",
        //    "speed limit 45.crp",
        //    "speed limit 65.crp",
        //    "us interstate sign.crp",
        //    "us no left turn.crp",
        //    "us no right turn.crp",
        //    "us no parking.crp"
        //};

        //  Highway Networks:
        public static string[] HighwayNetworksWithoutManholes = new string[] {
            "1242750637.HighwayRampElevated1", // Highways
            "1242750637.HighwayRampElevated0",
            "1242750637.HighwayRamp Tunnel0",
            "1242750637.HighwayRamp Slope0",
            "Large Highway Slope",
            "Large Highway Tunnel",
            "Large Highway Bridge",
            "Large Highway Elevated",
            "Large Highway",
            "Five-Lane Highway Slope",
            "Five-Lane Highway Tunnel",
            "Five-Lane Highway Bridge",
            "Five-Lane Highway Elevated",
            "Five-Lane Highway",
            "Four-Lane Highway Slope",
            "Four-Lane Highway Tunnel",
            "Four-Lane Highway Bridge",
            "Four-Lane Highway Elevated",
            "Four-Lane Highway",
            "Highway2L2W Slope",
            "Highway2L2W Tunnel",
            "Highway2L2W Bridge",
            "Highway2L2W Elevated",
            "Highway2L2W",
            "AsymHighwayL1R2 Slope",
            "AsymHighwayL1R2 Tunnel",
            "AsymHighwayL1R2 Bridge",
            "AsymHighwayL1R2 Elevated",
            "AsymHighwayL1R2",
            "Rural Highway Slope",
            "Rural Highway Tunnel",
            "Rural Highway Bridge",
            "Rural Highway Elevated",
            "Rural Highway",
            "Small Rural Highway Slope",
            "Small Rural Highway Tunnel",
            "Small Rural Highway Bridge",
            "Small Rural Highway Elevated",
            "Small Rural Highway",
            "Highway Barrier",
            "Highway Bridge",
            "Highway Elevated",
            "Highway Slope",
            "Highway Tunnel",
            "Highway",
            "HighwayRamp Slope",
            "HighwayRamp Tunnel",
            "HighwayRamp",
            "HighwayRampElevated",
            "Four Lane Highway Barrier",
            "Four Lane Highway Bridge",
            "Four Lane Highway Elevated",
            "Four Lane Highway Slope",
            "Four Lane Highway Tunnel",
            "Four Lane Highway",
            "Two Lane Highway Barrier",
            "Two Lane Highway Bridge",
            "Two Lane Highway Elevated",
            "Two Lane Highway Slope",
            "Two Lane Highway Tunnel",
            "Two Lane Highway",
            "Two Lane Highway Twoway Barrier",
            "Two Lane Highway Twoway Bridge",
            "Two Lane Highway Twoway Elevated",
            "Two Lane Highway Twoway Slope",
            "Two Lane Highway Twoway Tunnel",
            "Two Lane Highway Twoway"
        };

        //  Elevated/Bridge Networks:
        public static string[] RoadNetworksWithoutManholes = new string[] {
            "Eight-Lane Avenue Bridge", // Regular Roads
            "Eight-Lane Avenue Elevated",
            "Six-Lane Avenue Median Bridge",
            "Six-Lane Avenue Median Elevated",
            "Medium Avenue TL Bridge",
            "Medium Avenue TL Elevated",
            "Medium Avenue Bridge",
            "Medium Avenue Elevated",
            "Oneway4L Bridge",
            "Oneway4L Elevated",
            "AsymRoadL1R3 Bridge",
            "AsymRoadL1R3 Elevated",
            "AsymAvenueL2R3 Bridge",
            "AsymAvenueL2R3 Elevated",
            "AsymAvenueL2R4 Bridge",
            "AsymAvenueL2R4 Elevated",
            "Small Avenue Bridge",
            "Small Avenue Elevated",
            "Oneway3L Bridge",
            "Oneway3L Elevated",
            "BasicRoadMdn Bridge",
            "BasicRoadMdn Elevated",
            "AsymRoadL1R2 Bridge",
            "AsymRoadL1R2 Elevated",
            "BasicRoadTL Bridge",
            "BasicRoadTL Elevated",
            "BasicRoadPntMdn Bridge",
            "BasicRoadPntMdn Elevated",
            "Large Oneway Bridge",
            "Large Oneway Elevated",
            "Large Road Bridge",
            "Large Road Elevated",
            "Medium Road Bridge",
            "Medium Road Elevated",
            "Basic Road Bridge",
            "Basic Road Elevated",
            "Oneway Road Bridge",
            "Oneway Road Elevated",
            "Asymmetrical Three Lane Road Bridge",
            "Asymmetrical Three Lane Road Elevated",
            "Avenue Large With Buslanes Grass Bridge",
            "Avenue Large With Buslanes Grass Elevated",
            "Avenue Large With Grass Bridge",
            "Avenue Large With Grass Elevated",
            //"Large Road Bridge With Bus Lanes", // Roads with Bus Lanes
            //"Large Road Elevated With Bus Lanes",
            //"Small Busway OneWay Bridge",
            //"Small Busway OneWay Elevated",
            //"Small Busway Bridge",
            //"Small Busway Elevated",
            //"Large Road Bridge Bus",
            //"Large Road Elevated Bus",
            //"Medium Road Bridge Bus",
            //"Medium Road Elevated Bus",
            //"Medium Road Bridge Tram", // Roads with Tram Tracks
            //"Medium Road Elevated Tram",
            //"Basic Road Bridge Tram",
            //"Basic Road Elevated Tram",
            //"Oneway Road Bridge Tram",
            //"Oneway Road Elevated Tram",
            //"Oneway Tram Track Bridge",
            //"Oneway Tram Track Elevated",
            //"Tram Track Bridge",
            //"Tram Track Elevated",
            //"Medium Road Monorail Elevated", // Roads with Monorail Tracks
            //"Small Road Monorail Bridge",
            //"Small Road Monorail Elevated",
            //"Oneway with bicycle lanes Bridge", // Roads with Bike Lanes
            //"Oneway with bicycle lanes Elevated",
            //"Large Road Bridge Bike",
            //"Large Road Elevated Bike",
            //"Medium Road Bridge Bike",
            //"Medium Road Elevated Bike",
            //"Basic Road Bridge Bike",
            //"Basic Road Elevated Bike",
        };

        public static Texture gantryMainTexture = new Texture();
        public static Texture gantryACITexture = new Texture();
        public static Texture gantryXYSTexture = new Texture();
        public static Texture streetnameMainTexture = new Texture();

        public static List<PropInfo> vanillaAssets = new List<PropInfo>();
        public static List<PropInfo> workshopAssets = new List<PropInfo>();
        public static List<CustomizableRoadsignItem> CustomizableRoadsignsList = new List<CustomizableRoadsignItem>();

        //  METHODS \\
        //  
        public static void Reset()
        {
            CustomizableRoadsignsList.Clear();
            //
            var go = FindObjectOfType<AmericanRoadsignsTool>();
            if (go != null)
            {
                Destroy(go);
            }

            config = null; // do??
            isGameLoaded = false;
        }

        public static void Initialize()
        {
            //  Map = European?
            //  
            var go = new GameObject("AmericanRoadsignsTool");
            try
            {
                go.AddComponent<AmericanRoadsignsTool>();
                //  Init. GUI Components:
                m_mainbutton = UIView.GetAView().AddUIComponent(typeof(UIMainButton)) as UIMainButton;
                DebugUtils.Log("MainButton created.");
                m_mainpanel = UIView.GetAView().AddUIComponent(typeof(UIMainPanel)) as UIMainPanel;
                DebugUtils.Log("MainPanel created.");
                //  Set vars:
                isGameLoaded = true;
                isNoWorkshop = PluginManager.noWorkshop;
                propPrefix = (isNoWorkshop) ? "418637762" : "690066392"; // (local = 418637762, workshop = 690066392)
                strModLocation = "Workshop";
                ConfigFileName = (isNoWorkshop) ? ConfigFileNameLocal : ConfigFileNameOnline;
                DebugUtils.Log($"Currently used config File: {ConfigFileName}.");
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
                if (go != null)
                {
                    Destroy(go);
                }
            }
        }

        //  Config:
        public static void SaveBackup()
        {
            ConfigFileName = (PluginManager.noWorkshop) ? ConfigFileNameLocal : ConfigFileNameOnline;
            if (!File.Exists(ConfigFileName)) return;

            File.Copy(ConfigFileName, ConfigFileName + ".bak", true);
            //  
            if (config.enable_debug)
            {
                DebugUtils.Log("Backup configuration file created.");
            }
        }

        public static void RestoreBackup()
        {
            ConfigFileName = (PluginManager.noWorkshop) ? ConfigFileNameLocal : ConfigFileNameOnline;
            if (!File.Exists(ConfigFileName + ".bak")) return;

            File.Copy(ConfigFileName + ".bak", ConfigFileName, true);
            //  
            if (config.enable_debug)
            {
                DebugUtils.Log("Backup configuration file restored.");
            }
        }

        public static void LoadConfig()
        {
            if (!isGameLoaded)
            {
                var fileName = (PluginManager.noWorkshop) ? ConfigFileNameLocal : ConfigFileNameOnline;
                if (File.Exists(fileName))
                {
                    //  Load config:
                    config = Configuration.Load(fileName);
                    //  Save config file so in case of new properties, they will be added with default values:
                    SaveConfig();

                    if (config.enable_debug)
                    {
                        DebugUtils.Log($"OnSettingsUI: configuration loaded (file name: {fileName}).");
                    }
                }
                else
                {
                    //  No config: create and save new config:
                    config = new Configuration();
                    SaveConfig();
                }
                return;
            }
            //  Load config:
            if (!File.Exists(ConfigFileName))
            {
                //  No config:
                if (config.enable_debug)
                {
                    DebugUtils.Log($"OnLevelLoaded: No configuration found, new configuration file created (file name: {ConfigFileName}).");
                }
                //  Create and save new config:
                config = new Configuration();
                SaveConfig();
                return;
            }
            //  
            config = Configuration.Load(ConfigFileName);
            if (config.enable_debug)
            {
                DebugUtils.Log($"OnLevelLoaded: Configuration loaded (file name: {ConfigFileName}).");
            }
            return;
        }

        public static void SaveConfig()
        {
            Configuration.Save();
        }

        //  Asset-related:
        public static void SetModPath()
        {
            Workshop workshopInstance = new Workshop();
            string workshopPath = ".";
            foreach (PublishedFileId mod in workshopInstance.GetSubscribedItems())
            {
                if (mod.AsUInt64 == Mod.workshop_id)
                {
                    workshopPath = workshopInstance.GetSubscribedItemPath(mod);
                    if (config.enable_debug)
                    {
                        DebugUtils.Log($"Mod path: {workshopPath}.");
                    }
                    break;
                }
            }
            //  Use included assets - check if all assets are present:
            if (CheckProps(workshopPath))
            {
                if (config.enable_debug)
                {
                    DebugUtils.Log($"Load included props and textures from Workshop folder ({workshopPath}).");
                }
                ModPath = workshopPath;
                return;
            }
            else
            {
                if (config.enable_debug)
                {
                    DebugUtils.Log("Not all included props and textures found in Workshop folder: please contact the mod creator.");
                }
            }
        }

        public static bool CheckProps(string path)
        {
            bool dependenciesPresent = true;
            foreach (var IncludedDependency in USRoadsignsDependencies)
            {
                if (File.Exists(path + "\\" + IncludedDependency))
                {
                    if (config.enable_debug)
                    {
                        DebugUtils.Log($"Custom '{IncludedDependency}' found in {strModLocation} folder '{path}'.");
                    }
                }
                else
                {
                    if (config.enable_debug)
                    {
                        DebugUtils.Log($"Custom '{IncludedDependency}' NOT found in {strModLocation} folder '{path}'.");
                    }
                    dependenciesPresent = false;
                }
            }
            //  
            if (dependenciesPresent)
            {
                if (config.enable_debug)
                {
                    DebugUtils.Log($"All included props and textures found in {strModLocation} folder.");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void InitProps()
        {
            string propNameTest = string.Empty;
            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                PropInfo prop = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                //  Check to avoid 'incompatible' props causing errors on European maps (like bus stop props):
                try
                {
                    propNameTest = prop.name;
                }
                catch
                {
                    continue;
                }
                //  Custom props:
                if (prop.m_isCustomContent)
                {
                    CustomizableRoadsignItem roadsignItem = new CustomizableRoadsignItem();
                    roadsignItem._originalSign = new PropInfo();
                    roadsignItem._customSign = new PropInfo();
                    if (prop.name.ToLower().Contains($"{propPrefix}.speed limit 65"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("100 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.speed limit 45")) //690066392.speed limit 45_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("60 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.speed limit 30")) //690066392.speed limit 30_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("50 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.speed limit 25")) // 690066392.speed limit 25_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("40 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.speed limit 15")) //690066392.speed limit 15_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("30 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.us interstate sign")) //690066392.us interstate sign_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("Motorway Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.us no left turn")) //690066392.us no left turn_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("No Left Turn Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.us no right turn")) //690066392.us no right turn_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("No Right Turn Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains($"{propPrefix}.us no parking")) //690066392.us no parking_Data
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("No Parking Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }

                    foreach (var p in workshopAssets)
                    {
                        DebugUtils.Log($"[DEBUGGER] - sign name = {p.name}.");
                    }
                }
                //  Vanilla props:
                else
                {
                    // Highway Gantry/Street Name sign:
                    if (prop.name.ToLower().Equals("motorway overroad signs") || prop.name.ToLower().Equals("street name sign"))
                    {
                        CustomizableRoadsignItem roadsignItem = new CustomizableRoadsignItem();
                        roadsignItem._originalSign = new PropInfo();
                        roadsignItem._originalSign = prop;
                        roadsignItem._customSign = null;
                        CustomizableRoadsignsList.Add(roadsignItem);
                        vanillaAssets.Add(prop);

                        if (prop.name.ToLower().Equals("motorway overroad signs"))
                        {
                            gantryMainTexture = prop.m_material.GetTexture("_MainTex");
                            gantryACITexture = prop.m_material.GetTexture("_ACIMap");
                            gantryXYSTexture = prop.m_material.GetTexture("_XYSMap");
                        }
                        else
                        {
                            streetnameMainTexture = prop.m_material.GetTexture("_MainTex");
                        }
                    }
                    else if (// Road signs:
                        prop.name.ToLower().Equals("motorway sign") ||
                        prop.name.ToLower().Equals("30 speed limit") ||
                        prop.name.ToLower().Equals("40 speed limit") ||
                        prop.name.ToLower().Equals("50 speed limit") ||
                        prop.name.ToLower().Equals("60 speed limit") ||
                        prop.name.ToLower().Equals("100 speed limit") ||
                        prop.name.ToLower().Equals("no left turn sign") ||
                        prop.name.ToLower().Equals("no right turn sign") ||
                        prop.name.ToLower().Equals("no parking sign") ||
                        //  Road props:
                        prop.name.ToLower().Equals("manhole") ||
                        //  Roadside props:
                        prop.name.ToLower().Equals("electricity box") ||
                        prop.name.ToLower().Equals("fire hydrant") ||
                        prop.name.ToLower().Equals("info terminal") ||
                        prop.name.ToLower().Equals("parking meter") ||
                        prop.name.ToLower().Equals("random street prop")) {
                        vanillaAssets.Add(prop);
                    }
                }
            }
            if (config.enable_debug)
            {
                DebugUtils.Log($"[DEBUG] - Number of vanilla props added to list: {vanillaAssets.Count()} (expect 17).");
                DebugUtils.Log($"[DEBUG] - Number of workshop props added to list: {workshopAssets.Count()} (expect 9).");
                DebugUtils.Log($"[DEBUG] - Number of replaceable road sign props: {CustomizableRoadsignsList.Count()} (expect 11).");
            }
        }


        //  OnLoad:
        public static void ReplacePropsOnLoad()
        {
            //  Replace:
            List<CustomizableRoadsignItem> affectedHighwaySign = new List<CustomizableRoadsignItem>();
            affectedHighwaySign.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway sign").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedHighwaySign);

            List<CustomizableRoadsignItem> affectedNoparkingSign = new List<CustomizableRoadsignItem>();
            affectedNoparkingSign.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no parking sign").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedNoparkingSign);

            List<CustomizableRoadsignItem> affectedNoturningSigns = new List<CustomizableRoadsignItem>();
            affectedNoturningSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no left turn sign").FirstOrDefault());
            affectedNoturningSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no right turn sign").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedNoturningSigns);

            List<CustomizableRoadsignItem> affectedSpeedlimitSigns = new List<CustomizableRoadsignItem>();
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "30 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "40 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "50 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "60 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "100 speed limit").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedSpeedlimitSigns);
        }

        public static void RetexturePropsOnLoad()
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
            //  Roadside props:
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    var prefabName = prefab.name;
                    //  config.rendermode_x: 0 = American, 1 = Vanilla, 2 = Hide:

                    //  Highway gantry:
                    if (prefabName.ToLower().Equals("motorway overroad signs"))
                    {
                        RetextureRoadSignProp(prefabName, config.rendermode_highwaygantry_usealt);
                        ////  Custom:
                        ////var tex = new Texture2D (1, 1);
                        ////tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs.png")));
                        ////prefab.m_material.mainTexture = tex;
                        ////  Global:
                        //if (config.rendermode_highwaygantry_usealt)
                        //{
                        //    prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-alt.dds")));
                        //}
                        //else
                        //{
                        //    prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs.dds")));
                        //}
                        //prefab.m_material.SetTexture("_ACIMap", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-motorway-overroad-signs-aci.dds")));
                        //prefab.m_material.SetTexture("_XYSMap", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-motorway-overroad-signs-xys.dds")));
                        //prefab.m_lodRenderDistance = 100000;
                        //prefab.m_lodMesh = null;
                        //prefab.RefreshLevelOfDetail();
                        //if (config.enable_debug)
                        //{
                        //    DebugUtils.Log("Motorway overhead sign props retextured successfully.");
                        //}
                    }
                    //  Street name sign:
                    else if (prefabName.ToLower().Equals("motorway sign"))
                    {
                        //RetextureRoadSignProp(prefabName, false);
                        //prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "street-name-sign.dds")));
                        //prefab.m_lodRenderDistance = 100000;
                        //prefab.m_lodMesh = null;
                        //prefab.RefreshLevelOfDetail();
                        //if (config.enable_debug)
                        //{
                        //    DebugUtils.Log("Street name sign props retextured successfully.");
                        //}
                    }
                    //  Street name sign:
                    else if (prefabName.ToLower().Equals("street name sign"))
                    {
                        RetextureRoadSignProp(prefabName, false);
                        //prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "street-name-sign.dds")));
                        //prefab.m_lodRenderDistance = 100000;
                        //prefab.m_lodMesh = null;
                        //prefab.RefreshLevelOfDetail();
                        //if (config.enable_debug)
                        //{
                        //    DebugUtils.Log("Street name sign props retextured successfully.");
                        //}
                    }
                }
            }
        }

        public static void HidePropsOnLoad()
        {
            if (!config.enable_gantrysigns)
            {
                ChangeRoadsignPropVisibility("gantry", config.enable_gantrysigns);
            }
            if (!config.enable_highwaysigns)
            {
                ChangeRoadsignPropVisibility("highway", config.enable_highwaysigns);
            }
            if (!config.enable_noparkingsigns)
            {
                ChangeRoadsignPropVisibility("noparking", config.enable_noparkingsigns);
            }
            if (!config.enable_turningsigns)
            {
                ChangeRoadsignPropVisibility("turning", config.enable_turningsigns);
            }
            if (!config.enable_speedlimitsigns)
            {
                ChangeRoadsignPropVisibility("speedlimit", config.enable_speedlimitsigns);
            }
            if (!config.enable_streetnamesigns)
            {
                ChangeRoadsignPropVisibility("streetname", config.enable_streetnamesigns);
            }
        }

        public static void ChangePropsOnLoad()
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
            //  Roadside props:
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    var prefabName = prefab.name.ToLower();
                    //  Street props:
                    if ((prefabName.Equals("electricity box") && !config.enable_streetprops_electricitybox) ||
                            (prefabName.Equals("fire hydrant") && !config.enable_streetprops_firehydrant) ||
                            (prefabName.Equals("info terminal") && !config.enable_streetprops_infoterminal) ||
                            (prefabName.Equals("parking meter") && !config.enable_streetprops_parkingmeter) ||
                            (prefabName.Equals("random street prop") && !config.enable_streetprops_random)) {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"{prefab.name} props hidden successfully.");
                        }
                    }
                }
            }

            //  Laneprops:
            if (!config.enable_manholes_highway || !config.enable_manholes_elevated)
            {
                //  Highway manhole covers:
                if (!config.enable_manholes_highway)
                {
                    var highwayNetworks = Resources.FindObjectsOfTypeAll<NetInfo>().Where(x => HighwayNetworksWithoutManholes.Contains(x.name)).ToList();
                    foreach (var highwayNetwork in highwayNetworks)
                    {
                        try
                        {
                            if (highwayNetwork.m_lanes == null || highwayNetwork.m_lanes.Count() == 0)
                            {
                                continue;
                            }
                            foreach (var lane in highwayNetwork.m_lanes)
                            {
                                if (lane?.m_laneProps?.m_props == null)
                                {
                                    continue;
                                }
                                var list = new FastList<NetLaneProps.Prop>();
                                foreach (var prop in lane.m_laneProps.m_props)
                                {
                                    if (!prop.m_prop.name.ToLower().Equals("manhole"))
                                    {
                                        list.Add(prop);
                                    }
                                }
                                if (list.m_size > 0)
                                {
                                    lane.m_laneProps.m_props = list.ToArray();
                                    if (config.enable_debug)
                                    {
                                        DebugUtils.Log($"Manhole cover props successfully removed from highway type {highwayNetwork.name}.");
                                    }
                                }
                            }
                        }
                        catch
                        {
                            DebugUtils.Log($"[ERROR] - Failed to remove manhole cover props from highway type {highwayNetwork.name}.");
                        }
                    }
                }

                //  Elevated road/Bridge manhole covers:
                if (!config.enable_manholes_elevated)
                {
                    var roadNetworks = Resources.FindObjectsOfTypeAll<NetInfo>().Where(x => RoadNetworksWithoutManholes.Contains(x.name)).ToList();
                    foreach (var roadNetwork in roadNetworks)
                    {
                        try
                        {
                            if (roadNetwork.m_lanes == null || roadNetwork.m_lanes.Count() == 0)
                            {
                                continue;
                            }

                            foreach (var lane in roadNetwork.m_lanes)
                            {
                                if (lane?.m_laneProps?.m_props == null)
                                {
                                    continue;
                                }
                                var list = new FastList<NetLaneProps.Prop>();
                                foreach (var prop in lane.m_laneProps.m_props)
                                {
                                    if (!prop.m_prop.name.ToLower().Equals("manhole"))
                                    {
                                        list.Add(prop);
                                    }
                                }
                                if (list.m_size > 0)
                                {
                                    lane.m_laneProps.m_props = list.ToArray();
                                    if (config.enable_debug)
                                    {
                                        DebugUtils.Log($"Manhole cover props successfully removed from road type {roadNetwork.name}.");
                                    }
                                }
                            }
                        }
                        catch
                        {
                            DebugUtils.Log($"[ERROR] - Failed to remove manhole cover props from road type {roadNetwork.name}.");
                        }
                    }
                }
            }
        }

        //  ROAD SIGN PROPS:
        //  Handler for road sign props with custom model (replace/hide):
        public static void ChangeRoadsignPropWithCustomAsset(List<CustomizableRoadsignItem> affectedSigns)
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
            //  
            Stopwatch ReplacePropsTimer = new Stopwatch();
            ReplacePropsTimer.Start();
            //  
            foreach (var affectedSign in affectedSigns)
            {
                foreach (var pc in prop_collections)
                {
                    foreach (var prefab in pc.m_prefabs)
                    {
                        var prefabName = prefab.name.ToLower();
                        if (prefabName == affectedSign._originalSign.name.ToLower())
                        {
                            //  Show:
                            prefab.m_mesh = affectedSign._customSign.m_mesh;
                            prefab.m_material = affectedSign._customSign.m_material;
                            prefab.m_Atlas = affectedSign._customSign.m_Atlas;
                            //  
                            prefab.m_maxRenderDistance = 1000;
                            prefab.m_maxScale = 1;
                            prefab.m_minScale = 1;
                            prefab.m_lodRenderDistance = 100000;
                            prefab.m_lodMesh = null;
                            prefab.RefreshLevelOfDetail();
                            if (config.enable_debug)
                            {
                                DebugUtils.Log($"{prefab.name} replaced successfully.");
                            }
                        }
                    }
                }
            }
            //  
            ReplacePropsTimer.Stop();
            if (config.enable_debug)
            {
                DebugUtils.Log($"Road sign props replaced succesfully. Operation took {ReplacePropsTimer.ElapsedMilliseconds} ms.");
            }
        }

        //  Handler for road sign props without custom model (retexture/hide):
        public static void ChangeRoadsignPropVisibility(string affectedSign, bool isVisible, bool addAction = false)
        {
            DebugUtils.Log($"[DEBUGGER] - Change visibility to visible: {isVisible}");

            //PropInfo affectedSignProp = new PropInfo();
            List<PropInfo> affectedSignProps = new List<PropInfo>();
            if (affectedSign == "gantry")
            {
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("Motorway Overroad Signs"));
            }
            else if (affectedSign == "highway")
            {
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("Motorway Sign"));
            }
            else if (affectedSign == "noparking")
            {
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("No Parking Sign"));
            }
            else if (affectedSign == "turning")
            {
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("No Left Turn Sign"));
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("No Right Turn Sign"));
            }
            else if (affectedSign == "speedlimit")
            {
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("100 Speed Limit"));
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("60 Speed Limit"));
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("50 Speed Limit"));
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("40 Speed Limit"));
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("30 Speed Limit"));
            }
            else if (affectedSign == "streetname")
            {
                affectedSignProps.Add(PrefabCollection<PropInfo>.FindLoaded("Street Name Sign"));
            }
            //  
            string message = string.Empty;
            Stopwatch ReplacePropsTimer = new Stopwatch();

            if (addAction)
            {
                //  InGame, so add action to SimulationManager (huge performance boost):
                SimulationManager.instance.AddAction(() =>
                {
                    DebugUtils.Log($"[DEBUGGER] - addAction");
                    DebugUtils.Log($"[DEBUGGER] - number of prefabs affected: {affectedSignProps.Count}");
                    foreach (PropInfo prefab in affectedSignProps)
                    {
                        prefab.m_maxRenderDistance = (isVisible) ? 1000 : 0;
                        prefab.m_maxScale = (isVisible) ? 1 : 0;
                        prefab.m_minScale = (isVisible) ? 1 : 0;
                        message += $"Road sign '{prefab.name}' {(isVisible ? "unhidden" : "hidden")} successfully.\n";
                    }
                });
            }
            else
            {
                DebugUtils.Log($"[DEBUGGER] - not addAction");
                DebugUtils.Log($"[DEBUGGER] - number of prefabs affected: {affectedSignProps.Count}");
                //  OnLoad, do not use SimulationManager (not available at this point):
                foreach (PropInfo prefab in affectedSignProps)
                {
                    prefab.m_maxRenderDistance = (isVisible) ? 1000 : 0;
                    prefab.m_maxScale = (isVisible) ? 1 : 0;
                    prefab.m_minScale = (isVisible) ? 1 : 0;
                    message += $"Road sign '{prefab.name}' {(isVisible ? "unhidden" : "hidden")} successfully.\n";
                }
            }

            ReplacePropsTimer.Stop();
            message += $" Operation took {ReplacePropsTimer.ElapsedMilliseconds} ms.";
            if (config.enable_debug)
            {
                DebugUtils.Log(message);
            }
        }

        //  Handler for road sign props:
        public static void RetextureRoadSignProp(string prefabName, bool useCustomTexture)
        {
            PropInfo affectedProp = PrefabCollection<PropInfo>.FindLoaded(prefabName);
            var prop_collections = FindObjectsOfType<PropCollection>();
            //  Roadside props:
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    prefabName = prefab.name.ToLower();
                    //  Highway gantry:
                    if (prefabName == affectedProp.name.ToLower() && affectedProp.name.ToLower() == "motorway overroad signs")
                    {
                        //var tex = new Texture2D (1, 1);
                        //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs.png")));
                        //prefab.m_material.mainTexture = tex;
                        if (useCustomTexture)
                        {
                            prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-alt.dds")));
                            var meh = prefab.m_material.GetTexture("_MainTex");
                        }
                        else
                        {
                            prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs.dds")));
                        }
                        prefab.m_material.SetTexture("_ACIMap", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-motorway-overroad-signs-aci.dds")));
                        prefab.m_material.SetTexture("_XYSMap", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-motorway-overroad-signs-xys.dds")));
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        prefab.RefreshLevelOfDetail();
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"Motorway overhead sign props retextured to {((config.rendermode_highwaygantry_usealt) ? "alternative " : "standard ")} custom successfully.");
                        }
                    }
                    else if (prefabName == affectedProp.name.ToLower() && affectedProp.name.ToLower() == "motorway sign")
                    {
                        prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-sign-alt.dds")));
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        prefab.RefreshLevelOfDetail();
                        if (config.enable_debug)
                        {
                            DebugUtils.Log("Highway sign props retextured successfully.");
                        }
                    }
                    else if (prefabName == affectedProp.name.ToLower() && affectedProp.name.ToLower() == "street name sign")
                    {
                        prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "street-name-sign.dds")));
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        prefab.RefreshLevelOfDetail();
                        if (config.enable_debug)
                        {
                            DebugUtils.Log("Street name sign props retextured successfully.");
                        }
                    }
                }
            }
        }

        //  ROADSIDE/LANE PROPS:
        //  Show/hide roadside prop:
        public static void ChangeRoadSidePropVisibility(PropInfo affectedProp, bool isVisible)
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
            //  Roadside props:
            foreach (var pc in prop_collections)
            {
                foreach (var prop in pc.m_prefabs)
                {
                    if (prop == affectedProp)
                    {
                        if (isVisible)
                        {
                            prop.m_maxRenderDistance = 500;
                            prop.m_maxScale = 1;
                            prop.m_minScale = 1;
                        }
                        else
                        {
                            prop.m_maxRenderDistance = 0;
                            prop.m_maxScale = 0;
                            prop.m_minScale = 0;
                        }
                    }
                }
            }
        }

        //  Show/hide lane prop:
        public static void ChangeLanePropVisibility(PropInfo affectedProp, bool isVisible, string name = "")
        {
            DebugUtils.Log($"[DEBUG] - prop to be changed = {affectedProp.name} / should be visible: {isVisible}.");
            var roads = Resources.FindObjectsOfTypeAll<NetInfo>();
            foreach (var road in roads)
            {
                if (road.m_lanes == null)
                {
                    return;
                }

                //  Highways:
                if (name == "highway" && road.name.ToLower().Contains("highway"))
                {
                    foreach (var lane in road.m_lanes)
                    {
                        if (lane?.m_laneProps?.m_props == null)
                        {
                            continue;
                        }
                        foreach (var prop in lane.m_laneProps.m_props)
                        {
                            if (prop.m_prop == affectedProp)
                            {
                                DebugUtils.Log($"[DEBUG] - {affectedProp.name} should be visible: {isVisible}.");
                                if (isVisible)
                                {
                                    prop.m_prop.m_maxRenderDistance = 1000;
                                    prop.m_prop.m_maxScale = 1;
                                    prop.m_prop.m_minScale = 1;
                                }
                                else
                                {
                                    prop.m_prop.m_maxRenderDistance = 0;
                                    prop.m_prop.m_maxScale = 0;
                                    prop.m_prop.m_minScale = 0;
                                }
                            }
                        }
                    }
                }

                //  Elevated roads/bridges:
                if (name == "elevated" && road.name.ToLower().Contains("bridge") || road.name.ToLower().Contains("elevated"))
                {
                    foreach (var lane in road.m_lanes)
                    {
                        if (lane?.m_laneProps?.m_props == null)
                        {
                            continue;
                        }
                        foreach (var prop in lane.m_laneProps.m_props)
                        {
                            if (prop.m_prop == affectedProp)
                            {
                                DebugUtils.Log($"[DEBUG] - {affectedProp.name} should be visible: {isVisible}.");
                                if (isVisible)
                                {
                                    prop.m_prop.m_maxRenderDistance = 1000;
                                    prop.m_prop.m_maxScale = 1;
                                    prop.m_prop.m_minScale = 1;
                                }
                                else
                                {
                                    prop.m_prop.m_maxRenderDistance = 0;
                                    prop.m_prop.m_maxScale = 0;
                                    prop.m_prop.m_minScale = 0;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
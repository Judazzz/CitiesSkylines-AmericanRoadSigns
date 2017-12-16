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

        //  I/O:
        public static string ConfigFileName;
        public static readonly string ConfigFileNameOnline = "CSL_AmericanRoadSigns.xml";
        public static readonly string ConfigFileNameLocal = "CSL_AmericanRoadSigns_local.xml";
        public static readonly string CustomFileNameOnline = "CSL_AmericanRoadSigns_Custom.xml";
        public static readonly string CustomFileNameLocal = "CSL_AmericanRoadSigns_Custom_local.xml";

        public static string ModPath;
        public static string strModLocation;

        //  Size Constants:
        //public static float WIDTH = 270;
        //public static float HEIGHT = 350;
        //public static float SPACING = 5;
        //public static float TITLE_HEIGHT = 36;
        //public static float TABS_HEIGHT = 28;

        //  'Reference Library' Members:
        public static string[] IncludedDependencies = new string[] {
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
                strModLocation = "Workshop";
                ConfigFileName = (PluginManager.noWorkshop) ? ConfigFileNameLocal : ConfigFileNameOnline;
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
            foreach (var IncludedDependency in IncludedDependencies)
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
                    if (prop.name.ToLower().Contains("speed limit 65"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("100 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("speed limit 45"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("60 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("speed limit 30"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("50 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("speed limit 25"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("40 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("speed limit 15"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("30 Speed Limit");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("us interstate sign"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("Motorway Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("us no left turn"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("No Left Turn Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("us no right turn"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("No Right Turn Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
                    }
                    else if (prop.name.ToLower().Contains("us no parking"))
                    {
                        roadsignItem._customSign = prop;
                        roadsignItem._originalSign = PrefabCollection<PropInfo>.FindLoaded("No Parking Sign");
                        CustomizableRoadsignsList.Add(roadsignItem);
                        workshopAssets.Add(prop);
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
            CustomizableRoadsignItem affectedGantrySign = CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway overroad signs").FirstOrDefault();
            ChangeRoadsignPropWithoutCustomAsset(affectedGantrySign, config.rendermode_highwaygantry);

            List<CustomizableRoadsignItem> affectedHighwaySign = new List<CustomizableRoadsignItem>();
            affectedHighwaySign.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "motorway sign").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedHighwaySign, config.rendermode_highwaysign, false);

            List<CustomizableRoadsignItem> affectedNoparkingSign = new List<CustomizableRoadsignItem>();
            affectedNoparkingSign.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no parking sign").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedNoparkingSign, config.rendermode_noparking, false);

            List<CustomizableRoadsignItem> affectedNoturningSigns = new List<CustomizableRoadsignItem>();
            affectedNoturningSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no left turn sign").FirstOrDefault());
            affectedNoturningSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "no right turn sign").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedNoturningSigns, config.rendermode_noturnings, false);

            List<CustomizableRoadsignItem> affectedSpeedlimitSigns = new List<CustomizableRoadsignItem>();
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "30 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "40 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "50 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "60 speed limit").FirstOrDefault());
            affectedSpeedlimitSigns.Add(CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "100 speed limit").FirstOrDefault());
            ChangeRoadsignPropWithCustomAsset(affectedSpeedlimitSigns, config.rendermode_speedlimits, false);

            CustomizableRoadsignItem affectedStreetnameSign = CustomizableRoadsignsList.Where(x => x._originalSign.name.ToLower() == "street name sign").FirstOrDefault();
            ChangeRoadsignPropWithoutCustomAsset(affectedStreetnameSign, config.rendermode_streetname);
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
                    //  config.rendermode_x: 0 = American, 1 = Vanilla, 2 = Hide:

                    //  Highway gantry:
                    if (prefabName.Equals("motorway overroad signs"))
                    {
                        if (config.rendermode_highwaygantry == 0)
                        {
                            //  Custom:
                            //var tex = new Texture2D (1, 1);
                            //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs.png")));
                            //prefab.m_material.mainTexture = tex;
                            //  Global:
                            if (config.rendermode_highwaygantry_usealt)
                            {
                                prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-alt.dds")));
                            }
                            else
                            {
                                prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs.dds")));
                            }
                            //var tex2 = new Texture2D (1, 1);
                            //tex2.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs-motorway-overroad-signs-aci.png")));
                            prefab.m_material.SetTexture("_ACIMap", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-motorway-overroad-signs-aci.dds")));
                            //var tex3 = new Texture2D (1, 1);
                            //tex3.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs-motorway-overroad-signs-xys.png")));
                            prefab.m_material.SetTexture("_XYSMap", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "motorway-overroad-signs-motorway-overroad-signs-xys.dds")));
                            prefab.m_lodRenderDistance = 100000;
                            prefab.m_lodMesh = null;
                            //prefab.m_maxRenderDistance = 12000;
                            prefab.RefreshLevelOfDetail();
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Motorway overhead sign props retextured successfully.");
                            }
                        }
                        else if (config.rendermode_highwaygantry == 2)
                        {
                            prefab.m_maxRenderDistance = 0;
                            prefab.m_maxScale = 0;
                            prefab.m_minScale = 0;
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Motorway overhead sign props hidden successfully.");
                            }
                        }
                    }
                    //  Street name sign:
                    else if (prefabName.Equals("street name sign"))
                    {
                        if (config.rendermode_streetname == 0)
                        {
                            //var tex = new Texture2D (1, 1);
                            //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "street-name-sign.png")));
                            prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "street-name-sign.dds")));
                            prefab.m_lodRenderDistance = 100000;
                            prefab.m_lodMesh = null;
                            prefab.RefreshLevelOfDetail();
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Street name sign props retextured successfully.");
                            }
                        }
                        else if (config.rendermode_streetname == 2)
                        {
                            prefab.m_maxRenderDistance = 0;
                            prefab.m_maxScale = 0;
                            prefab.m_minScale = 0;
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Street name sign props hidden successfully.");
                            }
                        }
                    }
                    //  Street props:
                    else if ((prefabName.Equals("electricity box") && !config.enable_streetprops_electricitybox) ||
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
                var roads = Resources.FindObjectsOfTypeAll<NetInfo>();
                foreach (var road in roads)
                {
                    if (road.m_lanes == null)
                    {
                        return;
                    }

                    //  Highway manhole covers:
                    if (!config.enable_manholes_highway && road.name.ToLower().Contains("highway"))
                    {
                        foreach (var lane in road.m_lanes)
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
                                    DebugUtils.Log($"Manhole cover props successfully removed from highway type {road.name}.");
                                }
                            }
                        }
                    }

                    //  Elevated road/Bridge manhole covers:
                    if (!config.enable_manholes_elevated && road.name.ToLower().Contains("bridge") || road.name.ToLower().Contains("elevated"))
                    {
                        foreach (var lane in road.m_lanes)
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
                                    DebugUtils.Log($"Manhole cover props successfully removed from road type {road.name}.");
                                }
                            }
                        }
                    }
                }
            }
        }


        //  ROAD SIGN PROPS:

        //  Handler for road sign props with custom model (replace/hide):
        public static void ChangeRoadsignPropWithCustomAsset(List<CustomizableRoadsignItem> affectedSigns, int selectedValue, bool addAction = true)
        {
            DebugUtils.Log($"[DEBUG] - original prop {affectedSigns[0]._originalSign.name}, custom prop {affectedSigns[0]._customSign.name}.");

            var net_collections = FindObjectsOfType<NetCollection>();
            //  
            Stopwatch ReplacePropsTimer = new Stopwatch();
            ReplacePropsTimer.Start();
            //  Do the magic:
            if (addAction)
            {   //  InGame, so add action to SimulationManager (huge performance boost):
                SimulationManager.instance.AddAction(() =>
                {
                    if (selectedValue < 2)
                    {   //  Show vanilla/custom:
                    foreach (var nc in net_collections)
                        {
                        //  Loop Prefabs;
                        foreach (var prefab in nc.m_prefabs)
                            {
                            //  Loop lanes if present:
                            if (prefab.m_lanes != null)
                                {
                                    foreach (var lane in prefab.m_lanes)
                                    {
                                        if (lane.m_laneProps != null)
                                        {
                                            foreach (var affectedSign in affectedSigns)
                                            {
                                                ReplaceRoadsignProp(lane.m_laneProps, affectedSign, selectedValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {   //  Hide
                    foreach (var nc in net_collections)
                        {
                        //  Loop Prefabs;
                        foreach (var prefab in nc.m_prefabs)
                            {
                            //  Loop lanes if present:
                            if (prefab.m_lanes != null)
                                {
                                    foreach (var lane in prefab.m_lanes)
                                    {
                                        if (lane.m_laneProps != null)
                                        {
                                        //  Loop LaneProps:
                                        lane.m_laneProps.m_props.ForEach(
                                                x =>
                                                {
                                                    if (x.m_prop != null)
                                                    {
                                                        affectedSigns.ForEach(y =>
                                                        {
                                                            if (x.m_prop.name == y._originalSign.name || x.m_prop.name == y._customSign.name)
                                                            {
                                                                HideRoadsignProp(x);
                                                            }
                                                        });
                                                    }
                                                }
                                            );
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }
            else
            {   //  OnLoad, so do not add action to SimulationManager (not available):
                if (selectedValue < 2)
                {   //  Show vanilla/custom:
                    foreach (var nc in net_collections)
                    {
                        //  Loop Prefabs;
                        foreach (var prefab in nc.m_prefabs)
                        {
                            //  Loop lanes if present:
                            if (prefab.m_lanes != null)
                            {
                                foreach (var lane in prefab.m_lanes)
                                {
                                    if (lane.m_laneProps != null)
                                    {
                                        foreach (var affectedSign in affectedSigns)
                                        {
                                            ReplaceRoadsignProp(lane.m_laneProps, affectedSign, selectedValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {   //  Hide
                    foreach (var nc in net_collections)
                    {
                        //  Loop Prefabs;
                        foreach (var prefab in nc.m_prefabs)
                        {
                            //  Loop lanes if present:
                            if (prefab.m_lanes != null)
                            {
                                foreach (var lane in prefab.m_lanes)
                                {
                                    if (lane.m_laneProps != null)
                                    {
                                        //  Loop LaneProps:
                                        lane.m_laneProps.m_props.ForEach(
                                                x =>
                                                {
                                                    if (x.m_prop != null)
                                                    {
                                                        affectedSigns.ForEach(y =>
                                                        {
                                                            if (x.m_prop.name == y._originalSign.name || x.m_prop.name == y._customSign.name)
                                                            {
                                                                HideRoadsignProp(x);
                                                            }
                                                        });
                                                    }
                                                }
                                            );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //  
            ReplacePropsTimer.Stop();
            if (config.enable_debug)
            {
                if (selectedValue < 2)
                {
                    DebugUtils.Log($"Road sign(s) replaced with {((selectedValue == 0) ? "custom" : "vanilla")} model succesfully. Operation took {ReplacePropsTimer.ElapsedMilliseconds} ms.");
                }
                else
                {
                    DebugUtils.Log($"Road sign(s) hidden succesfully. Operation took {ReplacePropsTimer.ElapsedMilliseconds} ms.");
                }
            }
        }

        //  Handler for road sign props without custom model (retexture/hide):
        public static void ChangeRoadsignPropWithoutCustomAsset(CustomizableRoadsignItem affectedSign, int selectedValue)
        {
            DebugUtils.Log($"[DEBUG] - original prop {affectedSign._originalSign.name} to be retextured.");

            PropInfo affectedSignProp = affectedSign._originalSign;
            //  
            string message = string.Empty;
            Stopwatch ReplacePropsTimer = new Stopwatch();
            ReplacePropsTimer.Start();
            //  Do the magic:
            if (selectedValue < 2)
            {   //  Show
                RetextureRoadsignProp(affectedSignProp, selectedValue);
                message = $"Road sign '{affectedSignProp.name}' retextured to {((selectedValue == 0) ? "custom" : "vanilla")} succesfully.";
            }
            else
            {   //  Hide
                affectedSignProp.m_maxRenderDistance = 0;
                affectedSignProp.m_maxScale = 0;
                affectedSignProp.m_minScale = 0;
                message = $"Road sign '{affectedSignProp.name}' hidden successfully.";
            }
            //  
            ReplacePropsTimer.Stop();
            message += $" Operation took {ReplacePropsTimer.ElapsedMilliseconds} ms.";
            if (config.enable_debug)
            {
                DebugUtils.Log(message);
            }
        }

        //  Replace road sign prop x with road sign prop y:
        public static void ReplaceRoadsignProp(NetLaneProps affectedProps, CustomizableRoadsignItem affectedSign, int selectedValue)
        {
            //  Init:
            PropInfo oldProp = (selectedValue == 0) ? PrefabCollection<PropInfo>.FindLoaded(affectedSign._originalSign.name) : PrefabCollection<PropInfo>.FindLoaded(affectedSign._customSign.name);
            PropInfo newProp = (selectedValue == 0) ? PrefabCollection<PropInfo>.FindLoaded(affectedSign._customSign.name) : PrefabCollection<PropInfo>.FindLoaded(affectedSign._originalSign.name);
            //  Null check:
            if (!newProp)
            {
                return;
            }
            foreach (var prop in affectedProps.m_props)
            {
                if (prop.m_prop != null)
                {
                    var propInstance = prop.m_finalProp;
                    if (propInstance == null)
                    {
                        continue;
                    }
                    if (propInstance.name == oldProp.name || propInstance.name == newProp.name)
                    {
                        //  Make visible:
                        prop.m_prop.m_maxRenderDistance = 1000;
                        prop.m_prop.m_maxScale = 1;
                        prop.m_prop.m_minScale = 1;
                        prop.m_finalProp = newProp;
                        prop.m_prop = newProp;
                    }
                }
            }
        }

        //  Hide road sign prop:
        public static void HideRoadsignProp(NetLaneProps.Prop prop)
        {
            if (prop.m_prop == null)
            {
                return;
            }

            PropInfo affectedProp = prop.m_prop;
            affectedProp.m_maxRenderDistance = 0;
            affectedProp.m_maxScale = 0;
            affectedProp.m_minScale = 0;
            if (config.enable_debug)
            {
                DebugUtils.Log($"{affectedProp.name} hidden successfully.");
            }
        }

        //  Retexture road sign prop:
        public static void RetextureRoadsignProp(PropInfo affectedProp, int selectedValue)
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
            //  Roadside props:
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    var prefabName = prefab.name.ToLower();
                    //  Highway gantry:
                    if (prefabName == affectedProp.name.ToLower() && affectedProp.name.ToLower() == "motorway overroad signs")
                    {
                        //  Show
                        prefab.m_maxRenderDistance = 1000;
                        prefab.m_maxScale = 1;
                        prefab.m_minScale = 1;
                        if (selectedValue == 0)
                        {   //  Set to custom
                            //var tex = new Texture2D (1, 1);
                            //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs.png")));
                            //prefab.m_material.mainTexture = tex;
                            //  Global:
                            if (config.rendermode_highwaygantry_usealt)
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
                            if (config.enable_debug)
                            {
                                DebugUtils.Log($"Motorway overhead sign props retextured to {((config.rendermode_highwaygantry_usealt) ? "alternative " : "standard ")} custom successfully.");
                            }
                        }
                        else
                        {   //  Reset to vanilla
                            prefab.m_material.SetTexture("_MainTex", gantryMainTexture);
                            prefab.m_material.SetTexture("_ACIMap", gantryACITexture);
                            prefab.m_material.SetTexture("_XYSMap", gantryXYSTexture);
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Motorway overhead sign props retextured to vanilla successfully.");
                            }
                        }
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        prefab.RefreshLevelOfDetail();
                    }
                    //  Street name sign:
                    else if (prefabName == affectedProp.name.ToLower() && affectedProp.name.ToLower() == "street name sign")
                    {
                        //  Show
                        prefab.m_maxRenderDistance = 1000;
                        prefab.m_maxScale = 1;
                        prefab.m_minScale = 1;
                        if (selectedValue == 0)
                        {   //  Set to custom
                            prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(ModPath, "street-name-sign.dds")));
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Street name sign props retextured to custom successfully.");
                            }
                        }
                        else
                        {   //  Reset to vanilla
                            prefab.m_material.SetTexture("_MainTex", streetnameMainTexture);
                            if (config.enable_debug)
                            {
                                DebugUtils.Log("Street name sign props retextured to vanilla successfully.");
                            }
                        }
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        prefab.RefreshLevelOfDetail();
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
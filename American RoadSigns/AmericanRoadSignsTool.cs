using AmericanRoadSigns.GUI;
using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

        public static string ConfigFileName;
        public static readonly string ConfigFileNameOnline = "CSL_AmericanRoadSigns.xml";
        public static readonly string ConfigFileNameLocal = "CSL_AmericanRoadSigns_local.xml";
        public static readonly string CustomFileNameOnline = "CSL_AmericanRoadSigns_Custom.xml";
        public static readonly string CustomFileNameLocal = "CSL_AmericanRoadSigns_Custom_local.xml";

        public static string ModPath;
        public static string strModLocation;

        static PropInfo sl15 = new PropInfo();
        static PropInfo sl25 = new PropInfo();
        static PropInfo sl30 = new PropInfo();
        static PropInfo sl45 = new PropInfo();
        static PropInfo sl65 = new PropInfo();

        static PropInfo left_turn = new PropInfo();
        static PropInfo right_turn = new PropInfo();
        static PropInfo motorwaysign = new PropInfo();
        static PropInfo parkingsign = new PropInfo();

        static int slpropsfound = 0;
        static int turnsignpropsfound = 0;
        static bool motorwaypropfound = false;
        static bool parkingsignpropfound = false;

        //  Size Constants:
        public static float WIDTH = 270;
        public static float HEIGHT = 350;
        public static float SPACING = 5;
        public static float TITLE_HEIGHT = 36;
        public static float TABS_HEIGHT = 28;

        public static string[] IncludedAssets = new string[] {
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

        public static List<PropInfo> vanillaAssets = new List<PropInfo>();
        public static List<PropInfo> workshopAssets = new List<PropInfo>();

        //  Methods:
        public static void Reset()
        {
            var go = FindObjectOfType<AmericanRoadsignsTool>();
            if (go != null)
            {
                Destroy(go);
            }

            config = null; // do??
            isGameLoaded = false;
            slpropsfound = 0;
            turnsignpropsfound = 0;
            motorwaypropfound = false;
            parkingsignpropfound = false;
        }

        public static void Initialize()
        {
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

        //  Config
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

        //  Assets-related:
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
            foreach (var dependency in IncludedAssets)
            {
                if (File.Exists(path + "\\" + dependency))
                {
                    if (config.enable_debug)
                    {
                        DebugUtils.Log($"Custom '{dependency}' found in {strModLocation} folder '{path}'.");
                    }
                }
                else
                {
                    if (config.enable_debug)
                    {
                        DebugUtils.Log($"Custom '{dependency}' NOT found in {strModLocation} folder '{path}'.");
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
            string propNames = string.Empty;
            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                PropInfo prop = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                //  Custom props:
                if (prop.m_isCustomContent)
                {
                    if (prop.name.ToLower().Contains("speed limit 65"))
                    {
                        sl65 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        slpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("speed limit 45"))
                    {
                        sl45 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        slpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("speed limit 30"))
                    {
                        sl30 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        slpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("speed limit 25"))
                    {
                        sl25 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        slpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("speed limit 15"))
                    {
                        sl15 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        slpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("us interstate sign"))
                    {
                        motorwaysign = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        motorwaypropfound = true;
                    }
                    else if (prop.name.ToLower().Contains("us no left turn"))
                    {
                        left_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        turnsignpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("us no right turn"))
                    {
                        right_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        turnsignpropsfound++;
                    }
                    else if (prop.name.ToLower().Contains("us no parking"))
                    {
                        parkingsign = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        workshopAssets.Add(prop);
                        parkingsignpropfound = true;
                    }
                }
                //  Vanilla props:
                else
                {
                    if (// Road signs:
                        prop.name.ToLower().Equals("motorway overroad signs") ||
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
            DebugUtils.Log($"[DEBUG] - Number of vanilla props added to list: {vanillaAssets.Count()} (expect 16).");
            DebugUtils.Log($"[DEBUG] - Number of workshop props added to list: {workshopAssets.Count()} (expect 9).");
        }


        //  OnLoad:
        #region OnLoad:

        public static void ReplacePropsOnLoad()
        {
            var net_collections = FindObjectsOfType<NetCollection>();
            //  Loop NetCollections:
            Stopwatch ReplacePropsTimer = new Stopwatch();
            ReplacePropsTimer.Start();
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
                                lane.m_laneProps.m_props.ForEach(x => ReplaceProp(x));
                            }
                        }
                    }
                }
            }
            ReplacePropsTimer.Stop();
            DebugUtils.Log($"Replaced all props successfully (time elapsed: {ReplacePropsTimer.Elapsed} seconds).");
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
                                else { DebugUtils.Log($"[DEBUG] - {prop.m_prop.name} (highway): m_maxRenderDistance = {prop.m_prop.m_maxRenderDistance}, m_maxScale = {prop.m_prop.m_maxScale}, m_minScale = {prop.m_prop.m_minScale}.");
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
                                else
                                {
                                    DebugUtils.Log($"[DEBUG] - {prop.m_prop.name} (elevated): m_maxRenderDistance = {prop.m_prop.m_maxRenderDistance}, m_maxScale = {prop.m_prop.m_maxScale}, m_minScale = {prop.m_prop.m_minScale}.");
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
        
        public static void ReplaceProp(NetLaneProps.Prop prop)
        {
            if (prop.m_prop == null)
            {
                return;
            }

            PropInfo oldProp = prop.m_prop;
            PropInfo oldProp2 = prop.m_finalProp;
            PropInfo newProp = new PropInfo();
            string propName = prop.m_prop.name.ToLower();

            bool changeprop = false;
            bool hideprop = false;
            //  
            if (propName.EndsWith("speed limit") && slpropsfound >= 5)
            {
                if (config.rendermode_speedlimits == 0)
                {
                    if (propName.Contains("30"))
                    {
                        newProp = sl15;
                        changeprop = true;
                    }
                    else if (propName.Contains("40"))
                    {
                        newProp = sl25;
                        changeprop = true;
                    }
                    else if (propName.Contains("50"))
                    {
                        newProp = sl30;
                        changeprop = true;
                    }
                    else if (propName.Contains("60"))
                    {
                        newProp = sl45;
                        changeprop = true;
                    }
                    else if (propName.Contains("100"))
                    {
                        newProp = sl65;
                        changeprop = true;
                    }
                }
                else if (config.rendermode_speedlimits == 2)
                {
                    hideprop = true;
                }
            }
            else if (propName.Contains("motorway sign") && motorwaypropfound)
            {
                if (config.rendermode_highwaysign == 0)
                {
                    newProp = motorwaysign;
                    changeprop = true;
                }
                else if (config.rendermode_highwaysign == 2)
                {
                    hideprop = true;
                }
            }
            else if (turnsignpropsfound >= 2 && propName.Contains("no right turn") || propName.Contains("no left turn"))
            {
                if (config.rendermode_noturnings == 0)
                {
                    if (propName.Contains("no left turn sign"))
                    {
                        newProp = left_turn;
                        changeprop = true;
                    }
                    else if (propName.Contains("no right turn sign"))
                    {
                        newProp = right_turn;
                        changeprop = true;
                    }
                }
                else if (config.rendermode_noturnings == 2)
                {
                    hideprop = true;
                }
            }
            else if (propName.Contains("no parking sign") && parkingsignpropfound)
            {
                if (config.rendermode_noparking == 0)
                {
                    newProp = parkingsign;
                    changeprop = true;
                }
                else if (config.rendermode_noparking == 2)
                {
                    hideprop = true;
                }
            }

            if (changeprop)
            {
                oldProp.m_mesh = newProp.m_mesh;
                oldProp2.m_mesh = newProp.m_mesh;

                oldProp.m_material = newProp.m_material;
                oldProp2.m_material = newProp.m_material;

                oldProp.m_lodMesh = newProp.m_lodMesh;
                oldProp2.m_lodMesh = newProp.m_lodMesh;

                oldProp.m_lodMeshCombined1 = newProp.m_lodMeshCombined1;
                oldProp2.m_lodMeshCombined1 = newProp.m_lodMeshCombined1;

                oldProp.m_lodMeshCombined4 = newProp.m_lodMeshCombined4;
                oldProp2.m_lodMeshCombined4 = newProp.m_lodMeshCombined4;

                oldProp.m_lodMeshCombined8 = newProp.m_lodMeshCombined8;
                oldProp2.m_lodMeshCombined8 = newProp.m_lodMeshCombined8;

                oldProp.m_lodMeshCombined16 = newProp.m_lodMeshCombined16;
                oldProp2.m_lodMeshCombined16 = newProp.m_lodMeshCombined16;

                oldProp.m_lodMaterialCombined = newProp.m_lodMaterialCombined;
                oldProp2.m_lodMaterialCombined = newProp.m_lodMaterialCombined;

                oldProp.m_lodLocations = newProp.m_lodLocations;
                oldProp2.m_lodLocations = newProp.m_lodLocations;

                oldProp.m_lodObjectIndices = newProp.m_lodObjectIndices;
                oldProp2.m_lodObjectIndices = newProp.m_lodObjectIndices;

                oldProp.m_lodColors = newProp.m_lodColors;
                oldProp2.m_lodColors = newProp.m_lodColors;

                oldProp.m_lodCount = newProp.m_lodCount;
                oldProp2.m_lodCount = newProp.m_lodCount;

                oldProp.m_lodMin = newProp.m_lodMin;
                oldProp2.m_lodMin = newProp.m_lodMin;

                oldProp.m_lodMax = newProp.m_lodMax;
                oldProp2.m_lodMax = newProp.m_lodMax;

                oldProp.m_lodHeightMap = newProp.m_lodHeightMap;
                oldProp2.m_lodHeightMap = newProp.m_lodHeightMap;

                oldProp.m_lodHeightMapping = newProp.m_lodHeightMapping;
                oldProp2.m_lodHeightMapping = newProp.m_lodHeightMapping;

                oldProp.m_lodSurfaceMapping = newProp.m_lodSurfaceMapping;
                oldProp2.m_lodSurfaceMapping = newProp.m_lodSurfaceMapping;

                oldProp.m_lodRenderDistance = 2000;
                oldProp2.m_lodRenderDistance = 2000;
                //  
                if (config.enable_debug)
                {
                    DebugUtils.Log($"{propName} replacement successful.");
                }
            }
            else if (hideprop)
            {
                oldProp.m_maxRenderDistance = 0;
                oldProp.m_maxScale = 0;
                oldProp.m_minScale = 0;
                if (config.enable_debug)
                {
                    DebugUtils.Log($"{propName} hidden successfully.");
                }
            }
        }

        #endregion


        //  InGame:
        #region InGame:

        //  Americanize/Vanilla-ize roadside prop texture:
        public static void RetexturePropInGame(PropInfo prop, int action)
        {

        }

        //  Americanize/Vanilla-ize/hide roadside prop (0 = American, 1 = Vanilla, 2 = Hide):
        public static void ReplaceRoadsignInGame(PropInfo oldProp, PropInfo newProp, int action)
        {

        }

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

        #endregion
    }
}

/*
    Motorway Overroad Signs: m_maxRenderDistance = 1000, m_maxScale = 1, m_minScale = 1
    Street Name Sign: m_maxRenderDistance = 257.3326, m_maxScale = 1, m_minScale = 1
    
    Manhole: m_maxRenderDistance = 1000, m_maxScale = 1, m_minScale = 1

    Road signs: m_maxRenderDistance = 351.0643, m_maxScale = 1, m_minScale = 1

    Electricity Box: m_maxRenderDistance = 454.9089, m_maxScale = 1, m_minScale = 1
    Fire Hydrant: m_maxRenderDistance = 298.6388, m_maxScale = 1, m_minScale = 1
    Info Terminal: m_maxRenderDistance = 435.7743, m_maxScale = 1, m_minScale = 1
    Parking Meter: m_maxRenderDistance = 439.9601, m_maxScale = 1, m_minScale = 1
    Random Street Prop: m_maxRenderDistance = 454.9089, m_maxScale = 1, m_minScale = 1
*/
using ColossalFramework.IO;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
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
        public const string version = "1.1.2";

        public string Name
        {
            get { return "American RoadSigns " + version; }
        }

        public string Description
        {
            get { return "Americanizes Road and Highway Signs."; }
        }

        //  Select Options:
        private void OnHighwayGantryChanged(int c)
        {
            AmericanRoadSigns.config.rendermode_highwaygantry = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnHighwaySignChanged(int c)
        {
            AmericanRoadSigns.config.rendermode_highwaysign = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnNoParkingChanged(int c)
        {
            AmericanRoadSigns.config.rendermode_noparking = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnNoTurningChanged(int c)
        {
            AmericanRoadSigns.config.rendermode_noturnings = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnSpeedLimitChanged(int c)
        {
            AmericanRoadSigns.config.rendermode_speedlimits = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnStreetNameChanged(int c)
        {
            AmericanRoadSigns.config.rendermode_streetname = c;
            AmericanRoadSigns.SaveConfig();
        }
        //  Toggle Options:
        private void OnAltHighwayGantryChanged(bool c)
        {
            AmericanRoadSigns.config.rendermode_highwaygantry_usealt = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleManholesHighwayChanged(bool c)
        {
            AmericanRoadSigns.config.enable_manholes_highway = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleManholesElevatedChanged(bool c)
        {
            AmericanRoadSigns.config.enable_manholes_elevated = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleStreetPropElectricityboxChanged(bool c)
        {
            AmericanRoadSigns.config.enable_streetprops_electricitybox = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleStreetPropFirehydrantChanged(bool c)
        {
            AmericanRoadSigns.config.enable_streetprops_firehydrant = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleStreetPropInfoterminalChanged(bool c)
        {
            AmericanRoadSigns.config.enable_streetprops_infoterminal = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleStreetPropParkingmeterChanged(bool c)
        {
            AmericanRoadSigns.config.enable_streetprops_parkingmeter = c;
            AmericanRoadSigns.SaveConfig();
        }
        private void OnToggleStreetPropRandomChanged(bool c)
        {
            AmericanRoadSigns.config.enable_streetprops_random = c;
            AmericanRoadSigns.SaveConfig();
        }

        private void OnEnableDebugChanged(bool c)
        {
            AmericanRoadSigns.config.enable_debug = c;
            AmericanRoadSigns.SaveConfig();
        }

        private void OnEnableLocalAssetsChanged(bool c)
        {
            AmericanRoadSigns.config.enable_localassets = c;
            AmericanRoadSigns.SaveConfig();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            var activeConfigPath = (PluginManager.noWorkshop) ? AmericanRoadSigns.configPathLocal : AmericanRoadSigns.configPath;
            AmericanRoadSigns.config = Configuration.Deserialize(activeConfigPath);
            //  
            bool flag = AmericanRoadSigns.config == null;
            if (flag)
            {
                AmericanRoadSigns.config = new Configuration();
            }
            AmericanRoadSigns.SaveConfig();

            //  Mod options:
            UIHelperBase group = helper.AddGroup(Name);
            group.AddSpace(10);
            //  Select Options (Road signs):
            group.AddDropdown("Highway overhead gantries", new[] { "American", "Vanilla", "Hide" }, AmericanRoadSigns.config.rendermode_highwaygantry, OnHighwayGantryChanged);
            group.AddCheckbox("Use alternative American highway overhead gantry texture (WIP).", AmericanRoadSigns.config.rendermode_highwaygantry_usealt, new OnCheckChanged(OnAltHighwayGantryChanged));
            group.AddSpace(10);
            group.AddDropdown("Highway route signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadSigns.config.rendermode_highwaysign, OnHighwaySignChanged);
            group.AddDropdown("'No parking' signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadSigns.config.rendermode_noparking, OnNoParkingChanged);
            group.AddDropdown("No left/right turn' signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadSigns.config.rendermode_noturnings, OnNoTurningChanged);
            group.AddDropdown("Speed limit signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadSigns.config.rendermode_speedlimits, OnSpeedLimitChanged);
            group.AddDropdown("Streetname signs", new[] { "American", "Vanilla", "Hide" }, AmericanRoadSigns.config.rendermode_streetname, OnStreetNameChanged);
            group.AddSpace(10);
            //  Toggle Options (Street props):
            group.AddCheckbox("Show manhole covers on highways", AmericanRoadSigns.config.enable_manholes_highway, new OnCheckChanged(OnToggleManholesHighwayChanged));
            group.AddCheckbox("Show manhole covers on elevated roads and bridges", AmericanRoadSigns.config.enable_manholes_elevated, new OnCheckChanged(OnToggleManholesElevatedChanged));
            group.AddSpace(10);
            group.AddCheckbox("Show road-side electricity box props", AmericanRoadSigns.config.enable_streetprops_electricitybox, new OnCheckChanged(OnToggleStreetPropElectricityboxChanged));
            group.AddCheckbox("Show road-side fire hydrant props", AmericanRoadSigns.config.enable_streetprops_firehydrant, new OnCheckChanged(OnToggleStreetPropFirehydrantChanged));
            group.AddCheckbox("Show road-side info terminal props", AmericanRoadSigns.config.enable_streetprops_infoterminal, new OnCheckChanged(OnToggleStreetPropInfoterminalChanged));
            group.AddCheckbox("Show road-side parking meter props", AmericanRoadSigns.config.enable_streetprops_parkingmeter, new OnCheckChanged(OnToggleStreetPropParkingmeterChanged));
            group.AddCheckbox("Show road-side random street props", AmericanRoadSigns.config.enable_streetprops_random, new OnCheckChanged(OnToggleStreetPropRandomChanged));
            group.AddSpace(10);
            //  Toggle Options (Misc.):
            group.AddCheckbox("Load dependencies from local mod folder, if present", AmericanRoadSigns.config.enable_localassets, new OnCheckChanged(OnEnableLocalAssetsChanged));
            group.AddCheckbox("Write additional data to debug log", AmericanRoadSigns.config.enable_debug, new OnCheckChanged(OnEnableDebugChanged));
            group.AddSpace(5);
            group.AddGroup("WARNING: enabling debug data may increase loading times considerably!\nEnable this setting is only recommended when you experience problems.");
            group.AddSpace(20);
        }
    }

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
    }

    public class ModLoader : LoadingExtensionBase
    {

        public static string[] _dependencies = new string[] {
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

        public string getModPath()
        {
            Workshop workshopInstance = new Workshop();
            string workshopPath = ".";
            //  Use local assets:
            if (AmericanRoadSigns.config.enable_localassets)
            {
                string localPath = DataLocation.modsPath + "\\American RoadSigns";
                //DebugUtils.Log($"Mod path: {localPath}.");
                if (Directory.Exists(localPath))
                {
                    if (DependenciesPresent(true, localPath))
                    {
                        DebugUtils.Log($"Load included props and textures from local mod folder ({localPath}).");
                        return localPath;
                    }
                    else
                    {
                        DebugUtils.Log("Not all included props and textures found in local mod folder: retrying to load from Workshop folder.");
                    }
                }
            }
            //  Use included assets:
            //PrefabCollection<PropInfo>.FindLoaded("694123443.AmericanTrafficLightMain_Data");
            foreach (PublishedFileId mod in workshopInstance.GetSubscribedItems())
            {
                if (mod.AsUInt64 == Mod.workshop_id)
                {
                    workshopPath = workshopInstance.GetSubscribedItemPath(mod);
                    DebugUtils.Log($"Mod path: {workshopPath}.");
                    DependenciesPresent(false, workshopPath);
                    break;
                }
            }
            DebugUtils.Log($"Load included props and textures from Workshop folder ({workshopPath}).");
            return workshopPath;
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            // Disable mod for all but in-game:
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                return;
            }
            //  
            var activeConfigPath = (PluginManager.noWorkshop) ? AmericanRoadSigns.configPathLocal : AmericanRoadSigns.configPath;
            AmericanRoadSigns.config = Configuration.Deserialize(activeConfigPath);
            if (AmericanRoadSigns.config == null)
            {
                AmericanRoadSigns.config = new Configuration();
            }
            AmericanRoadSigns.SaveConfig();
            //  
            string path = getModPath();
            AmericanRoadSigns.FindProps();
            AmericanRoadSigns.ReplaceProps();
            AmericanRoadSigns.ChangeProps(path);
            //  
            base.OnLevelLoaded(mode);
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
        }

        public static bool DependenciesPresent(bool isLocal, string path)
        {
            bool dependenciesPresent = true;
            string fileLocation = (isLocal) ? "Local" : "Workshop";
            foreach (var dependency in _dependencies)
            {
                if (File.Exists(path + "\\" + dependency))
                {
                    DebugUtils.Log($"Custom '{dependency}' found in {fileLocation} folder '{path}'.");
                }
                else
                {
                    DebugUtils.Log($"Custom '{dependency}' NOT found in {fileLocation} folder '{path}'.");
                    dependenciesPresent = false;
                }
            }
            //  
            if (dependenciesPresent)
            {
                DebugUtils.Log($"All included props and textures found in {fileLocation} folder.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class AmericanRoadSigns : MonoBehaviour
    {
        public static Configuration config;
        public static readonly string configPath = "CSL_AmericanRoadSigns.xml";
        public static readonly string configPathLocal = "CSL_AmericanRoadSigns_local.xml";

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

        public static void SaveConfig()
        {
            var activeConfigPath = (PluginManager.noWorkshop) ? configPathLocal : configPath;
            Configuration.Serialize(activeConfigPath, config);
        }

        public static Texture2D LoadTextureDDS(string texturePath)
        {
            var ddsBytes = File.ReadAllBytes(texturePath);
            var height = BitConverter.ToInt32(ddsBytes, 12);
            var width = BitConverter.ToInt32(ddsBytes, 16);
            var texture = new Texture2D(width, height, TextureFormat.DXT5, true);
            List<byte> byteList = new List<byte>();

            for (int i = 0; i < ddsBytes.Length; i++)
            {
                if (i > 127)
                {
                    byteList.Add(ddsBytes[i]);
                }
            }
            texture.LoadRawTextureData(byteList.ToArray());

            texture.Apply();
            texture.anisoLevel = 8;
            return texture;
        }

        public static Texture2D LoadTexture(string texturePath)
        {
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(File.ReadAllBytes(texturePath));
            texture2D.anisoLevel = 8;
            return texture2D;
        }

        public static void FindProps()
        {
            //  No f'n clue what causes this weird behavior, but code below causes error on European maps, so return if loaded map is European:
            if (LoadingManager.instance.m_loadedEnvironment.ToLower() == "europe")
            {
                return;
            }
            //  
            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                string propName = PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower();
                if (propName.Contains("speed limit 65"))
                {
                    sl65 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                }
                else if (propName.Contains("speed limit 45"))
                {
                    sl45 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                }
                else if (propName.Contains("speed limit 30"))
                {
                    sl30 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                }
                else if (propName.Contains("speed limit 25"))
                {
                    sl25 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                }
                else if (propName.Contains("speed limit 15"))
                {
                    sl15 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                }

                else if (propName.Contains("us interstate sign"))
                {
                    motorwaysign = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    motorwaypropfound = true;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom Interstate sign found.");
                    }
                }

                else if (propName.Contains("us no left turn"))
                {
                    left_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    turnsignpropsfound++;
                }
                else if (propName.Contains("us no right turn"))
                {
                    right_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    turnsignpropsfound++;
                }

                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("us no parking"))
                {
                    parkingsign = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    parkingsignpropfound = true;
                }
            }
        }

        public static void ReplaceProps()
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
                                for (int i = 0; i < lane.m_laneProps.m_props.Length; i++)
                                {
                                    if (lane.m_laneProps.m_props[i].m_prop != null)
                                    {
                                        var pr = lane.m_laneProps.m_props[i].m_prop;
                                        var pr2 = lane.m_laneProps.m_props[i].m_finalProp;
                                        PropInfo testprop = new PropInfo();

                                        bool changeprop = false;
                                        bool hideprop = false;
                                        string propName = pr.name.ToLower();

                                        //if (propName.Contains("andom street pro") && !config.enable_streetprops)
                                        //{
                                        //    hideprop = true;
                                        //}
                                        //  config.rendermode_x: 0 = American, 1 = Vanilla, 2 = Hide:
                                        //else if (propName.Contains("speed limit") && slpropsfound >= 5)
                                        if (propName.Contains("speed limit") && slpropsfound >= 5)
                                        {
                                            if (config.rendermode_speedlimits == 0)
                                            {
                                                if (propName.Contains("30"))
                                                {
                                                    testprop = sl15;
                                                    changeprop = true;
                                                }
                                                else if (propName.Contains("40"))
                                                {
                                                    testprop = sl25;
                                                    changeprop = true;
                                                }
                                                else if (propName.Contains("50"))
                                                {
                                                    testprop = sl30;
                                                    changeprop = true;
                                                }
                                                else if (propName.Contains("60"))
                                                {
                                                    testprop = sl45;
                                                    changeprop = true;
                                                }
                                                else if (propName.Contains("100"))
                                                {
                                                    testprop = sl65;
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
                                                testprop = motorwaysign;
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
                                                    testprop = left_turn;
                                                    changeprop = true;
                                                }
                                                else if (propName.Contains("no right turn sign"))
                                                {
                                                    testprop = right_turn;
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
                                                testprop = parkingsign;
                                                changeprop = true;
                                            }
                                            else if (config.rendermode_noparking == 2)
                                            {
                                                hideprop = true;
                                            }
                                        }

                                        if (changeprop)
                                        {
                                            pr.m_mesh = testprop.m_mesh;
                                            pr2.m_mesh = testprop.m_mesh;

                                            pr.m_material = testprop.m_material;
                                            pr2.m_material = testprop.m_material;

                                            pr.m_lodMesh = testprop.m_lodMesh;
                                            pr2.m_lodMesh = testprop.m_lodMesh;

                                            pr.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;
                                            pr2.m_lodMeshCombined1 = testprop.m_lodMeshCombined1;

                                            pr.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;
                                            pr2.m_lodMeshCombined4 = testprop.m_lodMeshCombined4;

                                            pr.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;
                                            pr2.m_lodMeshCombined8 = testprop.m_lodMeshCombined8;

                                            pr.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;
                                            pr2.m_lodMeshCombined16 = testprop.m_lodMeshCombined16;

                                            pr.m_lodMaterialCombined = testprop.m_lodMaterialCombined;
                                            pr2.m_lodMaterialCombined = testprop.m_lodMaterialCombined;

                                            pr.m_lodLocations = testprop.m_lodLocations;
                                            pr2.m_lodLocations = testprop.m_lodLocations;

                                            pr.m_lodObjectIndices = testprop.m_lodObjectIndices;
                                            pr2.m_lodObjectIndices = testprop.m_lodObjectIndices;

                                            pr.m_lodColors = testprop.m_lodColors;
                                            pr2.m_lodColors = testprop.m_lodColors;

                                            pr.m_lodCount = testprop.m_lodCount;
                                            pr2.m_lodCount = testprop.m_lodCount;

                                            pr.m_lodMin = testprop.m_lodMin;
                                            pr2.m_lodMin = testprop.m_lodMin;

                                            pr.m_lodMax = testprop.m_lodMax;
                                            pr2.m_lodMax = testprop.m_lodMax;

                                            pr.m_lodHeightMap = testprop.m_lodHeightMap;
                                            pr2.m_lodHeightMap = testprop.m_lodHeightMap;

                                            pr.m_lodHeightMapping = testprop.m_lodHeightMapping;
                                            pr2.m_lodHeightMapping = testprop.m_lodHeightMapping;

                                            pr.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;
                                            pr2.m_lodSurfaceMapping = testprop.m_lodSurfaceMapping;

                                            pr.m_lodRenderDistance = 2000;
                                            pr2.m_lodRenderDistance = 2000;
                                            //  
                                            if (config.enable_debug)
                                            {
                                                DebugUtils.Log($"[DEBUG] - {propName} replacement successful.");
                                            }
                                        }
                                        else if (hideprop)
                                        {
                                            pr.m_maxRenderDistance = 0;
                                            pr.m_maxScale = 0;
                                            pr.m_minScale = 0;
                                            if (config.enable_debug)
                                            {
                                                DebugUtils.Log($"[DEBUG] - {propName} hidden successfully.");
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            ReplacePropsTimer.Stop();
            DebugUtils.Log($"Replaced all props successfully (time elapsed: {ReplacePropsTimer.Elapsed} seconds).");
        }

        public static void ChangeProps(string textureDir)
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
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
                                prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs-alt.dds")));
                            }
                            else
                            {
                                prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs.dds")));
                            }
                            //var tex2 = new Texture2D (1, 1);
                            //tex2.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs-motorway-overroad-signs-aci.png")));
                            prefab.m_material.SetTexture("_ACIMap", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs-motorway-overroad-signs-aci.dds")));
                            //var tex3 = new Texture2D (1, 1);
                            //tex3.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs-motorway-overroad-signs-xys.png")));
                            prefab.m_material.SetTexture("_XYSMap", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs-motorway-overroad-signs-xys.dds")));
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
                            prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "street-name-sign.dds")));
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
                    //  Street prop - electricity box:
                    else if (prefabName.Equals("electricity box") && !config.enable_streetprops_electricitybox)
                    {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"{prefab.name} props hidden successfully.");
                        }
                    }
                    //  Street prop - fire hydrant:
                    else if (prefabName.Equals("fire hydrant") && !config.enable_streetprops_firehydrant)
                    {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"{prefab.name} props hidden successfully.");
                        }
                    }
                    //  Street prop - info terminal:
                    else if (prefabName.Equals("info terminal") && !config.enable_streetprops_infoterminal)
                    {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"{prefab.name} props hidden successfully.");
                        }
                    }
                    //  Street prop - electricity box:
                    else if (prefabName.Equals("parking meter") && !config.enable_streetprops_parkingmeter)
                    {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"{prefab.name} props hidden successfully.");
                        }
                    }
                    //  Street prop - electricity box:
                    else if (prefabName.Equals("random street prop") && !config.enable_streetprops_random)
                    {
                        prefab.m_maxRenderDistance = 0;
                        prefab.m_maxScale = 0;
                        prefab.m_minScale = 0;
                        if (config.enable_debug)
                        {
                            DebugUtils.Log($"{prefab.name}s hidden successfully.");
                        }
                    }
                }
            }

            //  Props:
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
    }
}
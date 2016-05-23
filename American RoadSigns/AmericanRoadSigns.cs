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
            AmericanRoadSigns.config.enable_debug = c;
            AmericanRoadSigns.SaveConfig();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            AmericanRoadSigns.config = Configuration.Deserialize(AmericanRoadSigns.configPath);
            bool flag = AmericanRoadSigns.config == null;
            if (flag)
            {
                AmericanRoadSigns.config = new Configuration();
            }
            AmericanRoadSigns.SaveConfig();


            UIHelperBase group = helper.AddGroup(Name);
            group.AddSpace(10);
            group.AddCheckbox("Write data to debug log", AmericanRoadSigns.config.enable_debug, new OnCheckChanged(EventEnableDebug));
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
            AmericanRoadSigns.config = Configuration.Deserialize(AmericanRoadSigns.configPath);
            if (AmericanRoadSigns.config == null)
            {
                AmericanRoadSigns.config = new Configuration();
            }
            AmericanRoadSigns.SaveConfig();

            string path = getModPath();

            AmericanRoadSigns.FindProps();
            AmericanRoadSigns.ReplaceProps();
            AmericanRoadSigns.ChangeProps(path);

            base.OnLevelLoaded(mode);
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
        }
    }

    public class AmericanRoadSigns : MonoBehaviour
    {
        public static Configuration config;
        public static readonly string configPath = "CSL_AmericanRoadSigns.xml";

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
            Configuration.Serialize(configPath, config);
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

        public static void FindProps()
        {
            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                string propName = PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower();
                if (propName.Contains("speed limit 65"))
                {
                    sl65 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'Speed limit 65' found.");
                    }
                }
                else if (propName.Contains("speed limit 45"))
                {
                    sl45 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'Speed limit 45' found.");
                    }
                }
                else if (propName.Contains("speed limit 30"))
                {
                    sl30 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'Speed limit 30' found.");
                    }
                }
                else if (propName.Contains("speed limit 25"))
                {
                    sl25 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'Speed limit 25' found.");
                    }
                }
                else if (propName.Contains("speed limit 15"))
                {
                    sl15 = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    slpropsfound++;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'Speed limit 15' found.");
                    }
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
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'No left turn' sign found.");
                    }
                }
                else if (propName.Contains("us no right turn"))
                {
                    right_turn = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    turnsignpropsfound++;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'No right turn' sign found.");
                    }
                }

                else if (PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower().Contains("us no parking"))
                {
                    parkingsign = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                    parkingsignpropfound = true;
                    if (config.enable_debug)
                    {
                        DebugUtils.Log("Custom 'No parking' sign found.");
                    }
                }
            }
            if (slpropsfound >= 5 && motorwaypropfound && turnsignpropsfound >= 2 && parkingsignpropfound)
            {
                DebugUtils.Log("All custom props found.");
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

                                        string propName = pr.name.ToLower();

                                        if (propName.Contains("speed limit") && slpropsfound >= 5)
                                        {

                                            if (propName.Contains("30"))
                                            {
                                                testprop = sl15;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'Speed limit 30' sign found.");
                                                }
                                            }
                                            else if (propName.Contains("40"))
                                            {
                                                testprop = sl25;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'Speed limit 40' sign found.");
                                                }
                                            }
                                            else if (propName.Contains("50"))
                                            {
                                                testprop = sl30;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'Speed limit 50' sign found.");
                                                }
                                            }
                                            else if (propName.Contains("60"))
                                            {
                                                testprop = sl45;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'Speed limit 60' sign found.");
                                                }
                                            }
                                            else if (propName.Contains("100"))
                                            {
                                                testprop = sl65;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'Speed limit 100' sign found.");
                                                }
                                            }
                                        }
                                        else if (propName.Contains("motorway sign") && motorwaypropfound)
                                        {
                                            testprop = motorwaysign;
                                            changeprop = true;
                                        }
                                        else if (turnsignpropsfound >= 2 && propName.Contains("no right turn") || propName.Contains("no left turn"))
                                        {
                                            if (propName.Contains("no left turn sign"))
                                            {
                                                testprop = left_turn;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'No left turn' sign found.");
                                                }
                                            }
                                            else if (propName.Contains("no right turn sign"))
                                            {
                                                testprop = right_turn;
                                                changeprop = true;
                                                if (config.enable_debug)
                                                {
                                                    DebugUtils.Log("Vanilla 'No right turn' sign found.");
                                                }
                                            }
                                        }
                                        else if (propName.Contains("no parking sign") && parkingsignpropfound)
                                        {
                                            testprop = parkingsign;
                                            changeprop = true;
                                            if (config.enable_debug)
                                            {
                                                DebugUtils.Log("Vanilla 'No Parking' sign found.");
                                            }
                                        }

                                        if (config.enable_debug)
                                        {
                                            DebugUtils.Log($"American RoadSigns: {pr.name} => changeprop = {changeprop}.");
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
                                                DebugUtils.Log($"{propName} replacement succesful.");
                                            }
                                        }
                                        else
                                        {
                                            //  
                                            if (config.enable_debug)
                                            {
                                                DebugUtils.Log($"{propName} replacement unsuccesful.");
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
            DebugUtils.Log($"Replaced all props succesfully (time elapsed: {ReplacePropsTimer.Elapsed} seconds).");
        }

        public static void ChangeProps(string textureDir)
        {
            var prop_collections = FindObjectsOfType<PropCollection>();
            foreach (var pc in prop_collections)
            {
                foreach (var prefab in pc.m_prefabs)
                {
                    if (prefab.name.Equals("Motorway Overroad Signs"))
                    {
                        //var tex = new Texture2D (1, 1);
                        //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "motorway-overroad-signs.png")));
                        prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "motorway-overroad-signs.dds")));
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
                    }
                    else if (prefab.name.Equals("Street Name Sign"))
                    {
                        //var tex = new Texture2D (1, 1);
                        //tex.LoadImage (System.IO.File.ReadAllBytes (Path.Combine (textureDir, "street-name-sign.png")));
                        prefab.m_material.SetTexture("_MainTex", LoadTextureDDS(Path.Combine(textureDir, "street-name-sign.dds")));
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;

                        prefab.RefreshLevelOfDetail();
                    }
                }
            }
            DebugUtils.Log("Motorway overhead sign props replaced succesfully.");
            DebugUtils.Log("Street name sign props replaced succesfully.");

            //var net_collections = FindObjectsOfType<NetCollection>();
            //try
            //{
            //    foreach (var nc in net_collections)
            //    {
            //        foreach (var prefab in nc.m_prefabs)
            //        {
            //            var prefab_name = prefab.m_class.name.ToLower();
            //            if (!prefab_name.Contains("next"))
            //            {   //  Exclude NExt roads:
            //                if (prefab_name.Contains("highway") || prefab_name.Contains("elevated") || prefab_name.Contains("bridge"))
            //                {
            //                    if (prefab.m_lanes != null)
            //                    {
            //                        foreach (var lane in prefab.m_lanes)
            //                        {
            //                            var list = new FastList<NetLaneProps.Prop>();
            //                            foreach (var prop in lane.m_laneProps.m_props)
            //                            {
            //                                if (!prop.m_prop.name.ToLower().Equals("manhole"))
            //                                {
            //                                    list.Add(prop);
            //                                }
            //                            }
            //                            if (list.m_size > 0)
            //                            {
            //                                lane.m_laneProps.m_props = list.ToArray();
            //                            }
            //                        }
            //                    }
            //                }
            //                //if (prefab.m_class.name.ToLower().Contains("elevated") || prefab.m_class.name.ToLower().Contains("bridge"))
            //                //{
            //                //    foreach (var lane in prefab.m_lanes)
            //                //    {
            //                //        var list = new FastList<NetLaneProps.Prop>();
            //                //        foreach (var prop in lane.m_laneProps.m_props)
            //                //        {
            //                //            if (!prop.m_prop.name.ToLower().Equals("manhole"))
            //                //            {
            //                //                list.Add(prop);
            //                //            }
            //                //        }
            //                //        lane.m_laneProps.m_props = list.ToArray();
            //                //    }
            //                //}
            //            }
            //        }
            //    }
            //    DebugUtils.Log("Manhole cover props removed from elevated roads, bridges and highways succesfully.");
            //}
            //catch (Exception e)
            //{
            //    DebugUtils.Log("Manhole cover props removed from elevated roads, bridges and highways unsuccesfully.");
            //    DebugUtils.LogException(e);
            //}
        }
    }
}
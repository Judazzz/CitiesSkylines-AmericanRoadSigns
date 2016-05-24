using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AmericanRoadSigns
{
    public class AmericanRoadSignsPropReplacer : MonoBehaviour
    {
        public static Configuration config;
        public static readonly string configPath = "CSL_AmericanRoadSigns.xml";

        /// <summary>
        /// Dictionary containing a mapping of default prop names to the replacement propinfos
        /// </summary>
        static Dictionary<string, PropInfo> propReplacementDict = new Dictionary<string, PropInfo>();

        public static void SaveConfig()
        {
            Configuration.Serialize(configPath, config);
        }

        public static void FindProps()
        {
            // Get replacement prop name mapping
            Dictionary<string, string> propsToLoad = new Dictionary<string, string>(PropConsts.AMERICAN_ROAD_PROPS);

            for (int i = 0; i < PrefabCollection<PropInfo>.LoadedCount(); i++)
            {
                string propName = PrefabCollection<PropInfo>.GetLoaded((uint)i).name.ToLower();

                // Check to see if the prop has been found
                for(int j =0; j< propsToLoad.Count; j++ ) 
                {
                    var propToLoad = propsToLoad.ElementAt(j);
                    if (propName.Contains(propToLoad.Value))
                    {
                        propReplacementDict[propToLoad.Key] = PrefabCollection<PropInfo>.GetLoaded((uint)i);
                        propsToLoad.Remove(propToLoad.Key);
                        if (config.enable_debug)
                        {
                            DebugUtils.Log(String.Format("{0} found", propToLoad.Value));

                        }
                    }
                }
            }
            if (propsToLoad.Count == 0)
            {
                DebugUtils.Log("All custom props found.");
            }
            else
            {
                foreach (var unloadedProps in propsToLoad)
                {
                    DebugUtils.Log(string.Format("Prop {0} not found!",unloadedProps.Value));

                }
            }
        }

        public static void ReplaceProps()
        {
            //  Loop NetCollections:
            Stopwatch ReplacePropsTimer = new Stopwatch();
            ReplacePropsTimer.Start();

            NetCollection[] propCollections = FindObjectsOfType<NetCollection>();
            foreach (NetCollection collection in propCollections)
            {
                // Loop prefabs 
                foreach (NetInfo prefab in collection.m_prefabs.Where(prefab => prefab.m_lanes != null))
                {
                    foreach (NetInfo.Lane lane in prefab.m_lanes.Where(lane => lane.m_laneProps != null))
                    {
                        //  Loop LaneProps:
                        for (int i = 0; i < lane.m_laneProps.m_props.Length; i++)
                        {
                            if (lane.m_laneProps.m_props[i].m_prop != null)
                            {

                                PropInfo pr = lane.m_laneProps.m_props[i].m_prop;
                                PropInfo pr2 = lane.m_laneProps.m_props[i].m_finalProp;
                                string propName = pr.name.ToLower();
                                if (pr != null && pr2 != null && propName != null)
                                {
                                    if (propReplacementDict.ContainsKey(propName))
                                    {
                                        if (config.enable_debug)
                                        {
                                            DebugUtils.Log(string.Format("{0} found", propName));
                                        }
                                        PropInfo testprop = propReplacementDict[propName];
                                        if (testprop != null)
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
                                        }

                                        if (config.enable_debug)
                                        {
                                            DebugUtils.Log($"{propName} replacement succesful.");
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
                    if (PropConsts.AMERICAN_ROAD_PROP_TEXTURES.ContainsKey(prefab.name))
                    {
                        prefab.m_material.SetTexture("_MainTex", TextureUtils.LoadTextureDDS(Path.Combine(textureDir, PropConsts.AMERICAN_ROAD_PROP_TEXTURES[prefab.name]+".dds")));
                        if (File.Exists(Path.Combine(textureDir, PropConsts.AMERICAN_ROAD_PROP_TEXTURES[prefab.name] + "-aci.dds")))
                        {
                            prefab.m_material.SetTexture("_ACIMap", TextureUtils.LoadTextureDDS(Path.Combine(textureDir, PropConsts.AMERICAN_ROAD_PROP_TEXTURES[prefab.name] + "-aci.dds")));
                        }
                        if (File.Exists(Path.Combine(textureDir, PropConsts.AMERICAN_ROAD_PROP_TEXTURES[prefab.name] + "-xys.dds")))
                        {
                            prefab.m_material.SetTexture("_XYSMap", TextureUtils.LoadTextureDDS(Path.Combine(textureDir, PropConsts.AMERICAN_ROAD_PROP_TEXTURES[prefab.name] + "-xys.dds")));
                        }
                        prefab.m_lodRenderDistance = 100000;
                        prefab.m_lodMesh = null;
                        prefab.RefreshLevelOfDetail();

                        if (config.enable_debug)
                        {
                            DebugUtils.Log(string.Format("{0} texture replacement successful", prefab.name));
                        }
                    }
            
                }
            }
        }
    }

}


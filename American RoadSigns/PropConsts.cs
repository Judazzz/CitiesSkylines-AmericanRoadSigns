using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmericanRoadSigns
{
    public class PropConsts
    {
        public static readonly List<string> VANILLA_PROPS = new List<string>{
            "30 speed limit",
            "40 speed limit",
            "50 speed limit",
            "60 speed limit",
            "100 speed limit",
            "motorway sign",
            "no right turn sign",
            "no left turn sign",
            "no parking sign", };

        public static readonly Dictionary<string, string> AMERICAN_ROAD_PROPS = new Dictionary<string, string>{
            { "100 speed limit", "speed limit 65" },
            { "60 speed limit", "speed limit 45" },
            { "50 speed limit", "speed limit 30" },
            { "40 speed limit", "speed limit 25" },
            { "30 speed limit", "speed limit 15" },
            { "motorway sign","us interstate sign" },

            { "no right turn sign", "us no right turn" },
            { "no left turn sign", "us no left turn" },
            { "no parking sign", "us no parking" }, };

        public static readonly Dictionary<string, string> AMERICAN_ROAD_PROP_TEXTURES = new Dictionary<string, string>{
            { "Motorway Overroad Signs", "motorway-overroad-signs" },
            { "Street Name Sign", "street-name-sign" }, };
    }
}

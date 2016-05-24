using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AmericanRoadSigns
{
    public class TextureUtils
    {
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
    }
}

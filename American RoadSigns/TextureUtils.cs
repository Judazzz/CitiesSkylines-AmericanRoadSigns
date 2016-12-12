using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AmericanRoadSigns
{
    public class TextureUtils : MonoBehaviour
    {

        public static Texture2D LoadTextureDDS(string textureFullPath)
        {
            var ddsBytes = File.ReadAllBytes(textureFullPath);
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

        public static Texture2D LoadTexture(string textureFullPath)
        {
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(File.ReadAllBytes(textureFullPath));
            texture2D.anisoLevel = 8;
            return texture2D;
        }
    }
}

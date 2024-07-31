using Plusbe.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mono
{
    public class ResourceABManager
    {
        public static ABData ab_test;

        private static List<ABData> abs;

        public static void Init()
        {
            // ab_test = new ABData("");

            abs = new List<ABData>();
            LoadAllAB();
        }

        public static void LoadAllAB()
        {
            LoadAB(new ABData("中间主体"));

        }

        public static Texture2D[] GetTexture2Ds(string fileName)
        {
            for (int i = 0; i < abs.Count; i++)
            {
                if (abs[i].fileName == fileName) return abs[i].sprites;
            }

            return new Texture2D[2];
        }
        private static void LoadAB(ABData data)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(GlobalSetting.ABPath + data.fileName);
            data.sprites = ab.LoadAllAssets<Texture2D>();

            abs.Add(data);
        }
    }

    public class ABData
    {   
        public ABData(string fileName)
        {
            this.fileName = fileName;
        }
        public string fileName;
        public Texture2D[] sprites;
    }
}

/*
*┌────────────────────────────────────────────────┐
*│　描    述：AppData                                                    
*│　作    者：Kimi                                          
*│　版    本：1.0                                              
*│　创建时间：2019/8/28 10:51:47                        
*└────────────────────────────────────────────────┘
*/

using Plusbe.Core;
using Plusbe.Drawing;
using PlusbeHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mono
{
    public class AppData : MonoBehaviour
    {
        private static AppData instance;

        public bool isLoadOK = false;

        // private string logos = "UploadFiles";


        public static AppData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AppData>();
                }

                return instance;
            }
        }

        public void Init()
        {
            //personGuowai = CheckFiles(PathHelper.GetFileImage(GlobalSetting.DataPath + logos, false));

            //StartCoroutine(ToLoadResources());
        }

        private System.Collections.IEnumerator ToLoadResources()
        {
            yield return new WaitForSeconds(0.1f);

            //for (int i = 0; i < personGuowai.Count; i++)
            //{
            //    personGuowai[i].ToLoadResource();

            //    yield return new WaitForSeconds(0.1f);
            //}

            isLoadOK = true;
            Debug.Log("图片资源加载完成！");
        }

        public List<PersonInfo> CheckFiles(string[] files)
        {
            List<PersonInfo> persons = new List<PersonInfo>();

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].IndexOf("logo") == -1)
                {
                    string[] tags = Path.GetFileNameWithoutExtension(files[i]).Split('_');
                    PersonInfo person = new PersonInfo();
                    person.index = persons.Count;

                    string logoPath = Path.GetDirectoryName(files[i]) + "/" + Path.GetFileNameWithoutExtension(files[i]) + "_logo.png";
                    if (File.Exists(logoPath)) person.logoPath = logoPath;

                    person.showName = tags[0];
                    person.bubbleType = 1;
                    person.contentPath = files[i];

                    if (tags.Length >= 2)
                    {
                        try
                        {
                            person.bubbleType = Convert.ToInt32(tags[1]);

                        }
                        catch { }
                    }

                    persons.Add(person);
                }
            }

            return persons;
        }
    }

    public class PersonInfo
    {
        public int index;

        public string showName;

        public string logoPath; //是否有对应得logo信息
        public string contentPath; //内容

        public Texture logo = null;
        public Texture content = null;

        public int bubbleType;

        public void ToLoadResource()
        {
            if (File.Exists(contentPath))
            {
                content = PicHelper.LoadPicTexture(contentPath);
            }
        }

        public int GetIndexType()
        {
            if (bubbleType <= 4 && bubbleType >= 1)
                return bubbleType - 1;

            return 0;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlusbeQuickPlugin.HttpService
{
    public enum PlusbeWebDataType
    {
        Any,
        MOVIE,
        Web,
        PPT,
        PIC
    }
    public class PlusbeWebV3
    {
        private static Dictionary<string, PlusbeWebV3JsonLoader> dict = new Dictionary<string, PlusbeWebV3JsonLoader>();
        static PlusbeWebV3JsonLoader defaultLoader;
        public static PlusbeWebV3JsonLoader DefaultLoader { 
            get {
                if (defaultLoader == null)
                {
                    defaultLoader = new PlusbeWebV3JsonLoader();
                    string dataPath = Application.isEditor ? Application.dataPath + "/../Apps/Datas/" : Application.dataPath + "/../Datas/";
                    string downloadJsonPath = dataPath + "Configs/FileConfig.json";
                    defaultLoader.LoadJsonFile(downloadJsonPath);
                }
                return defaultLoader;
            }
        }

        public static PlusbeWebV3JsonLoader AddFileConfigLoader(string loaderName)
        {
            PlusbeWebV3JsonLoader loader = new PlusbeWebV3JsonLoader();
            if (dict.ContainsKey(loaderName)) dict.Remove(loaderName);
            dict.Add(loaderName, loader);
            return loader;
        }

        public static PlusbeWebV3JsonLoader GetFileConfigLoader(string loaderName)
        {
            if (dict.TryGetValue(loaderName, out PlusbeWebV3JsonLoader loader)) 
                return loader;
            return null;
        }

    }
    public class PlusbeWebV3JsonLoader
    {
        public List<PlusbeWebContent> ContentList { get; private set; } = new List<PlusbeWebContent>();
        public List<PlusbeWebColumn> ColumnList { get; private set; } = new List<PlusbeWebColumn>();
        private string jsonPath;
        private string recordedJsonContent;

        /// <summary>
        /// 加载json
        /// </summary>
        /// <param name="jsonPath"></param>
        public void LoadJsonFile(string jsonPath)
        {
            Debug.Log("准备加载后台配置文件 webV3jsonPath : " + jsonPath);
            this.jsonPath = jsonPath;
            if(File.Exists(jsonPath)==false)
            {
                Debug.LogWarning("后台json文件不存在");
                return;
            }
            string jsonContent = File.ReadAllText(jsonPath);
            if (jsonContent == "")
            {
                Debug.LogWarning("json数据为空");
                return;
            }
            recordedJsonContent = jsonContent;
            ReadColumnList(jsonContent);
            ReadDataList(jsonContent);
        }

        public void LoadJson(JObject jo)
        {

            ReadColumnList(jo);
            ReadDataList(jo);
        }

        /// <summary>
        /// 检查数据是否发生变化
        /// </summary>
        /// <returns></returns>
        public bool CheckCotentUpdated()
        {
            string latestJsonContent = File.ReadAllText(jsonPath);
            bool hasChanged = latestJsonContent != recordedJsonContent;
            recordedJsonContent = hasChanged ? latestJsonContent : recordedJsonContent;
            return hasChanged;
        }

        private void ReadDataList(string jsonContent)
        {
            ContentList.Clear();
            JObject jo = JsonConvert.DeserializeObject(jsonContent) as JObject;
            JArray jArray = jo["list"] as JArray;
            foreach (JObject obj in jArray)
            {
                PlusbeWebContent content = new PlusbeWebContent();
                content.AnalyzeJson(obj);
                ContentList.Add(content);
            }
        }
        private void ReadDataList(JObject jo)
        {
            ContentList.Clear();
            JArray jArray = jo["list"] as JArray;
            foreach (JObject obj in jArray)
            {
                PlusbeWebContent content = new PlusbeWebContent();
                content.AnalyzeJson(obj);
                ContentList.Add(content);
            }
        }
        private void ReadColumnList(string jsonContent)
        {
            ColumnList.Clear();
            JObject jo = JsonConvert.DeserializeObject(jsonContent) as JObject;
            JArray jArray = jo["Channellist"] as JArray;
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject obj = jArray[i] as JObject;
                PlusbeWebColumn dataModel = new PlusbeWebColumn();
                dataModel.AnalyzeJson(obj);
                ColumnList.Add(dataModel);
                if(dataModel.News!=null&&dataModel.News.Length>0)
                {
                    ContentList.AddRange(dataModel.News);
                }
                
            }
        }

        private void ReadColumnList(JObject jo)
        {
            ColumnList.Clear();
            JArray jArray = jo["Channellist"] as JArray;
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject obj = jArray[i] as JObject;
                PlusbeWebColumn dataModel = new PlusbeWebColumn();
                dataModel.AnalyzeJson(obj);
                ColumnList.Add(dataModel);
                if (dataModel.News != null && dataModel.News.Length > 0)
                {
                    ContentList.AddRange(dataModel.News);
                }

            }
        }

        //获取根栏目的名称
        public string GetRootColumnName()
        {
            return ColumnList.Find(x => x.ID == x.RootID).Name;
        }
        //获取根目录ID
        public int GetRootColumnID()
        {
            return ColumnList.Find(x => x.ID == x.RootID).ID;
        }

        public PlusbeWebColumn[] GetRootSubColumns()
        {
            return ColumnList.FindAll(x => x.ParentID == x.RootID).ToArray();
        }

        public string[] GetRootSubColumnNames()
        {
            var arr=GetRootSubColumns();
            List<string> result = new List<string>();
            foreach (var column in arr)
                result.Add(column.Name);
            return result.ToArray();
        }

        /// <summary>
        /// 通过路径返回下属栏目节点 root/columnA  --> a-1, a-2, a-3
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public PlusbeWebColumn[] GetChildColumnsByPath(string path)
        {
            string[] steps = path.Split('/');//层级数组
            List<PlusbeWebColumn> resultColumnList = ColumnList;
            for (int i = 0; i < steps.Length; i++)
            {
                string currStepColumnName = steps[i];
                int id = GetColumnIDByName(currStepColumnName,resultColumnList);
                resultColumnList = GetSubColumns(id);
            }
            return resultColumnList.ToArray();
        }
        public string[] GetColumnNamesByPath(string path)
        {
            var result = new List<string>();
            var columns = GetChildColumnsByPath(path);
            foreach (var column in columns)
            {
                result.Add(column.Name);
            }
            return result.ToArray();
        }
        public PlusbeWebColumn GetColumnByName(string name)
        {
            return ColumnList.Find(x => x.Name == name);
        }

        /// <summary>
        /// 获取指定栏目和指定类型的数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public PlusbeWebContent[] GetContentsWith(string path,PlusbeWebDataType type=PlusbeWebDataType.Any)
        {
            var columnID = GetColumnIDByPath(path);
            if (columnID == -1) return null;
            return GetContentsWith(columnID, type);
        }

        /// <summary>
        /// 获取指定名称的数据
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public PlusbeWebContent GetFirstContentWithTitle(string path, string title)
        {
            return GetContentsWith(path).Where(x => x.Title == title).FirstOrDefault();
        }
        
        public PlusbeWebContent GetFirstContentWith(string path,PlusbeWebDataType type=PlusbeWebDataType.Any)
        {
            var arr = GetContentsWith(path, type);
            return arr.Length == 0 ? null : arr[0];
        }

        public PlusbeWebContent GetFirstContentWith(int columnID, PlusbeWebDataType type = PlusbeWebDataType.Any)
        {
            var arr = GetContentsWith(columnID, type);
            return arr.Length == 0 ? null : arr[0];
        }

        public PlusbeWebContent[] GetContentsWith(int columnID, PlusbeWebDataType type = PlusbeWebDataType.Any)
        {
            //获取所有内容
            List<PlusbeWebContent> result = new List<PlusbeWebContent>();
            for (int i = 0; i < ContentList.Count; i++)
            {
                PlusbeWebContent data = ContentList[i];
                if (data.ChannelID == columnID)
                {
                    result.Add(data);
                }
            }
            
            //筛选类型
            if (type != PlusbeWebDataType.Any)
            {
                result = result.FindAll(x => x.FileType == type);
            }
            return result.ToArray();
        }

        public PlusbeWebContent GetContentWithID(int ID)
        {
            return ContentList.Find(x => x.ID == ID);
        }

        public PlusbeWebColumn GetColumnByID(int ID)
        {
            return ColumnList.Find(x => x.ID == ID);
        }

        public int GetColumnIDByPath(string path)
        {
            string[] steps = path.Split('/');//层级数组
            List<PlusbeWebColumn> resultColumnList = ColumnList;
            int id = -1;
            for (int i = 0; i < steps.Length; i++)
            {
                string currStepColumnName = steps[i];
                id = GetColumnIDByName(currStepColumnName, resultColumnList);
                resultColumnList = GetSubColumns(id);
            }
            return id;
        }
        public int GetColumnIDByName(string columnName, List<PlusbeWebColumn> columnList)
        {
            var column=columnList.Find(x => x.Name == columnName);
            if (column != null) return column.ID;
            return -1;
        }
        List<PlusbeWebColumn> GetSubColumns(int columnID)
        {
            var list = new List<PlusbeWebColumn>();
            foreach (PlusbeWebColumn col in ColumnList)
            {
                if (col.ParentID == columnID)
                {
                    list.Add(col);
                }
            }
            return list;
        }
    }
    public class PlusbeWebColumn:ITreeNodeConvertAble
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brief { get; set; }
        public string OrderID { get; set; }
        public string IsChild { get; set; }
        public int ParentID { get; set; }
        public int Depth { get; set; }
        public int RootID { get; set; }
        public PlusbeWebContent[] News { get; set; }
        public object NodeData { get => this;}
        public PlusbeWebColumn Parent
        {
            get
            {
                PlusbeWebColumn pColumn = PlusbeWebV3.DefaultLoader.GetColumnByID(ParentID);
                return pColumn;
            }
        }
        public string ParentName
        {
            get
            {
                return Parent != null ? Parent.Name : "";
            }
        }

        public void AnalyzeJson(JObject obj)
        {
            ID = Convert.ToInt32(obj["ID"].ToString());
            Name = obj["Name"].ToString();
            Brief = obj["Brief"].ToString();
            OrderID = obj["OrderID"].ToString();
            IsChild = obj["IsChild"].ToString();
            ParentID = Convert.ToInt32(obj["ParentID"].ToString());
            Depth = Convert.ToInt32(obj["Depth"].ToString());
            RootID = Convert.ToInt32(obj["RootID"].ToString());

            JArray jarr = obj["News"] as JArray;
            if (jarr == null) return;
            News = new PlusbeWebContent[jarr.Count];
            for (int i = 0; i < jarr.Count; i++)
            {
                PlusbeWebContent c = new PlusbeWebContent();
                JObject jo = jarr[i] as JObject;
                c.AnalyzeColumnJson(jo, ID);
                News[i] = c;
            }
        }
    }
    public class PlusbeWebContent
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Pic { get; set; }
        string files = "";
        public string Files {
            get { return files; }
            set {
                this.files = value;
                VideoFiles = new List<string>();
                PicFiles = new List<string>();
                AllFiles = new List<string>();
                string[] files = this.files.Split('|');
                for (int j = 0; j < files.Length; j++)
                {
                    string file = files[j];
                    string dataPath = Application.isEditor ? Application.dataPath + "/../Apps/Datas/" : Application.dataPath + "/../Datas/";
                    string filePath = dataPath + file;
                    AllFiles.Add(filePath);

                    if (IsVideoFile(file))
                        VideoFiles.Add(filePath);
                    else if (IsPicFile(file))
                        PicFiles.Add(filePath);
                }
            }
        }
        public int Tag { get; set; }//后台场景标签
        public string Word { get; set; }
        public string Brief1 { get; set; }
        public string Brief2 { get; set; }
        public PlusbeWebDataType FileType { get; set; }
        public int ChannelID { get; set; }//后台栏目标签

        public PlusbeWebColumn Column;

        public List<string> PicFiles;
        public List<string> VideoFiles;
        public List<string> AllFiles;

        public string FirstPic
        {
            get
            {
                if (PicFiles.Count > 0)
                    return PicFiles[0];
                return "";
            }
        }
        public string FirstVideo
        {
            get
            {
                if (VideoFiles.Count > 0)
                    return VideoFiles[0];
                return "";
            }
        }

        public void AnalyzeJson(JObject obj)
        {
            ID = Convert.ToInt32(obj["ID"]);
            Title = obj["Title"].ToString();
            Files = obj["File"].ToString();
            Tag = Convert.ToInt32(obj["Tag"]);
            ChannelID = Convert.ToInt32(obj["ChannlID"]);
            Word = obj["Word"].ToString();
            Brief1 = obj["Brief1"].ToString();
            Brief2 = obj["Brief2"].ToString();
            FileType = (PlusbeWebDataType)Convert.ToInt32(obj["FileType"]);

            Column = PlusbeWebV3.DefaultLoader.GetColumnByID(ChannelID);
        }

        public void AnalyzeColumnJson(JObject obj,int channelID)
        {
            ID = Convert.ToInt32(obj["ID"]);
            Title = obj["Title"].ToString();
            Files = obj["Files"].ToString();
            ChannelID = channelID;
            Word = obj["Brief"].ToString();
            FileType = (PlusbeWebDataType)GetTypeByFileAndWord();

            Column = PlusbeWebV3.DefaultLoader.GetColumnByID(ChannelID);
        }

        int GetTypeByFileAndWord()
        {
            if(Word.StartsWith("http"))
                return 2;
            if (AllFiles.TrueForAll(IsPPTFile))
                return 3;
            if (AllFiles.TrueForAll(IsPicFile))
                return 4;
            return 1;
        }

        bool IsInSets<T>(T target,IEnumerable<T> sets)
        {
            List<T> l = new List<T>();
            l.AddRange(sets);
            return l.Contains(target);
        }

        bool IsPPTFile(string file)
        {
            string format = Path.GetExtension(file);
            return IsInSets(format.ToLower(), new string[] { ".ppt", ".pptx" });
        }

        bool IsVideoFile(string file)
        {
            string format = Path.GetExtension(file);
            return IsInSets(format.ToLower(), new string[] { ".mp4", ".mov", ".avi" });
        }

        bool IsPicFile(string file)
        {
            string format = Path.GetExtension(file);
            return IsInSets(format.ToLower(), new string[] { ".jpg", ".jpeg", ".png" });
        }


        public string[] GetFMTFiles(params string[] fmts)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < AllFiles.Count; i++)
            {
                string path = AllFiles[i];
                string fmt = Path.GetExtension(path);
                //Debug.Log($"path: {path} fmt: {fmt}");
                if (IsInSets(fmt.ToLower(), fmts))
                {
                    list.Add(path);
                }
            }
            return list.ToArray();
        }

        public string GetFirstFMTFile(params string[] fmts)
        {
            var arr = GetFMTFiles(fmts);
            if (arr.Length > 0)
                return arr[0];
            return "";
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("-----------PlusbeWebCotent-----------\nID: " + ID);
            sb.Append("\nTitle: " + Title);
            sb.Append("\nFiles: " + Files);
            sb.Append("\nWord: " + Word);
            sb.Append("\nBrief1: " + Brief1);
            sb.Append("\nBrief2: " + Brief2);
            sb.Append("\nFileType: " + FileType);
            sb.Append("\nChannelID: " + ChannelID);
            return sb.ToString();
        }
    }

}


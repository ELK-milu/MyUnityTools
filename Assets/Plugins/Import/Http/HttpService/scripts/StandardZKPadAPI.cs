using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace PlusbeQuickPlugin.HttpService
{
    /// <summary>
    /// 自动生成可供PAD调用的标准接口数据
    /// </summary>
    public class StandardZKPadAPI
    {
        private static StandardZKPadAPI singleton;
        public static StandardZKPadAPI Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new StandardZKPadAPI();
                }
                return singleton;
            }
        }

        public delegate void StandardPlayAtID(int id);
        public delegate void StandardPlayAtColumn(PlusbeWebColumn column);
        public delegate void StandardPlayAtContent(PlusbeWebContent content);
        public StandardPlayAtColumn ColumnPlayAt;
        public StandardPlayAtContent ContentPlayAt;
        public StandardPlayAtID ContentPlayAtID;
        public StandardPlayAtID ColumnPlayAtID;
        
        List<StandardContent> list = new List<StandardContent>();
        List<StandardColumn> columnlist = new List<StandardColumn>();
        public void AddTag(int tag,string name,int parentTag)
        {
            StandardColumn column = new StandardColumn
            {
                Tag = tag,
                Name = name,
                ParentTag = parentTag
            };
            columnlist.Add(column);
        }
        public void AddContent(int ID, string title, int tag)
        {
            StandardContent content = new StandardContent
            {
                ID = ID,
                Title = title,
                Tag = tag
            };
            list.Add(content);
        }

        /// <summary>
        /// 移除指定栏目
        /// </summary>
        public void RemovePlusbeColumn(string column)
        {
            int id=PlusbeWebV3.DefaultLoader.GetColumnIDByPath(column);
            var needDeleteColumn = columnlist.Where(x => x.ID == id).FirstOrDefault();
            columnlist.Remove(needDeleteColumn);
        }

        /// <summary>
        /// 添加后台栏目
        /// </summary>
        /// <param name="columns"></param>
        private void AddPlusbeColumns(PlusbeWebColumn[] columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                AddTag(columns[i].ID, columns[i].Name, columns[i].ParentID);
            }
        }

        /// <summary>
        /// 添加后台数据
        /// </summary>
        /// <param name="contents"></param>
        private void AddPlusbeContents(PlusbeWebContent[] contents)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                AddContent(contents[i].ID, contents[i].Title, contents[i].ChannelID);
            }
        }

        TreeViewDataModel treeModel;
        /// <summary>
        /// 创建标准中控API，并指定使用的端口
        /// </summary>
        /// <param name="defaultServicePort"></param>
        public void CreateToUse()
        {
            //解析树状图
            treeModel = new TreeViewDataModel();
            treeModel.AnalyzeNodes(columnlist.ToArray());
            //绑定到默认服务,并提供查询和点播接口
            HttpServiceManager.StandardService.RegisterResponseCommand("获取json列表","act,states","unity,json","object",ResponseJosnList);
            HttpServiceManager.StandardService.RegisterCallCommand("点播指定ID内容", "act,states", "unity,content", "object", CallPlayAtContent);
            HttpServiceManager.StandardService.RegisterCallCommand("点播指定ID栏目", "act,states", "unity,channel", "object", CallPlayAtColumn);
        }

        public void CreateByCloudWeb()
        {
            StandardZKPadAPI.Instance.AddPlusbeColumns(PlusbeWebV3.DefaultLoader.ColumnList.ToArray());
            StandardZKPadAPI.Instance.AddPlusbeContents(PlusbeWebV3.DefaultLoader.ContentList.ToArray());
            StandardZKPadAPI.Instance.CreateToUse();
        }

        private string ResponseJosnList(string[] values)
        {
            string obj = values[0];
            if (int.TryParse(obj, out int tag))
            {
                return GetTagList(tag);
            }
            return GetFullList();
        }

        private void CallPlayAtColumn(string[] keyValues)
        {
            if (int.TryParse(keyValues[0], out int id))
            {
                ColumnPlayAt?.Invoke(PlusbeWebV3.DefaultLoader.GetColumnByID(id));
                ColumnPlayAtID?.Invoke(id);
            }
        }
        private void CallPlayAtContent(string[] keyValues)
        {
            if (int.TryParse(keyValues[0], out int id))
            {
                ContentPlayAt?.Invoke(PlusbeWebV3.DefaultLoader.GetContentWithID(id));
                ContentPlayAtID?.Invoke(id);
            }
        }

        public string GetFullList()
        {
            JObject jo = new JObject();
            jo.Add("RootNode", treeModel.RootNode.ID);
            
            JArray jarr = new JArray();
            jo.Add("list", jarr);

            StandardContent.StartToConvert();
            for (int i = 0; i < list.Count; i++)
            {
                jarr.Add(list[i].ConvertToListJObject());
            }

            JArray jarr2 = new JArray();
            jo.Add("subColumns", jarr2);
            for(int i=0;i<columnlist.Count;i++)
            {
                columnlist[i].Depth = treeModel.NodeList.Find(x => x.ID == columnlist[i].ID).Depth;
                jarr2.Add(columnlist[i].ConvertToJObject());
            }
            return jo.ToString();
        }

        /// <summary>
        /// 按照指定标签返回数据接口
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagList(int tag)
        {
            JObject jo = new JObject();
            jo.Add("RootNode", treeModel.RootNode.ID);

            JArray jarr = new JArray();
            jo.Add("list", jarr);

            StandardContent.StartToConvert();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Tag == tag)
                    jarr.Add(list[i].ConvertToListJObject());
            }

            JArray jarr2 = new JArray();
            jo.Add("subColumns", jarr2);
            var subColumns = treeModel.NodeList.Find(x => x.ID == tag).ChildrenNodes;
            for (int i = 0; i < subColumns.Count; i++)
            {
                var column = columnlist.Find(x => x.Tag == subColumns[i].ID);
                column.Depth = treeModel.NodeList.Find(x => x.ID == column.ID).Depth;
                jarr2.Add(column.ConvertToJObject());
            }
            return jo.ToString();
        }

        public class StandardColumn:ITreeNodeConvertAble
        {
            public string Name { get; set; }
            public int Tag { get; set; }
            public int ParentTag { get; set; }
            public int Depth { get; set; }

            public JObject ConvertToJObject()
            {
                JObject jo = new JObject();
                jo.Add("Name", Name);
                jo.Add("Tag", Tag);
                jo.Add("ParentTag", ParentTag);
                jo.Add("Depth", Depth);
                return jo;
            }

            #region ITreeNodeConvertAble接口
            public int ID => Tag;
            public int ParentID => ParentTag;
            public object NodeData => Name;
            #endregion
        }

        public class StandardContent
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Word { get; set; }
            public int FileType { get; set; } = 1;
            public int Tag { get; set; } = 1;

            static int index = 0;


            public static void StartToConvert()
            {
                index = 0;
            }

            public JObject ConvertToListJObject()
            {
                JObject jo = new JObject();
                jo.Add("ID", ID);
                jo.Add("title", Title);
                jo.Add("word", Word);
                jo.Add("index", index);
                jo.Add("fileType", FileType);
                jo.Add("tag", Tag);
                index++;
                return jo;
            }
        }
    }


}
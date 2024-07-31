using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlusbeQuickPlugin.HttpService
{
    public class TreeViewDataModel
    {

        #region 属性
        public TreeNode RootNode { get; set; }
        public List<TreeNode> NodeList { get; set; }
        private int CreateID { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 解析所有离散的节点形成模型树
        /// </summary>
        /// <param name="nodes"></param>
        public void AnalyzeNodes(ITreeNodeConvertAble[] nodes)
        {
            NodeList.Clear();
            List<TreeNode> NodeListNeedAnalyze = new List<TreeNode>();
            List<TreeNode> NoDadyNodeList = new List<TreeNode>();
            Dictionary<int, int> parentIDDict = new Dictionary<int, int>();

            for (int i = 0; i < nodes.Length; i++)
            {
                ITreeNodeConvertAble nodeAble = nodes[i];
                TreeNode node = new TreeNode();
                node.ID = nodeAble.ID;
                node.Name = nodeAble.Name;
                node.DATA = nodeAble.NodeData;
                parentIDDict[node.ID] = nodeAble.ParentID;
                NodeListNeedAnalyze.Add(node);
                NodeList.Add(node);
            }

            for (int i = 0; i < NodeListNeedAnalyze.Count; i++)
            {
                var node = NodeListNeedAnalyze[i];
                int parentId = parentIDDict[node.ID];
                node.ParentNode = NodeListNeedAnalyze.Find(x => x.ID == parentId);
                if(node.ParentNode!=null)
                {
                    node.ParentNode.AddChild(node);
                }
                else
                {
                    NoDadyNodeList.Add(node);
                }
            }

            //如果没有找到父节点的节点只有一个，那么它就作为根节点
            if(NoDadyNodeList.Count==1)
            {
                RootNode = NoDadyNodeList[0];
            }
            else//如果没有父节点的节点多于1个，那么创建一个虚拟根节点包容他们
            {
                RootNode = new TreeNode();
                RootNode.Name = "Root";
                RootNode.ID = -1;
                NoDadyNodeList.ForEach(x => RootNode.AddChild(x));
                NodeList.Add(RootNode);
            }
        }

        public void GenerateRenderOrder()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            RootNode.GetAllSubNodes(ref nodes);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].RenderOrder = i;
            }
        }

        /// <summary>
        /// 手动创建节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public TreeNode CreateNodeHand(string name, object data, TreeNode parentNode)
        {
            bool createAsRootNode = parentNode == null && RootNode == null;
            if (!createAsRootNode && parentNode == null) return null;

            TreeNode node = new TreeNode();
            node.ID = CreateID++;
            node.Name = name;
            node.DATA = data;

            if (createAsRootNode)
                RootNode = node;//作为根节点创建
            else
                parentNode.AddChild(node);

            NodeList.Add(node);
            return node;
        }

        public TreeNode CreateNodeHand(string name,object data,int parentID)
        {
            var parentNode = NodeList.Find(x => x.ID == parentID);
            return CreateNodeHand(name, data, parentNode);
        }

        /// <summary>
        /// 手动创建节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public TreeNode CreateNodeHand(string name,object data,string parentPath)
        {
            var parentNode = NodeList.Find(x => x.LinkPath == parentPath);
            return CreateNodeHand(name, data, parentNode);
        }

        /// <summary>
        /// 删除节点及其所有下属节点
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(TreeNode node)
        {
            if(NodeList.Contains(node))
            {
                List<TreeNode> subNodes = new List<TreeNode>();
                node.GetAllSubNodes(ref subNodes);

                NodeList.Remove(node);
                for (int i = 0; i < subNodes.Count; i++)
                {
                    NodeList.Remove(subNodes[i]);
                }
            }   
        }

        /// <summary>
        /// 通过路径得到ID
        /// </summary>
        /// <param name="path">like "root/columnA/A-1</param>
        /// <returns></returns>
        public int GetIDByPath(string path)
        {
            var node=NodeList.Find(x => x.LinkPath == path);
            return node != null ? node.ID : -1;
        }

        public override string ToString()
        {
            string GetRepeatStr(int num)
            {
                StringBuilder sb = new StringBuilder();
                for(int i=0;i<num;i++)
                {
                    sb.Append("-");
                }
                return sb.ToString();
            }

            string GetPaddingStr(int depth)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < depth; i++)
                {
                    if (i == depth - 1)
                        sb.Append(GetRepeatStr(depth));
                    else
                        sb.Append("   ");
                }
                return sb.ToString();
            }

            if(RootNode!=null)
            {
                List<TreeNode> nodes = new List<TreeNode>();
                RootNode.GetAllSubNodes(ref nodes);

                StringBuilder sb = new StringBuilder($"{RootNode.Name} ({RootNode.ID})\n");
                for (int i = 0; i < nodes.Count; i++)
                {
                    TreeNode node = nodes[i];
                    sb.AppendLine($"{GetPaddingStr(node.Depth)} {node.Name} ({node.ID})");
                }
                return sb.ToString();
            }
            return "<color=#ff0000>XXXXXXX--No Data!!--XXXXXXXX</color>";
        }
        #endregion

        public TreeViewDataModel()
        {
            NodeList = new List<TreeNode>();
        }
    }



}


using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlusbeQuickPlugin.HttpService
{
    public class TreeNode
    {
        #region 属性
        public int ID { get; set; }
        public int RenderOrder { get; set; }
        public string Name { get; set; }
        public TreeNode ParentNode { get; set; }
        public List<TreeNode> ChildrenNodes { get; set; }
        public int Depth
        {
            get
            {
                int depth = 0;
                TreeNode pNode = ParentNode;
                while (pNode != null)
                {
                    depth++;
                    pNode = pNode.ParentNode;
                }
                return depth;
            }
        }

        public string LinkPath
        {
            get
            {
                string[] arr = LinkArr;
                StringBuilder sb = new StringBuilder(arr[0]);
                for (int i = 1; i < arr.Length; i++)
                {
                    sb.Append($"/{arr[i]}");
                }
                return sb.ToString();
            }
        }

        public string[] LinkArr
        {
            get
            {
                List<string> list = new List<string>();
                list.Add(Name);
                TreeNode pNode = ParentNode;
                while (pNode != null)
                {
                    list.Add(pNode.Name);
                    pNode = pNode.ParentNode;
                }
                list.Reverse();
                return list.ToArray();
            }
        }

        public object DATA { get; set; }

        #endregion

        #region 方法
        public void AddChild(TreeNode node)
        {
            if(ChildrenNodes.Contains(node)==false)
            {
                ChildrenNodes.Add(node);
                node.ParentNode = this;
            }
        }

        public void RemoveChild(TreeNode node)
        {
            if(ChildrenNodes.Contains(node))
            {
                ChildrenNodes.Remove(node);
                node.ParentNode = null;
            }
        }

        /// <summary>
        /// 递归返回所有直接子节点和间接子节点
        /// </summary>
        /// <returns></returns>
        public void GetAllSubNodes(ref List<TreeNode> list)
        {
            for (int i = 0; i < ChildrenNodes.Count; i++)
            {
                list.Add(ChildrenNodes[i]);
                ChildrenNodes[i].GetAllSubNodes(ref list);
            }
        }
        #endregion

        public TreeNode()
        {
            ChildrenNodes = new List<TreeNode>();
        }
    }


}


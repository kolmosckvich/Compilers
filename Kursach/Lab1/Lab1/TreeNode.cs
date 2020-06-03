using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class TreeNode
    {
        public int Id;

        public TreeNode Parent;

        public List<TreeNode> Childs = new List<TreeNode>();

        public string TextNode = "";
        public string TypeNode = "";
        public string NodeValue = "";
        public string MacroType = "";

    }
}

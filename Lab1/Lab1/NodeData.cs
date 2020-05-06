using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class NodeData
    {
        public string NodeId;
        public List<NodeLink> ids = new List<NodeLink>();
        public bool IsInit;
        public bool IsFinish;
    }

    public class NodeLink
    {
        public string Id;
        public char Data;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Model
    {
        Node init;

        public Model(Node Init)
        {
            init = Init;
        }

        public bool Process(string exp)
        {
            Node curr = init;
            bool result = false;
            foreach(var c in exp)
            {
                if (!Utils.alphabet.Contains(c))
                    throw new Exception("Unknown symbol!");
                curr = GetNextNode(curr, c);
            }
            if (curr.isFinishNode)
                result = true;
            return result;
        }

        private Node GetNextNode(Node n, char c)
        {
            Node ret = null;
            foreach(var link in n.Outputs)
            {
                if(link.Subj == c)
                {
                    ret = link.Destination;
                }
            }
            return ret;
        }
    }
}

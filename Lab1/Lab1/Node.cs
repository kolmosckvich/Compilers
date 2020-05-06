using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Node
    {
        public Node Start;
        public Node Finish;
        public string Id;
        public int PrivateId;
        public bool isFinishNode;
        private static int privateId = 1;

        public Node()
        {
            Initialize();
        }

        public void Initialize()
        {
            Start = this;
            Finish = this;
            PrivateId = privateId++;
        }

        public virtual void AddInput(Transition link)
        {
            Inputs.Add(link);
        }

        public virtual void AddDest(Transition link)
        {
            Outputs.Add(link);
        }

        public List<Transition> Inputs = new List<Transition>();
        public List<Transition> Outputs = new List<Transition>();

        private string Name;

    }
}

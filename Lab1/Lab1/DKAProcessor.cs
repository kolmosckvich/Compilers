using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab1
{
    public static class DKAProcessor
    {
        public static Node GetDKA(Node initNKA)
        {
            List<DState> DStates = new List<DState>();
            var init = EClosure(new DState(initNKA));
            processFinishNode(init);
            DStates.Add(init);
            var selected = DStates.Where(x => !x.IsMarked()).FirstOrDefault();
            while (selected != null)
            {
                selected.Mark();
                foreach(var c in Utils.alphabet)
                {
                    var state = Move(selected, c);
                    processFinishNode(state);
                    if (!IsStateInCollection(state, DStates))
                    {
                        DStates.Add(state);
                        selected.Bind(state, c);
                    }
                    else
                    {
                        var collState = EqualStateFromCollection(state, DStates);
                        selected.Bind(collState, c);
                    }
                }
                selected = DStates.Where(x => !x.IsMarked()).FirstOrDefault();
            }
            return init;
        }

        private static DState EClosure(DState dState)
        {
            Stack<Node> statesStack = new Stack<Node>();
            DState res = new DState();
            foreach (var node in dState.Nodes)
            {
                statesStack.Push(node);
                res.AddNode(node);
            }
            while (statesStack.Count > 0)
            {
                var state = statesStack.Pop();
                foreach (var link in state.Start.Outputs)
                {
                    if (link.Subj == Utils.eps && !res.Nodes.Contains(link.Destination))
                    {
                        res.AddNode(link.Destination);
                        statesStack.Push(link.Destination);
                    }
                }
            }
            return res;
        }

        private static DState Move(DState dState, char a)
        {
            DState res = new DState();
            //var epsState = EClosure(dState);
            foreach (var node in dState.Nodes)
            {
                foreach (var link in node.Start.Outputs)
                {
                    if (link.Subj == a)
                    {
                        res.AddNode(link.Destination);
                    }
                }
            }
            var epsState = EClosure(res);
            return epsState;
        }

        private static bool IsStateInCollection(DState state, IEnumerable<DState> states)
        {
            bool ans = false;
            foreach(var sec in states)
            {
                if(IsEqualStates(state, sec))
                {
                    ans = true;
                    break;
                }
            }
            return ans;
        }

        private static DState EqualStateFromCollection(DState state, IEnumerable<DState> states)
        {
            DState ans = null;
            foreach (var sec in states)
            {
                if (IsEqualStates(state, sec))
                {
                    ans = sec;
                    break;
                }
            }
            return ans;
        }

        private static bool IsEqualStates(DState first, DState second)
        {
            bool equal = first.Nodes.Count == second.Nodes.Count;
            if(equal)
            {
                foreach (var node in first.Nodes)
                {
                    if (!second.Nodes.Contains(node))
                    {
                        equal = false;
                    }
                }
            }
            return equal;
        }

        private static void processFinishNode(DState state)
        {
            foreach(var node in state.Nodes)
            {
                if(node.Start.Outputs.Count == 0)
                {
                    state.isFinishNode = true;
                    break;
                }
            }
        }
    }

    public class DState : Node
    {
        private List<Node> _nodes;
        private bool marked = false;

        public List<Node> Nodes
        {
            get
            {
                return _nodes;
            }
        }

        public DState()
        {
            _nodes = new List<Node>();
        }

        public DState(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public DState(Node node)
        {
            _nodes = new List<Node>();
            _nodes.Add(node);
        }

        public void AddNode(Node n)
        {
            _nodes.Add(n);
        }

        public void Mark()
        {
            marked = true;
        }

        public bool IsMarked()
        {
            return marked;
        }
        
        public void Bind(DState next, char c)
        {
            GraphAutomat.Bind(this, next, c);
        }

    }

}

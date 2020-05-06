using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab1
{
    public static class MKAProcessor
    {
        public static Node GetMKA(Node initDKA)
        {
            var nodes = GetAllNodes(initDKA);
            var table = BuildTable(nodes);
            var eqClasses = GetEqClasses(table);
            var classesDict = GetClassesDict(nodes, eqClasses);
            var MKA = GetMKAGraph(nodes, classesDict);
            return MKA;
        }

        public static List<Node> GetAllNodes(Node init)
        {
            List<Node> nodes = new List<Node>();
            Queue<Node> qNodes = new Queue<Node>();
            qNodes.Enqueue(init);
            while(qNodes.Count != 0)
            {
                var qnode = qNodes.Dequeue();
                nodes.Add(qnode);
                foreach(var link in qnode.Start.Outputs)
                {
                    if(!nodes.Contains(link.Destination) && !qNodes.Contains(link.Destination))
                    {
                        qNodes.Enqueue(link.Destination);
                    }
                }
            }
            return nodes;
        }

        public static bool[][] BuildTable(List<Node> nodes)
        {
            Queue<Tuple<Node, Node>> qNodes = new Queue<Tuple<Node, Node>>();
            Dictionary<string, int> indexes = new Dictionary<string, int>();
            int nodesCount = nodes.Count;
            bool[][] table = new bool[nodesCount][];
            for (int i = 0; i < nodesCount; i++)
            {
                table[i] = new bool[nodesCount];
                indexes.Add(nodes[i].Id, i);
            }

            for (int i = 0; i < nodesCount; i++)
            {
                for (int j = 0; j < nodesCount; j++)
                {
                    if(!table[i][j] && nodes[i].isFinishNode != nodes[j].isFinishNode)
                    {
                        table[i][j] = true;
                        table[j][i] = true;
                        qNodes.Enqueue(new Tuple<Node, Node>(nodes[i], nodes[j]));
                    }
                }
            }

            while(qNodes.Count > 0)
            {
                var pair = qNodes.Dequeue();
                foreach(var c in Utils.alphabet)
                {
                    var firstCI = pair.Item1.Start.Inputs.Where(x => x.Subj == c);
                    var secondCI = pair.Item2.Start.Inputs.Where(x => x.Subj == c);
                    foreach(var fCInput in firstCI)
                    {
                        int fInd = indexes[fCInput.Start.Id];
                        foreach (var sCInput in secondCI)
                        {
                            int sInd = indexes[sCInput.Start.Id];
                            if(!table[fInd][sInd])
                            {
                                table[fInd][sInd] = true;
                                table[sInd][fInd] = true;
                                qNodes.Enqueue(new Tuple<Node, Node>(fCInput.Start, sCInput.Start));
                            }
                        }
                    }
                }
            }
            return table;
        }

        public static int[] GetEqClasses(bool[][] table)
        {
            int count = table.Length;
            int[] eqClasses = new int[count];
            int classCount = 0;
            for (int i = 0; i < count; i++)
                if (table[0][i])
                    eqClasses[i] = -1;
                else
                    eqClasses[i] = 0;
            for(int i = 1; i < count; i++)
            {
                if(eqClasses[i] == -1)
                {
                    eqClasses[i] = ++classCount;
                    for(int j = i+1; j < count; j++)
                    {
                        if (!table[i][j])
                            eqClasses[j] = classCount;
                    }
                }
            }
            return eqClasses;
        }

        private static Dictionary<string, int> GetClassesDict(List<Node> nodes, int[] eqClasses)
        {
            Dictionary<string, int> classesDict = new Dictionary<string, int>();
            for(int i = 0; i < nodes.Count; i++)
            {
                classesDict.Add(nodes[i].Id, eqClasses[i]);
            }
            return classesDict;
        }

        
        public static Node GetMKAGraph(List<Node> nodes, Dictionary<string, int> classesDict)
        {
            Dictionary<int, Node> eqClassNodes = new Dictionary<int, Node>();
            for(int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                int eqClass = classesDict[node.Id];
                if (!eqClassNodes.ContainsKey(eqClass))
                {
                    eqClassNodes.Add(eqClass, node);
                }
                else
                {
                    eqClassNodes[eqClass] = MergeNodes(eqClassNodes[eqClass], node);
                }
            }
            var init = eqClassNodes.Where(x => x.Value.Id == "S").FirstOrDefault().Value;
            return init;
        }

        public static Node MergeNodes(Node first, Node second)
        {
            Node merged = new Node();
            merged.Id = "NM";
            if(first.Id == "S" || second.Id == "S")
            {
                merged.Id = "S";
            }
            if (first.isFinishNode || second.isFinishNode)
            {
                merged.isFinishNode = true;
            }
            var outputs = first.Outputs.Concat(second.Outputs).ToList();
            HashSet<(Node, char)> outLinks = new HashSet<(Node, char)>();
            foreach(var output in outputs)
            {
                if(output.Destination == first || output.Destination == second)
                {
                    outLinks.Add((merged, output.Subj));
                }
                else
                {
                    outLinks.Add((output.Destination, output.Subj));
                }
                GraphAutomat.UnBind(output);
            }

            var inputs = first.Inputs.Concat(second.Inputs).ToList();
            HashSet<(Node, char)> inLinks = new HashSet<(Node, char)>();
            foreach (var input in inputs)
            {
                if (input.Start == first || input.Start == second)
                {
                    inLinks.Add((merged, input.Subj));
                }
                else
                {
                    inLinks.Add((input.Start, input.Subj));
                }
                GraphAutomat.UnBind(input);
            }

            foreach(var link in outLinks)
            {
                GraphAutomat.Bind(merged, link.Item1, link.Item2);
            }
            foreach (var link in inLinks)
            {
                GraphAutomat.Bind(link.Item1, merged, link.Item2);
            }
            return merged;
        }

        
    }
}

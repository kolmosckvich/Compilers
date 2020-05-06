using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lab1
{
    public class Program
    {
        private static GetStartProcessQuery getStartProcessQuery;
        private static GetProcessStartInfoQuery getProcessStartInfoQuery;
        private static RegisterLayoutPluginCommand registerLayoutPluginCommand;
        private static GraphGeneration wrapper;

        private static int NodeId = 1;

        static void Main(string[] args)
        {
            getStartProcessQuery = new GetStartProcessQuery();
            getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);
            string regExp = "(0+|1+)|101001";
            //string regExp = "(0|1)0*10+(00)";
            string preProcRegExp = RegexpProcessor.PreprocessRegexp(regExp);
            var polska = RegexpProcessor.SortingStation(preProcRegExp);
            Console.WriteLine(new String(polska.ToArray()));
            var NKA = NKAProcessor.GetNKA(polska);
            BuildGraph(NKA, "NKA.png");
            var DKA = DKAProcessor.GetDKA(NKA); 
            BuildGraph(DKA, "DKA.png");
            var MKA = MKAProcessor.GetMKA(DKA); //MOR DAKKA!!!
            Utils.ClearNodesID(MKA);
            BuildGraph(MKA, "MKA.png");
            Model model = new Model(MKA);
            string input = " ";
            while(input != "")
            {
                Console.WriteLine("Input string to check: ");
                input = Console.ReadLine();
                bool res = model.Process(input);
                if (res)
                    Console.WriteLine("Correct string");
                else
                    Console.WriteLine("Incorrect string");
            }
            
        }

        static void BuildGraph(Node init, string filename)
        {
            var tab = GetNodesTable(init);
            var graphStr = BuildGraphString(tab);

            byte[] output = wrapper.GenerateGraph(graphStr, Enums.GraphReturnType.Png);
            using (Image image = Image.FromStream(new MemoryStream(output)))
            {
                image.Save(filename, ImageFormat.Png);
            }
        }

        public static List<NodeData> GetNodesTable(Node init)
        {
            List<NodeData> table = new List<NodeData>();
            NodeId = 1;
            Queue<Node> nodesToLook = new Queue<Node>();
            nodesToLook.Enqueue(init);
            init.Id = "S";
            while (nodesToLook.Count > 0)
            {
                var node = nodesToLook.Dequeue();
                NodeData data = new NodeData();
                data.NodeId = node.Id;
                if (node.Id == "S")
                {
                    data.IsInit = true;
                }
                if (node.Start.Outputs.Count == 0 || node.isFinishNode)
                {
                    data.IsFinish = true;
                    node.isFinishNode = true; //Achtung!!!
                }
                foreach (var link in node.Start.Outputs)
                {
                    if (link.Destination.Id == null)
                    {
                        link.Destination.Id = (NodeId++).ToString();
                        nodesToLook.Enqueue(link.Destination);
                    }
                    NodeLink nl = new NodeLink();
                    nl.Id = link.Destination.Id;
                    nl.Data = link.Subj;
                    data.ids.Add(nl);
                }
                table.Add(data);
            }

            return table;
        }

        public static string BuildGraphString(List<NodeData> tab)
        {
            StringBuilder graph = new StringBuilder($"digraph finite_state_machine {{ node[shape = doublecircle]; ");
            foreach (var t in tab)
            {
                if(t.IsFinish)
                {
                    string output = t.IsInit ? $"S" : $"{t.NodeId}";

                    graph.Append($"{output} ");
                }
            }
            graph.Append($"; node[shape = circle]; ");

            foreach (var t in tab)
            {
                foreach(var link in t.ids)
                {
                    string output = link.Data == Utils.eps ? "eps" : link.Data.ToString();
                    string from = t.IsInit ? $"S" : $"{t.NodeId}";
                    string to = link.Id.ToString();

                    graph.Append($"{from} -> {to} [ label = \" { output }\" ]; ");
                }
            }

            graph.Append("}");

            return graph.ToString();
        }
    }
}

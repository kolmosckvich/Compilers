using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Lab1
{
    public static class GraphProcessor
    {


        public static void BuildGraph(TreeNode root, string filename)
        {
            GetStartProcessQuery getStartProcessQuery = new GetStartProcessQuery();
            GetProcessStartInfoQuery getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            RegisterLayoutPluginCommand registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            GraphGeneration wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);

            var graphStr = BuildGraphString(root);
            //Console.WriteLine(graphStr);
            byte[] output = wrapper.GenerateGraph(graphStr, Enums.GraphReturnType.Png);
            File.WriteAllText($"{filename}.txt", graphStr);
            using (Image image = Image.FromStream(new MemoryStream(output)))
            {
                image.Save($"{filename}.png", ImageFormat.Png);
            }
        }

        public static string BuildGraphString(TreeNode root)
        {
            StringBuilder graph = new StringBuilder("digraph rtree \n{ node[shape = circle];\n");
            Queue<TreeNode> nodesToVisit = new Queue<TreeNode>();
            nodesToVisit.Enqueue(root);
            int id = 0;
            while(nodesToVisit.Count > 0)
            {
                var node = nodesToVisit.Dequeue();
                node.Id = id++;
                string text = node.TextNode.Replace("\"", "\\\"");
                string type = node.TypeNode.Replace("\"", "\\\"");
                string label = $"node = {text}\n" +
                               $"type = {type}\n";
                if(node.MacroType != "")
                {
                    string mark = node.MacroType.Replace("\"", "\\\"");
                    label += $"mark = {mark}\n";
                }
                if(node.NodeValue != "")
                {
                    string value = node.NodeValue.Replace("\"", "\\\"");
                    label += $"value = {value}\n";
                }
                label = label.TrimEnd('\n');

                graph.Append($"n{node.Id} [label= \"{label}\"];\n");

                if(node.Parent != null)
                {
                    graph.Append($"n{node.Parent.Id} -> n{node.Id};\n");
                }

                foreach(var child in node.Childs)
                {
                    nodesToVisit.Enqueue(child);
                }
                
            }
            graph.Append("\n}");
            return graph.ToString();
        }

    }
}

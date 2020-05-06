using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public static class Utils
    {
        public static List<char> alphabet = new List<char> { '1', '0' };
        public const char eps = 'ε';

        public static List<char> operations = new List<char> { '(', ')', '*', '+', '|', '&' };
        public static List<char> brackets = new List<char> { '(', ')' };
        public static List<int> opPriors = new List<int> { 0, 0, 3, 3, 1, 2 };

        public static void ClearNodesID(Node init)
        {
            Queue<Node> qNodes = new Queue<Node>();
            qNodes.Enqueue(init);
            while (qNodes.Count != 0)
            {
                var qnode = qNodes.Dequeue();
                qnode.Id = null;
                foreach (var link in qnode.Start.Outputs)
                {
                    if (link.Destination.Id != null)
                    {
                        qNodes.Enqueue(link.Destination);
                    }
                }
            }
        }
    }
}

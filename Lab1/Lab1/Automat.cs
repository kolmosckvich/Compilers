using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class GraphAutomat : Node
    {
        public GraphAutomat():base()
        {
            Start = new Node();
            Finish = new Node();
        }

        public GraphAutomat(char c) : base()
        {
            Start = new Node();
            Finish = new Node();
            Bind(Start, Finish, c);
        }

        public override void AddInput(Transition link)
        {
            Start.AddInput(link);
        }

        public override void AddDest(Transition link)
        {
            Finish.AddDest(link);
        }

        public static Node GetConcatAuto(Node a, Node b)
        {
            GraphAutomat res = new GraphAutomat();
            ReBindStart(res, a);
            ReBindFinish(res, b);
            Bind(res.Start, a);
            Bind(a, b);
            Bind(b, res.Finish);
            return res;
        }

        public static Node GetChooseAuto(Node a, Node b)
        {
            GraphAutomat res = new GraphAutomat();
            ReBindStart(res, a);
            ReBindStart(res, b);
            ReBindFinish(res, a);
            ReBindFinish(res, b);
            Bind(res.Start, a);
            Bind(res.Start, b);
            Bind(a, res.Finish);
            Bind(b, res.Finish);
            return res;
        }

        public static Node GetStar(Node a)
        {
            GraphAutomat res = new GraphAutomat();
            ReBindStart(res, a);
            ReBindFinish(res, a);
            Bind(res.Start, res.Finish);
            Bind(res.Start, a);
            Bind(a, res.Finish);
            Bind(a, a);
            return res;
        }

        public static Node GetPlus(Node a)
        {
            GraphAutomat res = new GraphAutomat();
            ReBindStart(res, a);
            ReBindFinish(res, a);
            Bind(res.Start, a);
            Bind(a, res.Finish);
            Bind(a, a);
            return res;
        }

        //По возможности перенести в Node
        public static void Bind(Node a, Node b, char c = Utils.eps)
        {
            Transition link = new Transition(a.Finish, b.Start, c);
            a.Finish.AddDest(link);
            b.Start.AddInput(link);
        }

        public static void ReBindStart(Node newStart, Node oldStart)
        {
            foreach (var input in oldStart.Start.Inputs)
            {
                var inStart = input.Start;
                var inSubj = input.Subj;
                Bind(inStart, newStart.Start, inSubj);
                input.Start.Outputs.Remove(input);
            }
            oldStart.Start.Inputs.Clear();
        }

        public static void ReBindFinish(Node newFinish, Node oldFinish)
        {
            foreach (var output in oldFinish.Finish.Outputs)
            {
                var outFinish = output.Destination;
                var outSubj = output.Subj;
                Bind(newFinish, outFinish, outSubj);
                output.Destination.Inputs.Remove(output);
            }
            oldFinish.Finish.Outputs.Clear();
        }

        public static void UnBind(Transition link)
        {
            link.Start.Outputs.Remove(link);
            link.Destination.Inputs.Remove(link);
        }
    }
}

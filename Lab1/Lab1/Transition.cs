using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    public class Transition
    {
        public Transition(Node start, Node finish, char subj)
        {
            Start = start;
            Destination = finish;
            Subj = subj;
        }

        public char Subj;

        public Node Start;
        public Node Destination;
    }
}

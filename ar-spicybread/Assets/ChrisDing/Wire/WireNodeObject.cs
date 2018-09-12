using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConsoleApp1
{
    public class WireNodeObject
    {
        LineRenderer wire;
        GameObject Node1;
        GameObject Node2;

        public WireNodeObject(LineRenderer wire, GameObject Node1, GameObject Node2) {
            this.Wire = wire;
            this.Node11 = Node1;
            this.Node21 = Node2;
        }

        public GameObject Node11
        {
            get
            {
                return Node1;
            }

            set
            {
                Node1 = value;
            }
        }

        public GameObject Node21
        {
            get
            {
                return Node2;
            }

            set
            {
                Node2 = value;
            }
        }

        public LineRenderer Wire
        {
            get
            {
                return wire;
            }

            set
            {
                wire = value;
            }
        }
    }
}

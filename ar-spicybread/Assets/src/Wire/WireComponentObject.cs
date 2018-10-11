using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConsoleApp1
{
    public class WireComponentObject
    {
        GameObject component;
        List<WireNodeObject> wires;

        public WireComponentObject(GameObject component)
        {
            this.Component = component;
            Wires = new List<WireNodeObject>();
        }

        public GameObject Component
        {
            get
            {
                return component;
            }

            set
            {
                component = value;
            }
        }

        public List<WireNodeObject> Wires
        {
            get
            {
                return wires;
            }

            set
            {
                wires = value;
            }
        }
    }
}

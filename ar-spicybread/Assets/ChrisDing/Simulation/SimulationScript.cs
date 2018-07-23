
using ConsoleApp1;
using UnityEngine;
using System;

#if UNITY_WSA && !UNITY_EDITOR
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using SpiceSharp;
#endif

public class SimulationScript : MonoBehaviour {
#if UNITY_WSA && !UNITY_EDITOR
    Circuit ckt;
    Graph graph;
    DC dc;

    // Use this for initialization
    void Start () {
        ckt = new Circuit();
        graph = this.gameObject.GetComponentInChildren<GraphManager>().getGraph();
    }

    void onSimulate()
    {
        AddComponents();
        dc.OnExportSimulationData += (sender, args) =>
        {
            for (int i = 0; i < graph.getSize(); i++)
            {
                foreach (Transform child in graph.Contents[i].Value.transform)
                {
                    if (child.gameObject.name.Contains("Node"))
                    {
                        if (child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID != 0)
                        {
                            double voltageAtNode = args.GetVoltage(child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID.ToString());
                            child.gameObject.GetComponentInChildren<NodeScript>().changeText(Math.Round(voltageAtNode, 2).ToString());
                        }
                    }
                }
            }
        };
        ckt.Validate();
        dc.Run(ckt);
    }

    void AddComponents()
    {
        ckt.Objects.Clear();

        for (int i = 0; i < graph.getSize(); i++)
        {
            string node1 = "999";
            string node2 = "999";

            if (graph.Contents[i].Value.name.Contains("ResistorComponent"))
            {
                double value = 100.0;

                foreach (Transform child in graph.Contents[i].Value.transform)
                {
                    if (child.gameObject.name == "LeftNode")
                    {
                        node1 = child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID.ToString();
                    }
                    else if (child.gameObject.name == "RightNode")
                    {
                        node2 = child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID.ToString();
                    }
                }
                
                ckt.Objects.Add(new Resistor("R" + graph.Contents[i].Name, node1, node2, value));
                Debug.Log("R" + graph.Contents[i].Name + " Node1: " + node1 + " Node2: "  + node2);
            }
            else if (graph.Contents[i].Value.name.Contains("BatteryComponent"))
            {

                double value = 5.0;

                foreach (Transform child in graph.Contents[i].Value.transform)
                {
                    if (child.gameObject.name == "LeftNode")
                    {
                        node1 = child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID.ToString();
                    }
                    else if (child.gameObject.name == "RightNode")
                    {
                        node2 = child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID.ToString();
                    }
                }

                if (value > 0.0)
                {
                    ckt.Objects.Add(new VoltageSource("V" + graph.Contents[i].Name, node1, node2, value));
                    dc = new DC("DC sim", "V" + graph.Contents[i].Name, value - 1, value, 1.0);
                    Debug.Log("V" + graph.Contents[i].Name + " Node1: " + node1 + " Node2: " + node2);
                }
            }
        }
    }
#endif
}

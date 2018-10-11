
using ConsoleApp1;
using UnityEngine;
using SpiceSharp.Components;
using SpiceSharp.Simulations;
using SpiceSharp;
using System;
using Assets.VirtualComponent.Display;
using System.Collections;

public class SimulationScript : MonoBehaviour {
    Circuit ckt;
    Graph graph;
    DC dc;
    private DisplayManager displayManager;

    // Use this for initialization
    void Start () {
        ckt = new Circuit();
        graph = this.gameObject.GetComponentInChildren<GraphManager>().getGraph();
        displayManager = FindObjectOfType<DisplayManager>();
    }

    void onSimulate()
    {
        try
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
        }
        catch (NullReferenceException ne)
        {
            showErrorText();
        }

        try
        {
            ckt.Validate();
        }
        catch (CircuitException ce)
        {
            showErrorText();
        }
        finally
        {
            dc.Run(ckt);
        }

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
                double value = graph.Contents[i].Value.GetComponentInChildren<ResistorScript>().VoltageValue;

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
                    ckt.Objects.Add(new Resistor("R" + graph.Contents[i].Name, node1, node2, value));
                }
            }
            else if (graph.Contents[i].Value.name.Contains("BatteryComponent"))
            {

                double value = graph.Contents[i].Value.GetComponentInChildren<BatteryScript>().VoltageValue;

                foreach (Transform child in graph.Contents[i].Value.transform)
                {
                    Debug.Log(child.gameObject.name);
                    if (child.gameObject.name.Contains("LeftNode"))
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
                }
            }
        }
    }

    private void showErrorText() {
        displayManager.HudTooltip.text = "Please Check circuit integrity";
        displayManager.HudTooltip.color = Color.red;
        StartCoroutine(ExecuteAfterTime(3.0f));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        displayManager.HudTooltip.text = "";
    }
}

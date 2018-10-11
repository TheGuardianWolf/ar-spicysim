using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConsoleApp1;

public class GraphManager : MonoBehaviour {

    Graph circuitGraph;
    private int currentID;
    private int currentNodeID;

	// Use this for initialization
	void Start () {
        currentID = 0;
        currentNodeID = 1;
        circuitGraph = new Graph();
	}

    public Graph getGraph()
    {
        return circuitGraph;
    }

    public void addVertexToGraph(GameObject go)
    {
        if (go.name.Contains("ResistorComponent")) {
            circuitGraph.AddVertex(new Vertex<GameObject>(currentID, go, "R" + currentID.ToString()));
        }
        else if (go.name.Contains("BatteryComponent"))
        {
            circuitGraph.AddVertex(new Vertex<GameObject>(currentID, go, "V" + currentID.ToString()));
        }
        currentID++;
    }
    

    public int getNewNodeID()
    {
        int temp = currentNodeID;
        currentNodeID++;
        return temp;

    }
}

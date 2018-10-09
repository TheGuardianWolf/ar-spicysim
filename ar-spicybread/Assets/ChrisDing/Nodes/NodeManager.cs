using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    private List<GameObject> nodes;

	// Use this for initialization
	void Start () {
        nodes = new List<GameObject>();
	}

    public void addNode(GameObject node)
    {
        nodes.Add(node);
    }

    public void removeNode(GameObject node)
    {
        nodes.Remove(node);
    }

    public List<GameObject> getNodes()
    {
        return nodes;
    }
}

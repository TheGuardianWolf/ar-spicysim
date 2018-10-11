using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConsoleApp1;

public class WireComponentManager : MonoBehaviour {

    private List<WireComponentObject> wireComponentCollection;

    public List<WireComponentObject> WireComponentCollection
    {
        get
        {
            return wireComponentCollection;
        }

        set
        {
            wireComponentCollection = value;
        }
    }

    // Use this for initialization
    void Start () {
        WireComponentCollection = new List<WireComponentObject>();
	}

    public void addComponent(GameObject go)
    {
        wireComponentCollection.Add(new WireComponentObject(go));
    }

    public void moveWire(GameObject go)
    {
        WireComponentObject wco = wireComponentCollection.Find(x => x.Component == go);
        if (wco.Wires.Count > 0)
        {
            foreach (WireNodeObject wire in wco.Wires)
            {
                if (wire.Wire != null)
                {
                    wire.Wire.SetPosition(0, wire.Node11.transform.position);
                    wire.Wire.SetPosition(1, wire.Node21.transform.position);
                }
            }
        }
    }

    public void removeComponent(GameObject go) {
        WireComponentObject wco = wireComponentCollection.Find(x => x.Component == go);
        foreach (WireNodeObject wire in wco.Wires) {
            if (wire.Wire != null)
            {
                Destroy(wire.Wire);
            }
        }
        wireComponentCollection.Remove(wco);
    }

    public void addWire(GameObject go, LineRenderer lr, GameObject node1, GameObject node2) {
        WireComponentObject wco = wireComponentCollection.Find(x => x.Component == go);
        wco.Wires.Add(new WireNodeObject(lr, node1, node2));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConsoleApp1;

public class ResistorSelectorScript : MonoBehaviour {

    public GameObject cursor;
    public GameObject resistorPrefab;
    GraphManager graph;
    TapToPlaceParent SpiceCollectionScript;

	// Use this for initialization
	void Start () {
        Transform SpiceCollection = transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        graph = SpiceCollection.GetComponentInChildren<GraphManager>();
    }

    void OnSelect()
    {
        if (cursor.activeInHierarchy && SpiceCollectionScript.getPlacingMutex())
        {
            createNewResistor();
            SpiceCollectionScript.returnPlacingMutex();
        }

    }

    private GameObject createNewResistor()
    {
        GameObject newResistor = Instantiate(resistorPrefab) as GameObject;
        graph.addVertexToGraph(newResistor);
        newResistor.transform.parent = this.gameObject.transform.parent.transform;
        return newResistor;
    }
}

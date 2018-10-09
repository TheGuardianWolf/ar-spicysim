using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySelectorScript : MonoBehaviour
{
    public GameObject cursor;
    public GameObject batteryPrefab;
    GraphManager graph;
    TapToPlaceParent SpiceCollectionScript;

    // Use this for initialization
    void Start()
    {
        Transform SpiceCollection = this.transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        graph = SpiceCollection.GetComponentInChildren<GraphManager>();
    }

    void OnSelect()
    {
        if (cursor.activeInHierarchy && SpiceCollectionScript.getPlacingMutex())
        {
            //cursor.SetActive(false);
            createNewBattery();
            SpiceCollectionScript.returnPlacingMutex();
        }

    }

    private GameObject createNewBattery()
    {
        GameObject newResistor = Instantiate(batteryPrefab) as GameObject;
        graph.addVertexToGraph(newResistor);
        newResistor.transform.parent = gameObject.transform.parent.transform;
        return newResistor;
    }
}

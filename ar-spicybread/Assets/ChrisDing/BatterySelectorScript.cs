using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySelectorScript : MonoBehaviour
{
    public GameObject cursor;
    public GameObject batteryPrefab;
    TapToPlaceParent SpiceCollectionScript;

    // Use this for initialization
    void Start()
    {
        Transform SpiceCollection = this.transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
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
        newResistor.transform.parent = this.gameObject.transform.parent.transform;
        return newResistor;
    }
}

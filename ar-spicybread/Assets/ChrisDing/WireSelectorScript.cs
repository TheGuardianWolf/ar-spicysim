using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSelectorScript : MonoBehaviour
{
    TapToPlaceParent SpiceCollectionScript;

    // Use this for initialization
    void Start()
    {
        Transform SpiceCollection = this.transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
    }

    void OnSelect()
    {
        if (SpiceCollectionScript.getWiring())
        {
            SpiceCollectionScript.returnPlacingMutex();
            SpiceCollectionScript.clearWiring();
            SpiceCollectionScript.clearFirstWiringSelected();
        }
        else
        {
            if (SpiceCollectionScript.getPlacingMutex())
            {
                SpiceCollectionScript.setWiring();
            }
        }
    }

    private void Update()
    {
        if (SpiceCollectionScript.getWiring())
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }
    }

}

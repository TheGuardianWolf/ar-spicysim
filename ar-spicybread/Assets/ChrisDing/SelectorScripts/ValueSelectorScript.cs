using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSelectorScript : MonoBehaviour {

    TapToPlaceParent SpiceCollectionScript;

    // Use this for initialization
    void Start()
    {
        Transform SpiceCollection = this.transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
    }

    void OnSelect()
    {
        if (SpiceCollectionScript.getValuePlacement())
        {
            SpiceCollectionScript.clearValuePlacement();
        }
        else
        {
            SpiceCollectionScript.setValuePlacement();
        }
    }

    private void Update()
    {
        if (SpiceCollectionScript.getValuePlacement())
        {
            transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        }
    }
}

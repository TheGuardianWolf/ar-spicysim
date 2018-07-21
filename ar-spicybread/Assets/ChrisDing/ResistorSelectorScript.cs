using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistorSelectorScript : MonoBehaviour {

    public GameObject cursor;
    public GameObject resistorPrefab;
    TapToPlaceParent SpiceCollectionScript;

	// Use this for initialization
	void Start () {
        Transform SpiceCollection = this.transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
    }

    void OnSelect()
    {
        if (cursor.activeInHierarchy && SpiceCollectionScript.getPlacingMutex())
        {
            //cursor.SetActive(false);
            createNewResistor();
            SpiceCollectionScript.returnPlacingMutex();
        }

    }

    private GameObject createNewResistor() {
        GameObject newResistor = Instantiate(resistorPrefab) as GameObject;
        newResistor.transform.parent = this.gameObject.transform.parent.transform;
        return newResistor;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGenerateSelectorScript : MonoBehaviour {

    AutoPlacement autoPlacement;

	// Use this for initialization
	void Start () {
        autoPlacement = GetComponentInParent<AutoPlacement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

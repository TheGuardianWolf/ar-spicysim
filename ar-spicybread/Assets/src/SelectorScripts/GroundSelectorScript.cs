using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSelectorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnSelect()
    {
        this.transform.parent.transform.parent.BroadcastMessage("onGround");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

    public GameObject multiplierView;
    public GameObject baseView;

	// Use this for initialization
	void Start () {
		
	}

    public GameObject createMultiplierView()
    {
        GameObject mv = Instantiate(multiplierView);
        mv.transform.SetParent(transform);
        return mv;
    }

    public GameObject createBaseView()
    {
        GameObject bv = Instantiate(baseView);
        bv.transform.SetParent(transform);
        return bv;
    }
}

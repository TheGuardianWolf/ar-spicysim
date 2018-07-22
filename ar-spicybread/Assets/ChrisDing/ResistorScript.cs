using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistorScript : MonoBehaviour {

    private bool placing;
    TapToPlaceParent SpiceCollectionScript;
	// Use this for initialization
	void Start () {
        Vector3 scale = new Vector3(0.017375f, 0.1f, 0.019125f);
        placing = true;
        Transform SpiceCollection = this.transform.parent.transform;
        transform.rotation = SpiceCollection.rotation;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        SpiceCollectionScript.getPlacingMutex();

        //Vector3 rightEnd = new Vector3((transform.position + 0.040f * transform.right).x - 0.005f, (transform.position + 0.040f * transform.right).y + 0.012f, (transform.position + 0.040f * transform.right).z + 0.001f);
        //createWire(rightEnd, rightEnd + new Vector3(rightEnd.x + 0.02f, rightEnd.y, rightEnd.z), lineMaterial, this.transform);
    }

    private void OnSelect()
    {
        if (placing)
        {
            SpiceCollectionScript.returnPlacingMutex();
            placing = false;
        }
        else if (SpiceCollectionScript.getPlacingMutex())
        {
            placing = true;
        }
    }

    void onRotate()
    {
        if (placing)
        {
            this.gameObject.transform.RotateAround(this.gameObject.transform.position, transform.up, 90.0f);
        }
    }

    private void Update()
    {

        if (placing)
        {
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                var directionHead = (headPosition - hitInfo.point);
                this.transform.position = (hitInfo.point + (directionHead * 0.04f));
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Contains("Recycling"))
        {
            SpiceCollectionScript.returnPlacingMutex();
            Destroy(gameObject);
        }
    }
}

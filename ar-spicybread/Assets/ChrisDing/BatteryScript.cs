using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    private bool placing;
    TapToPlaceParent SpiceCollectionScript;

    // Use this for initialization
    void Start()
    {
        placing = true;
        Transform SpiceCollection = this.transform.parent.transform;
        transform.rotation = SpiceCollection.rotation;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        SpiceCollectionScript.getPlacingMutex();
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

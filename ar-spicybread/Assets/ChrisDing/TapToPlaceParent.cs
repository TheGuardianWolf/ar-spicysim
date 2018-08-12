using UnityEngine;

public class TapToPlaceParent : MonoBehaviour
{
    public GameObject cursor;
    public GameObject cursorVisual;
    public GameObject canvas;
    private bool placing = true;
    private bool wiring = false;
    private bool valuePlacement = false;
    private bool firstWiringSelected = false;
    private GameObject firstWiringComponent;
    private Vector3 firstWiringComponentInitialScale;
    private bool ComponentPlacingMutex; //true is being used, false is not
    float scalingFactor = 1.3f;
    bool scaleIncrease = true;

    private void Start()
    {
        //SpatialMapping.Instance.DrawVisualMeshes = true;
        cursor.SetActive(false);
        placing = true;
        ComponentPlacingMutex = false;
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    void OnSelect()
    {
        if (placing)
        {
            // On each Select gesture, toggle whether the user is in placing mode.
            placing = false;
            //SpatialMapping.Instance.DrawVisualMeshes = false;
            gameObject.GetComponentInChildren<AutoPlacement>().test();
            //this.cursor.SetActive(true);
        }
    }

    //Placing
    public bool getPlacingMutex()
    {
        if (!ComponentPlacingMutex && !valuePlacement)
        {
            ComponentPlacingMutex = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void returnPlacingMutex()
    {
        ComponentPlacingMutex = false;
    }

    //Wiring
    public void setWiring()
    {
        if (!valuePlacement)
        {
            wiring = true;
        }
    }

    public void clearWiring()
    {
        wiring = false;
    }

    public bool getWiring()
    {
        return wiring;
    }

    public bool isFirstWiringSelected()
    {
        return firstWiringSelected;
    }

    public void setFirstWiringSelected(GameObject go)
    {
        firstWiringComponent = go;
        firstWiringComponentInitialScale = firstWiringComponent.transform.localScale;
        firstWiringSelected = true;
    }

    public void clearFirstWiringSelected()
    {
        if (firstWiringComponent != null)
        {
            firstWiringComponent.transform.localScale = firstWiringComponentInitialScale;
            firstWiringComponent = null;
        }
        firstWiringSelected = false;
    }

    public GameObject getFirstWiringComponent()
    {
        return firstWiringComponent;
    }

    //value Placement
    public bool setValuePlacement()
    {
        if (!ComponentPlacingMutex)
        {
            valuePlacement = true;
            return true;
        }

        return false;
    }

    public void clearValuePlacement()
    {
        valuePlacement = false;
    }

    public bool getValuePlacement()
    {
        return valuePlacement;
    }

    // Update is called once per frame
    void Update()
    {

        if (placing)
        {
            cursor.SetActive(false);
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMapping.PhysicsRaycastMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.02f, hitInfo.point.z);

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.transform.rotation = toQuat;
            }
        }

        else
        {
            if (!ComponentPlacingMutex)
            {
                cursor.SetActive(true);
            }
            else
            {
                if (wiring)
                {
                    cursor.SetActive(true);
                }
                else
                {
                    cursor.SetActive(false);
                }
            }
        }

        if (firstWiringSelected)
        {
            if (firstWiringComponent.transform.localScale.x > firstWiringComponentInitialScale.x * 2.0f || firstWiringComponent.transform.localScale.x < firstWiringComponentInitialScale.x)
            {
                scaleIncrease = !scaleIncrease;
            }

            if (scaleIncrease)
            {
                firstWiringComponent.transform.localScale = new Vector3(firstWiringComponent.transform.localScale.x + firstWiringComponent.transform.localScale.x * scalingFactor * Time.deltaTime, firstWiringComponent.transform.localScale.y 
                    + firstWiringComponent.transform.localScale.y * scalingFactor * Time.deltaTime, firstWiringComponent.transform.localScale.z + firstWiringComponent.transform.localScale.z * scalingFactor * Time.deltaTime);
            }
            else
            {
                firstWiringComponent.transform.localScale = new Vector3(firstWiringComponent.transform.localScale.x - firstWiringComponent.transform.localScale.x * scalingFactor * Time.deltaTime, firstWiringComponent.transform.localScale.y 
                    - firstWiringComponent.transform.localScale.y * scalingFactor * Time.deltaTime, firstWiringComponent.transform.localScale.z - firstWiringComponent.transform.localScale.z * scalingFactor * Time.deltaTime);
            }
        }
        else
        {
            scaleIncrease = true;
        }

    }
}
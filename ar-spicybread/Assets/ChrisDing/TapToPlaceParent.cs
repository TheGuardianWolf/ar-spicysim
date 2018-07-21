using UnityEngine;

public class TapToPlaceParent : MonoBehaviour
{
    public GameObject cursor;
    public GameObject cursorVisual;
    private bool placing = true;
    private bool wiring = false;
    private bool firstWiringSelected = false;
    private GameObject firstWiringComponent;
    private Quaternion firstWiringComponentInitialRotation;
    private bool ComponentPlacingMutex; //true is being used, false is not

    private void Start()
    {
        SpatialMapping.Instance.DrawVisualMeshes = true;
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
            SpatialMapping.Instance.DrawVisualMeshes = false;
            //this.cursor.SetActive(true);
        }
    }

    public bool getPlacingMutex()
    {
        if (!ComponentPlacingMutex)
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

    public void setWiring()
    {
        wiring = true;
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
        firstWiringComponentInitialRotation = firstWiringComponent.transform.rotation;
        firstWiringSelected = true;
    }

    public void clearFirstWiringSelected()
    {
        firstWiringComponent.transform.rotation = firstWiringComponentInitialRotation;
        firstWiringComponent = null;
        firstWiringSelected = false;
    }

    public GameObject getFirstWiringComponent()
    {
        return firstWiringComponent;
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
            firstWiringComponent.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        }
    }
}
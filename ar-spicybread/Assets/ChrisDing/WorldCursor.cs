using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private GameObject lastNode;
    public GameObject SpiceCollection;
    private TapToPlaceParent SpiceCollectionScript;

    public GameObject LastNode
    {
        get
        {
            return lastNode;
        }

        set
        {
            lastNode = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Grab the mesh renderer that's on the same object as this script.
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = true;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();

    }

    // Update is called once per frame
    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram...
            // Display the cursor mesh.
            meshRenderer.enabled = true;

            // Move thecursor to the point where the raycast hit.
            var directionHead = (headPosition - hitInfo.point);
            this.transform.position = (hitInfo.point + (directionHead * 0.01f));

            // Rotate the cursor to hug the surface of the hologram.
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            if (hitInfo.collider.gameObject.name.Contains("Node"))
            {
                if (SpiceCollectionScript.getWiring())
                {
                    if (SpiceCollectionScript.isFirstWiringSelected())
                    {
                        if (SpiceCollectionScript.getFirstWiringComponent() != null && SpiceCollectionScript.getFirstWiringComponent().transform.parent != gameObject.transform.parent)
                        {
                            if (SpiceCollectionScript.getFirstWiringComponent().GetComponentInChildren<NodeScript>().mergeNet(hitInfo.collider.gameObject))
                            {
                                foreach (Transform child in hitInfo.collider.gameObject.transform.parent)
                                {
                                    if (child.gameObject.name.Contains("Node") && child.gameObject != hitInfo.collider.gameObject)
                                    {
                                        SpiceCollectionScript.clearFirstWiringSelected();
                                        SpiceCollectionScript.setFirstWiringSelected(child.gameObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            meshRenderer.enabled = false;
        }
    }
}
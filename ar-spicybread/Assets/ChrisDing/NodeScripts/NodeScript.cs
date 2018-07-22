using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeScript : MonoBehaviour {

    TapToPlaceParent SpiceCollectionScript;
    GraphManager graph;
    public Material lineMaterial;
    public int attachedNodeID;
    Transform SpiceCollection;
    Text valueText;
    public Text text;
    // Use this for initialization
    void Start () {
        SpiceCollection = transform.parent.transform.parent.transform;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        graph = SpiceCollection.GetComponentInChildren<GraphManager>();
    }

    void OnSelect()
    {
        if (SpiceCollectionScript.getWiring())
        {
            if (SpiceCollectionScript.isFirstWiringSelected())
            {
                if (SpiceCollectionScript.getFirstWiringComponent() != null && SpiceCollectionScript.getFirstWiringComponent().transform.parent != gameObject.transform.parent)
                {
                    createWire(gameObject.transform.position, SpiceCollectionScript.getFirstWiringComponent().transform.position, lineMaterial, this.transform);

                    //same node for both gameobjects
                    int newNode = graph.getNewNodeID();

                    //interatively turn all nodes into same node
                    //Transform[] allChildren = SpiceCollection.GetComponentsInChildren<Transform>();
                    //foreach (Transform child in allChildren)
                    //{
                    //    if (child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID == this.attachedNodeID || child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID == SpiceCollectionScript.getFirstWiringComponent().GetComponentInChildren<NodeScript>().attachedNodeID)
                    //    {
                    //        child.gameObject.GetComponentInChildren<NodeScript>().attachedNodeID = newNode;
                    //    }
                    //}

                    this.attachedNodeID = newNode;
                    SpiceCollectionScript.getFirstWiringComponent().GetComponentInChildren<NodeScript>().attachedNodeID = newNode;

                    SpiceCollectionScript.clearFirstWiringSelected();
                }
                else
                {
                    SpiceCollectionScript.clearFirstWiringSelected();
                }
            }
            else
            {
                SpiceCollectionScript.setFirstWiringSelected(gameObject);
            }
        }
    }

    public void changeText(string textToChange)
    {
        if (valueText == null)
        {
            valueText = Instantiate(text) as Text;
            valueText.color = Color.green;
            valueText.fontSize = 26;
            //place text on top of resistor and face towards camera
            valueText.rectTransform.SetPositionAndRotation(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.04f, this.gameObject.transform.position.z), Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
            valueText.alignment = TextAnchor.LowerCenter;
        }
        valueText.text = textToChange;
    }

    public static LineRenderer createWire(Vector3 node1, Vector3 node2, Material mat, Transform parent)
    {
        Color c1 = Color.green;
        GameObject ob = new GameObject("LineRenderer");
        ob.transform.parent = parent.transform.parent;

        LineRenderer wire = ob.AddComponent<LineRenderer>();
        wire.material = mat;
        wire.startColor = c1;
        wire.endColor = c1;
        wire.widthMultiplier = 0.002f;
        wire.positionCount = 2;
        wire.useWorldSpace = true;


        wire.SetPosition(0, node1);
        wire.SetPosition(1, node2);

        return wire;
    }

    void Update()
    {
        if (valueText != null)
        {
            valueText.rectTransform.SetPositionAndRotation(new Vector3(this.transform.position.x, this.transform.position.y + 0.08f, this.transform.position.z), Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
        }
    }
}

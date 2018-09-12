using ConsoleApp1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryScript : MonoBehaviour
{
    private bool placing;
    private bool valuePlacing;
    private bool selectingValue;
    private GameObject canvas;
    private int voltageValue;
    private float timeSinceRotate;
    TapToPlaceParent SpiceCollectionScript;
    Graph graph;
    NodeManager nodes;
    Text valueText;
    public Text text;

    public int VoltageValue
    {
        get
        {
            return voltageValue;
        }

        set
        {
            voltageValue = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        placing = true;
        selectingValue = false;
        Transform SpiceCollection = this.transform.parent.transform;
        transform.rotation = SpiceCollection.rotation;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        SpiceCollectionScript.getPlacingMutex();
        graph = SpiceCollection.gameObject.GetComponentInChildren<GraphManager>().getGraph();
        nodes = SpiceCollection.gameObject.GetComponentInChildren<NodeManager>();

        canvas = SpiceCollectionScript.canvas;
    }

    private void OnSelect()
    {
        if (!SpiceCollectionScript.getValuePlacement())
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
        else
        {
            if (SpiceCollectionScript.setValuePlacement())
            {
                valuePlacing = true;
                if (!selectingValue)
                {
                    canvas.GetComponentInChildren<CanvasScript>().createBaseView().transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                    selectingValue = true;
                }
            }
        }
    }

    void onRotate()
    {
        if (placing)
        {
            if ((Time.time - timeSinceRotate) > 0.2f)
            {
                this.gameObject.transform.RotateAround(gameObject.transform.position, transform.up, 90.0f);
                timeSinceRotate = Time.time;
            }
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
        valueText.text = textToChange + " V";
    }

    void baseValue(int x)
    {
        if (valuePlacing)
        {
            changeText(x.ToString());
            VoltageValue = x;
            valuePlacing = false;
            selectingValue = false;
        }
    }

            void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Contains("Recycling"))
        {
            SpiceCollectionScript.returnPlacingMutex();
            graph.RemoveByInstanceID(gameObject.GetInstanceID());

            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Contains("LeftNode") || child.gameObject.name.Contains("RightNode"))
                {
                    nodes.removeNode(child.gameObject);
                }
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.name.Contains("RotationSelector"))
        {
            this.onRotate();
        }
    }
}

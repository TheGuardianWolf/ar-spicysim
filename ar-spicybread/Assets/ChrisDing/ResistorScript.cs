using ConsoleApp1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Breadboard;

public class ResistorScript : MonoBehaviour {


    private bool placing;
    private bool valuePlacing;
    private bool firstBase;
    private bool selectingValue;
    private GameObject canvas;
    private Transform SpiceCollection;
    private double voltageValue;
    private TapToPlaceParent SpiceCollectionScript;
    private Graph graph;
    private NodeManager nodes;
    private Text valueText;
    private float timeSinceRotate;
    public Text text;

    public double VoltageValue
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

    public bool Placing
    {
        get
        {
            return placing;
        }

        set
        {
            placing = value;
        }
    }

    // Use this for initialization
    void Start () {
        Vector3 scale = new Vector3(0.017375f, 0.1f, 0.019125f);
        Placing = false;
        firstBase = true;
        valuePlacing = false;
        selectingValue = false;
        SpiceCollection = this.transform.parent.transform;
        transform.rotation = SpiceCollection.rotation;
        SpiceCollectionScript = SpiceCollection.GetComponentInChildren<TapToPlaceParent>();
        Placing = true;
        graph = SpiceCollection.gameObject.GetComponentInChildren<GraphManager>().getGraph();
        nodes = SpiceCollection.gameObject.GetComponentInChildren<NodeManager>();
        SpiceCollection.gameObject.GetComponentInChildren<WireComponentManager>().addComponent(gameObject);

        canvas = SpiceCollectionScript.canvas;
    }

    private void OnSelect()
    {
        if (!SpiceCollectionScript.getValuePlacement())
        {
            if (Placing)
            {
                SpiceCollectionScript.returnPlacingMutex();
                Placing = false;
                SpiceCollection.GetComponentInChildren<WireComponentManager>().moveWire(gameObject);
            }
            else if (SpiceCollectionScript.getPlacingMutex())
            {
                Placing = true;
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
        if (Placing)
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

        if (Placing)
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

        if (valueText != null)
        {
            valueText.rectTransform.SetPositionAndRotation(new Vector3(this.transform.position.x, this.transform.position.y + 0.08f, this.transform.position.z), Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up));
        }
    }

    void baseValue(int x)
    {
        if (valuePlacing)
        {
            if (firstBase)
            {
                changeText(x.ToString());
                VoltageValue = x * 10;
                canvas.GetComponentInChildren<CanvasScript>().createBaseView().transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                firstBase = false;
            }
            else
            {
                VoltageValue += x;
                changeText(VoltageValue.ToString());
                canvas.GetComponentInChildren<CanvasScript>().createMultiplierView().transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                firstBase = true;
            }
        }
    }

    void multiplierValue(double x)
    {
        if (valuePlacing)
        {
            VoltageValue *= x;
            if (VoltageValue / 1000000 > 1)
            {
                changeText((VoltageValue / 1000000).ToString() + "M");
            }
            else if (VoltageValue / 1000 > 1)
            {
                changeText((VoltageValue / 1000).ToString() + "K");
            }

            valuePlacing = false;
            selectingValue = false;
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

        valueText.text = textToChange + " \u2126";
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
            SpiceCollection.gameObject.GetComponentInChildren<WireComponentManager>().removeComponent(gameObject);
            Destroy(valueText);
            Destroy(gameObject);
        }
        else if (collision.gameObject.name.Contains("RotationSelector")) {
            this.onRotate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Breadboard;
using ConsoleApp1;

public class AutoPlacement : MonoBehaviour {

    private List<Item> realComponents;
    private List<Item> sortedComponents;
    private GraphManager graph;
    public GameObject resistorPrefab;

    public List<Item> RealComponents
    {
        get
        {
            return realComponents;
        }

        set
        {
            realComponents = value;
        }
    }

    // Use this for initialization
    void Start () {
        graph = GetComponentInChildren<GraphManager>();
        RealComponents = new List<Item>();
	}

    public void test()
    {
        Item i1 = new Item();
        i1.pos = new Point();
        i1.pos.x = 32;
        i1.pos.y = 4;
        i1.type = ItemType.RESISTOR;
        RealComponents.Add(i1);

        Item i2 = new Item();
        i2.pos = new Point();
        i2.pos.x = 25;
        i2.pos.y = 3;
        i2.type = ItemType.RESISTOR;
        RealComponents.Add(i2);

        Item i3 = new Item();
        i3.pos = new Point();
        i3.pos.x = 32;
        i3.pos.y = 3;
        i3.type = ItemType.RESISTOR;
        RealComponents.Add(i3);
        onGenerate();
    }

    public void onGenerate()
    {
        foreach (Item item in realComponents.ToList())
        {
            if (item.type == ItemType.EMPTY)
            {
                realComponents.Remove(item);
            }
        }

        sortedComponents = RealComponents.OrderBy(o => o.pos.x).ToList();
        foreach (Item item in sortedComponents.ToList())
        {
            int distanceFromBefore = 0;
            int index = sortedComponents.IndexOf(item);
            bool sameAsLast = false;
            int numSame = 0;
            if (index > 0)
            {
                distanceFromBefore = item.pos.x - sortedComponents[index - 1].pos.x;
                if (item.pos.x == sortedComponents[index - 1].pos.x)
                {
                    sameAsLast = true;
                }
                else
                {
                    sameAsLast = false;
                }
            }
            else
            {
                distanceFromBefore = item.pos.x;
            }

            GameObject newResistor = createNewResistor(distanceFromBefore, index, item, sameAsLast, numSame);

            if (sameAsLast)
            {
                numSame++;
            }
        }
    }

    private GameObject createNewResistor(float distanceFromBefore, int index, Item item, bool sameAsLast, int numSame)
    {
        GameObject newResistor = Instantiate(resistorPrefab) as GameObject;
        graph.addVertexToGraph(newResistor);
        newResistor.transform.parent = transform;
        StartCoroutine(LateStart(0.001f, newResistor, distanceFromBefore, index, item, sameAsLast, numSame));
        return newResistor;
    }

    IEnumerator LateStart(float waitTime, GameObject resistor, float distanceFromBefore, int index, Item item, bool sameAsLast, int numSame)
    {
        yield return new WaitForFixedUpdate();
        resistor.GetComponentInChildren<ResistorScript>().Placing = false;
        GetComponentInChildren<TapToPlaceParent>().returnPlacingMutex();
        if (!sameAsLast)
        {
            resistor.transform.localPosition = new Vector3(/*distanceFromBefore * 0.05f + */(index - numSame) * 0.8f - ((sortedComponents.Count() * 0.9f) / 2), 0.024f, 0.2f * item.pos.y - 1);
        }
        else
        {
            resistor.transform.localPosition = new Vector3(/*distanceFromBefore * 0.05f + */(index - 1 - numSame) * 0.8f - ((sortedComponents.Count() * 0.9f) / 2), 0.024f, 0.2f * item.pos.y - 1);
        }
    }

    private Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
}

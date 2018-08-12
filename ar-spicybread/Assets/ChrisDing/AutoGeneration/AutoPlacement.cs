﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Breadboard;
using ConsoleApp1;

public class AutoPlacement : MonoBehaviour {

    private List<Item> realComponents;
    private List<Item> sortedComponents;
    private List<ItemComponentPair> pair;
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
        pair = new List<ItemComponentPair>();
	}

    public void test()
    {
        //Item i1 = new Item();
        //i1.pos = new Point();
        //i1.pos.x = 32;
        //i1.pos.y = 1;
        //i1.type = ItemType.RESISTOR;
        //RealComponents.Add(i1);

        //Item i2 = new Item();
        //i2.pos = new Point();
        //i2.pos.x = 40;
        //i2.pos.y = 2;
        //i2.type = ItemType.RESISTOR;
        //RealComponents.Add(i2);

        //Item i3 = new Item();
        //i3.pos = new Point();
        //i3.pos.x = 36;
        //i3.pos.y = 3;
        //i3.type = ItemType.RESISTOR;
        //RealComponents.Add(i3);

        //Item i4 = new Item();
        //i4.pos = new Point();
        //i4.pos.x = 34;
        //i4.pos.y = 4;
        //i4.type = ItemType.RESISTOR;
        //RealComponents.Add(i4);

        //onGenerate();
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

        sortedComponents = RealComponents.OrderBy(o => o.pos.x).ThenBy(o => o.pos.y).ToList();
        foreach (Item item in sortedComponents.ToList())
        {
            int distanceFromBefore = 0;
            int index = sortedComponents.IndexOf(item);
            bool sameAsLast = false;
            int numSame = 0;
            List<ItemComponentPair> pairFoundParallel = null;
            ItemComponentPair closestComponentParallel = null;
            if (index > 0)
            {
                pairFoundParallel = pair.FindAll(o => o.Item.pos.x == (item.pos.x));
                if (pairFoundParallel.Count > 0)
                {
                    ItemComponentPair closestComponent = pair.Find(i => i.Item.pos.y == pairFoundParallel.Min(o => o.Item.pos.y));
                    sameAsLast = true;
                }
                else
                {
                    sameAsLast = false;
                }

                distanceFromBefore = item.pos.x - sortedComponents[index - 1].pos.x;
            }
            else
            {
                distanceFromBefore = item.pos.x;
            }

            GameObject newResistor = createNewResistor(distanceFromBefore, index, item, sameAsLast, numSame);

            pair.Add(new ItemComponentPair(newResistor, item));

            //see if any components connected in series
            List<ItemComponentPair> pairFoundSeries = pair.FindAll(o => o.Item.pos.x == (item.pos.x - 4));
            if (pairFoundSeries.Count > 0)
            {
                ItemComponentPair closestComponent = pair.Find(i => i.Item.pos.y == pairFoundSeries.Max(o => o.Item.pos.y));
                StartCoroutine(ConnectSeries(closestComponent.Component, newResistor));
            }

            if (sameAsLast)
            {
                StartCoroutine(ConnectParallel(newResistor, closestComponentParallel.Component));
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
            if (distanceFromBefore < 4)
            {
                resistor.transform.localPosition = new Vector3(/*distanceFromBefore * 0.05f + */(index - numSame) * 0.45f - ((sortedComponents.Count() * 0.9f) / 2) - distanceFromBefore * (0.45f / 4), 0.04f, 0.2f * item.pos.y - 1);
            }
            else
            {
                resistor.transform.localPosition = new Vector3(/*distanceFromBefore * 0.05f + */(index - numSame) * 0.45f - ((sortedComponents.Count() * 0.9f) / 2), 0.04f, 0.2f * item.pos.y - 1);
            }
        }
        else
        {
            resistor.transform.localPosition = new Vector3(/*distanceFromBefore * 0.05f + */(index - 1 - numSame) * 0.45f - ((sortedComponents.Count() * 0.9f) / 2), 0.04f, 0.2f * item.pos.y - 1);
        }
    }

    IEnumerator ConnectParallel(GameObject newResistor, GameObject otherResistor)
    {
        yield return new WaitForFixedUpdate();
        foreach (Transform child in newResistor.transform)
        {
            if (child.gameObject.name == "LeftNode")
            {
                foreach (Transform child1 in otherResistor.transform)
                {
                    if (child1.gameObject.name == "LeftNode")
                    {
                        child.GetComponentInChildren<NodeScript>().mergeNet(child1.gameObject);
                    }
                }
            }

            else if (child.gameObject.name == "RightNode")
            {
                foreach (Transform child1 in otherResistor.transform)
                {
                    if (child1.gameObject.name == "RightNode")
                    {
                        child.GetComponentInChildren<NodeScript>().mergeNet(child1.gameObject);
                    }
                }
            }
        }
    }

    IEnumerator ConnectSeries(GameObject leftResistor, GameObject RightResistor)
    {
        yield return new WaitForFixedUpdate();
        foreach (Transform child in leftResistor.transform)
        {
            if (child.gameObject.name == "LeftNode")
            {
                foreach (Transform child1 in RightResistor.transform)
                {
                    if (child1.gameObject.name == "RightNode")
                    {
                        child.GetComponentInChildren<NodeScript>().mergeNet(child1.gameObject);
                    }
                }
            }
        }
    }
}

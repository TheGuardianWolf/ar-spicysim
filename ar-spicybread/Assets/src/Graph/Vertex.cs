using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConsoleApp1
{
    public class Vertex<GameObject>
    {
        private int id;
        private GameObject data;
        private string name;
        private List<Vertex<GameObject>> neighbors = null;
        private List<int> neighboringNodes = null;

        public Vertex(int id) 
        {
            this.id = id;
            this.neighbors = new List<Vertex<GameObject>>();
        }

        //should only use this one xD
        public Vertex(int id, GameObject data, string name)
        {
            this.id = id;
            this.data = data;
            this.name = name;
            this.neighbors = new List<Vertex<GameObject>>();
            this.neighboringNodes = new List<int>();
        }

        public Vertex(int id, GameObject data, List<Vertex<GameObject>> neighbors)
        {
            this.id = id;
            this.data = data;
            this.neighbors = neighbors;
        }

        public GameObject Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public List<Vertex<GameObject>> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }

        public List<int> NeighboringNodes
        {
            get
            {
                return neighboringNodes;
            }
            set
            {
                neighboringNodes = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConsoleApp1
{
    public class Graph
    {
        private List<Vertex<GameObject>> content;
        private int currentVertex;
        private int previousVertex;
        private int previousPreviousVertex;

        public Graph()
        {
            content = new List<Vertex<GameObject>>();
        }

        public void AddVertex(Vertex<GameObject> v)
        {
            content.Add(v);
            SetCurrent(v.Id);
            if (getSize() > 1)
            {
                GetVertexById(GetCurrent()).Neighbors.Add(GetVertexById(GetPrevious()));
                GetVertexById(GetPrevious()).Neighbors.Add(GetVertexById(GetCurrent()));
            }
        }

        public void RemoveVertex(Vertex<GameObject> v)
        {
            GetVertexById(GetPrevious()).Neighbors.Remove(GetVertexById(GetCurrent()));
            content.Remove(v);
            UnityEngine.Object.Destroy(v.Value);
            SetCurrent(GetPrevious());
        }

        public Vertex<GameObject> GetVertexById(int id)
        {
            return content.Find(x => x.Id == id);
        }

        public int GetIdByInstanceID(int o)
        {
            for (int i = 0; i < this.content.Count; i++)
            {
                if (content[i].Value.GetInstanceID() == o) {
                    return content[i].Id;
                }

            }
            return -1;
        }

        public Vertex<GameObject> GetByInstanceID(int o)
        {
            return GetVertexById(GetIdByInstanceID(o));
        }

        public void RemoveByInstanceID(int o)
        {
            content.Remove(GetVertexById(GetIdByInstanceID(o)));
        }

        public int GetCurrent()
        {
            return currentVertex;
        }

        public int GetPrevious()
        {
            return previousVertex;
        }

        public int GetPreviousPrevious()
        {
            return previousPreviousVertex;
        }

        public void SetCurrent(int id)
        {
            if (currentVertex != id)
            {
                if (this.content.Count > 2)
                {
                    this.previousPreviousVertex = this.previousVertex;
                }
                if (this.content.Count > 1)
                {
                    this.previousVertex = this.currentVertex;
                }
                this.currentVertex = id;
            }
        }

        public void SetCurrentNotPrevious(int id)
        {
            this.currentVertex = id;
        }

        public int getSize()
        {
            return this.content.Count;
        }

        public List<Vertex<GameObject>> Contents
        {
            get
            {
                return content;
            }
        }

        public void recursiveSet(Vertex<GameObject> v) {
            if (GetVertexById(GetPrevious()).Value.name.Contains("Node"))
            {
                foreach (Vertex<GameObject> x in v.Neighbors)
                {
                    if (x.Value.name.Contains("Node") && x.Name != v.Name)
                    {
                        x.Name = v.Name;
                        recursiveSet(x);
                    }
                }
            }
        }
    }
}
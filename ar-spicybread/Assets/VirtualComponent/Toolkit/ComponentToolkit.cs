using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualComponent 
{
    public class ComponentToolkit : MonoBehaviour
    {
        private ComponentTool activeTool;

        public ComponentTool ActiveTool
        {
            get
            {
                return activeTool;
            }

            set
            {

                activeTool = value;
            }
        }

        void Start()
        {
            GazeGestureManager.OnTappedEvent += OnTapped;
        }

        void OnTapped(GameObject focusedObject)
        {
            if (activeTool == null)
            {
                var componentTool = focusedObject.GetComponentInChildren<ComponentTool>();
                if (componentTool != null)
                {
                    componentTool.ComponentToolSelect();
                } 
            }
            else
            {
                activeTool.ComponentToolPlace();
            }
        }
    }
}


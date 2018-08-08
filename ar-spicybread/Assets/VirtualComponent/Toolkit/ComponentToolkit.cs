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
    }
}


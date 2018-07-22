using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualComponent 
{
    public abstract class Placer
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


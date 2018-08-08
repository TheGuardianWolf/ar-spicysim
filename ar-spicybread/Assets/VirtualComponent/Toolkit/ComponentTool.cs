using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VirtualComponent
{
    public abstract class ComponentTool : MonoBehaviour
    {
        public abstract void ComponentToolSelect();
        public abstract void ComponentToolPlace();
    }
}
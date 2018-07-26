using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Breadboard;

namespace ConsoleApp1
{
    public class ItemComponentPair
    {
        private GameObject component;
        private Item item;
        public ItemComponentPair(GameObject component, Item item)
        {
            this.Component = component;
            this.Item = item;
        }

        public Item Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        public GameObject Component
        {
            get
            {
                return component;
            }

            set
            {
                component = value;
            }
        }

        public bool Equals(Item item)
        {
            if (this.item.pos.x == item.pos.x && this.item.pos.y == item.pos.y)
            {
                return true;
            }

            return false;
        }
    }
}

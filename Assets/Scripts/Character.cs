using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public struct Inventory
    {
        public int wood;
    }

    public Inventory inventory;
    public bool meleeing = false;
}

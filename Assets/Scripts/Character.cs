﻿using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public struct InventoryStruct
    {
        public int Wood;
    }

    public InventoryStruct Inventory;
    public bool Meleeing = false;
    public float Speed = 30.0f;
}

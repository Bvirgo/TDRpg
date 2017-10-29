using UnityEngine;
using System.Collections;
using System;

public class InventoryItem  {

    public InventoryItem()
    {
        guid = Guid.NewGuid().ToString();
    }
    public string guid;
    private Inventory inventory;
    private int level;
    private int count;
    private bool isDressed = false;

    public Inventory Inventory {
        get {
            return inventory;
        }
        set {
            inventory = value;
        }
    }
    public int Level {
        get {
            return level;
        }
        set {
            level = value;
        }
    }
    public int Count {
        get {
            return count;
        }
        set {
            count = value;
        }
    }
    public bool IsDressed {
        get {
            return isDressed;
        }
        set {
            isDressed = value;
        }
    }

}

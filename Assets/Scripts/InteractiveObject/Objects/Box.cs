using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractiveObject
{
    public List<Item> getItem;
    public string dialogFileName;
    public bool onlyItem;

    EquipmentPanelManager equipmentPanelManager;
    Dialog dialog;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        dialog = FindAnyObjectByType<Dialog>();

        if (onlyItem)
        {
            bool hasAll = true;
            foreach (Item item in getItem)
            {
                if (!equipmentPanelManager.HasItem(item))
                {
                    hasAll = false;
                }
            }
            triggered = hasAll;
        }
    }

    public void GetItem()
    {
        foreach (Item item in getItem)
        {
            if (!(onlyItem && equipmentPanelManager.HasItem(item)))
            {
                equipmentPanelManager.AddItem(item);
            }
        }
        dialog.Read(dialogFileName);
    }
}

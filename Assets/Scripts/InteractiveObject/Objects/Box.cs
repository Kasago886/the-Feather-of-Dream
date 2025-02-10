using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractiveObject
{
    public Item getItem;
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

        if (onlyItem && equipmentPanelManager.HasItem(getItem))
        {
            triggered = true;
        }
    }

    public void GetItem()
    {
        equipmentPanelManager.AddItem(getItem);
        dialog.Read(dialogFileName);
    }
}

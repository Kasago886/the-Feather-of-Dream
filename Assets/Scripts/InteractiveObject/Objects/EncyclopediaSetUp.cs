using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaSetUp : MonoBehaviour
{
    public Item playerEnc;
    public Item assistantEnc;

    EquipmentPanelManager equipmentPanelManager;

    private void Start()
    {
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
    }

    public void AddEncyclopedia()
    {
        //Debug.Log("add");
        if (equipmentPanelManager == null)
        {
            equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        }

        equipmentPanelManager.AddEncyclopedia(playerEnc);
        equipmentPanelManager.AddEncyclopedia(assistantEnc);
    }
}

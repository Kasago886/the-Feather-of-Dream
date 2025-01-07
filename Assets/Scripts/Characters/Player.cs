using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    ArchiveManager archiveManager;
    EquipmentPanelManager equipmentPanelManager;

    new private void Start()
    {
        base.Start();
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();

        tenacity = archiveManager.currentArchive.playerInfo.tenacity;
        strength = archiveManager.currentArchive.playerInfo.strength;
    }

    public void AddDream(int num)
    {
        archiveManager.currentArchive.playerInfo.dream += num;

        equipmentPanelManager.SetUpPlayerInfo();
    }
}

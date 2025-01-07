using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    ArchiveManager archiveManager;
    EquipmentPanelManager equipmentPanelManager;
    public Scroll hpScroll;

    new private void Start()
    {
        base.Start();
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();

        tenacity = archiveManager.currentArchive.playerInfo.tenacity;
        strength = archiveManager.currentArchive.playerInfo.strength;
    }

    /// <summary>
    /// ÃÌº”√Œ
    /// </summary>
    /// <param name="num"></param>
    public void AddDream(int num)
    {
        archiveManager.currentArchive.playerInfo.dream += num;

        equipmentPanelManager.SetUpPlayerInfo();
    }

    public override void ShowUnlockFeather(Feather feather)
    {
        base.ShowUnlockFeather(feather);

        HpUI hpUI = hpScroll.AddHp();

        hpUI.testTime = feather.lockTimer;
        hpUI.testHp = feather.health;
        hpUI.testHpMax = feather.maxHealth;
        
        feather.hpUI = hpUI;
    }

}

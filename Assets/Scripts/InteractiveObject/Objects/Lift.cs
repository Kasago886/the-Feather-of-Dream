using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : InteractiveObject
{
    [Header("电梯：")]
    public int targetLevelIndex = 0;
    public int targetArchivePoint = 0;
    public bool Lock;
    public string lockedDialog;
    public Item[] requiredItems;

    //public string targetSceneName = "";
    //public int targetSceneIndex;
    ExitPanelManager exitPanelManager;
    ArchiveManager archiveManager;
    EquipmentPanelManager equipmentPanelManager;
    Dialog dialog;
    protected override void Start()
    {
        base.Start();
        exitPanelManager = FindObjectOfType<ExitPanelManager>();
        archiveManager = FindObjectOfType<ArchiveManager>();
        equipmentPanelManager = FindObjectOfType<EquipmentPanelManager>();
        dialog = FindObjectOfType<Dialog>();
    }

    public override void Interact()
    {
        base.Interact();

        //拥有所有需要的物品才解锁
        if (Lock)
        {
            foreach (var item in requiredItems)
            {
                if (item != null)
                {
                    if (equipmentPanelManager.HasItem(item))
                    {
                        Lock = false;
                    }
                    else
                    {
                        Lock = true;
                        break;
                    }
                }
            }
        }

        if (!Lock)
        {
            //保存
            archiveManager.SaveCurrentArchive(level: targetLevelIndex, archivePoint: targetArchivePoint); 
            
            //跳转
            exitPanelManager.LoadScene("level"+targetLevelIndex);

            /*
            if (targetSceneName != "")
            {
                exitPanelManager.LoadScene(targetSceneName);
            }
            else if (targetSceneIndex != -1)
            {
                exitPanelManager.LoadScene(targetSceneIndex);
            }*/
        }
        else
        {
            if (lockedDialog != "" && dialog.ifPause)
            {
                dialog.Read(lockedDialog);
            }
        }
    }

    public void SetLocked(bool locked)
    {
        this.Lock = locked;
    }
}
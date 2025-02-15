using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamBottle : InteractiveObject {

    public FlagType type;
    private Player player_;
    Dialog dialog;

    bool used = false;
    protected override void Start()
    {
        base.Start();
        player_ = FindObjectOfType<Player>(); 
        dialog = FindObjectOfType<Dialog>();

        if (ArchiveManager.CheckFlag(type))
        {
            used = true;
            GetComponent<Animator>().Play("usedBottleNoAnimation");
        }
    }

    public override void Interact()
    {
        if (!used)
        {
            base.Interact();
            used = true;
            ArchiveManager.CheckFlag(type,true);

            if (player_ != null)
            {
                player_.AddDream(1);
            }
            else
            {
                ArchiveManager archiveManager = FindAnyObjectByType<ArchiveManager>();
                archiveManager.currentArchive.playerInfo.dream += 1;
                EquipmentPanelManager equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
                equipmentPanelManager.SetUpPlayerInfo();
            }

            GetComponent<Animator>().Play("usedBottle");

            dialog.Read("GetDream");
        }
    }
}

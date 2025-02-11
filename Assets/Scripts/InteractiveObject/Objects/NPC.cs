using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum NPCState
{
    LittleRedRidingHood
}

public class NPC : InteractiveObject
{
    public NPCState state;

    public string dialogFileName;
    public UnityEvent endEvent;
    public Item demandItem;
    public string gotThingDialogFileName;
    public UnityEvent endEvent2;
    public string dialogFileName3;

    bool gotThing = false;
    Dialog dialog;
    EquipmentPanelManager equipmentPanelManager;
    ArchiveManager archiveManager;
    protected override void Start()
    {
        base.Start();
        dialog = FindAnyObjectByType<Dialog>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        archiveManager = FindAnyObjectByType<ArchiveManager>();

        CheckState();

        if (gotThing)
        {
            endEvent2?.Invoke();
        }
    }
    public override void Interact()
    {
        base.Interact();
        if (dialog.ifPause)
        {
            if (!gotThing)
            {
                if (demandItem != null)
                {
                    if (equipmentPanelManager.RemoveItem(demandItem))
                    {
                        gotThing = true;
                        SetState();

                        dialog.Read(gotThingDialogFileName, endEvent2);
                    }
                }

                if (!gotThing)
                {
                    dialog.Read(dialogFileName, endEvent);
                }
            }
            else
            {
                dialog.Read(dialogFileName3);
            }
        }
    }

    void CheckState()
    {
        switch (state)
        {
            case NPCState.LittleRedRidingHood:
                gotThing = archiveManager.currentArchive.levelInfo.littleRedRidingHood;
                break;
        }
    }

    void SetState()
    {
        switch (state)
        {
            case NPCState.LittleRedRidingHood:
                archiveManager.currentArchive.levelInfo.littleRedRidingHood = gotThing;
                break;
        }
    }
}

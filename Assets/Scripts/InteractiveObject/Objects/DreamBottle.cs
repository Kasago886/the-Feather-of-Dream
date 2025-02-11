using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamBottle : InteractiveObject {

    public FlagType type;
    private Player player_;

    bool used = false;
    protected override void Start()
    {
        base.Start();
        player_ = FindObjectOfType<Player>(); 
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
            player_.AddDream(1);
            GetComponent<Animator>().Play("usedBottle");
        }
    }
}

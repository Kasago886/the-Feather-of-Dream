using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArchiveSaver : InteractiveObject 
{
    SalManager salManager;
    protected override void Start()
    {
        base.Start();

        salManager = FindAnyObjectByType<SalManager>();
    }
    public override void Interact()
    {
        base.Interact();

        salManager.ShowSavePanel();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    ArchiveManager archiveManager;
    new private void Start()
    {
        base.Start();
        archiveManager = FindAnyObjectByType<ArchiveManager>();

        tenacity = archiveManager.currentArchive.playerInfo.tenacity;
        strength = archiveManager.currentArchive.playerInfo.strength;
    }
}

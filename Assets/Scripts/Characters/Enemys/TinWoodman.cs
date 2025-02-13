using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinWoodman : Enemy
{
    public void SetTutorialDone()
    {
        ArchiveManager.CheckFlag(FlagType.tutorialDone,true,true);
    }
}

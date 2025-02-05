using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Lift : InteractiveObject
{
    [Header("µçÌÝ£º")]
    public bool Lock;
    public int targetLevelIndex = 0;
    public int targetArchivePoint = 0;
    //public string targetSceneName = "";
    //public int targetSceneIndex;
    ExitPanelManager exitPanelManager;
    ArchiveManager archiveManager;
    protected override void Start()
    {
        base.Start();
        exitPanelManager = FindObjectOfType<ExitPanelManager>();
        archiveManager = FindObjectOfType<ArchiveManager>();
    }

    public override void Interact()
    {
        base.Interact();
        if (!Lock)
        {
            //±£´æ
            archiveManager.SaveCurrentArchive(level: targetLevelIndex, archivePoint: targetArchivePoint); 
            
            //Ìø×ª
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
    }
}
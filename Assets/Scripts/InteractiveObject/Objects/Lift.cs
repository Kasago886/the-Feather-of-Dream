using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Lift : InteractiveObject
{
    [Header("µçÌÝ£º")]
    public bool Lock;
    public string targetSceneName = "";
    public int targetSceneIndex;
    ExitPanelManager exitPanelManager;
    protected override void Start()
    {
        base.Start();
        exitPanelManager = FindObjectOfType<ExitPanelManager>();
    }

    public override void Interact()
    {
        base.Interact();
        if (!Lock)
        {
            if (targetSceneName != "")
            {
                exitPanelManager.LoadScene(targetSceneName);
            }
            else if (targetSceneIndex != -1)
            {
                exitPanelManager.LoadScene(targetSceneIndex);
            }
        }
    }
}
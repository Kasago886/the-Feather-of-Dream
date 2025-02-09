using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnsureButtonScript : MonoBehaviour
{
    ExitPanelManager exitPanelManager;
    // Start is called before the first frame update
    void Start()
    {
        exitPanelManager = FindAnyObjectByType<ExitPanelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        exitPanelManager.LoadScene(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanelManager : MonoBehaviour
{
    public Animator exitPanelAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        exitPanelAnimator.GetComponent<ExitPanel>().SetTargetSceneName(sceneName);
        ExitPanelStart();
    }
    public void LoadScene(int index)
    {
        exitPanelAnimator.GetComponent<ExitPanel>().SetTargetSceneIndex(index);
        ExitPanelStart();
    }

    public void ExitPanelStart()
    {
        Time.timeScale = 1.0f;

        exitPanelAnimator.gameObject.SetActive(true);
        exitPanelAnimator.Play("ExitPanel");
    }
}

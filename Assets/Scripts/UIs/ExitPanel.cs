using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPanel : MonoBehaviour
{
    public string scenename = "";
    public int sceneindex = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetSceneName(string name)
    {
        scenename = name;
        sceneindex = -1;
    }

    public void load()
    {
        if (scenename != "")
        {
            SceneManager.LoadScene(scenename);
        }
        else if (sceneindex != -1)
        {
            SceneManager.LoadScene(sceneindex);
        }
    }

    public void SetTargetSceneIndex(int index)
    {
        scenename = "";
        sceneindex = index;
    }
}

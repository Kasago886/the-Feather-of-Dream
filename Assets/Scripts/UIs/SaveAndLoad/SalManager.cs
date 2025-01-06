using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalManager : MonoBehaviour
{
    public Button readButton;
    public Button deleteButton;
    public Button ensureDeleteButton;

    public loadScript chosenLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Ñ¡ÖÐ´æµµ
    /// </summary>
    /// <param name="chosenLoad"></param>
    /// <param name="isNull"></param>
    public void SetChosenLoad(loadScript chosenLoad, bool isNull)
    {
        this.chosenLoad = chosenLoad;

        readButton.onClick.RemoveAllListeners();
        ensureDeleteButton.onClick.RemoveAllListeners();

        readButton.onClick.AddListener(ReadChosenLoad);
        ensureDeleteButton.onClick.AddListener(DeleteChosenLoad);

        readButton.interactable = true;
        if (!isNull)
        {
            deleteButton.interactable = true;
        }
        else
        {
            deleteButton.interactable = false;
        }
    }

    /// <summary>
    /// ¶ÁÈ¡´æµµ
    /// </summary>
    public void ReadChosenLoad()
    {
        chosenLoad.LoadOrNew();
    }

    /// <summary>
    /// É¾³ý´æµµ
    /// </summary>
    public void DeleteChosenLoad()
    {
        chosenLoad.Delete();
    }
}

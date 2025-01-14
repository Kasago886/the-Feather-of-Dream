using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalManager : MonoBehaviour
{
    public Button readButton;
    public Button deleteButton;
    public Button ensureDeleteButton;

    public Button saveButton;
    public GameObject savePanel;
    public Button ensureRewriteButton;
    public GameObject ensureRewritePanel;

    loadScript chosenLoad;
    saveScript chosenSave;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 选中存档
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
    /// 读取存档
    /// </summary>
    public void ReadChosenLoad()
    {
        chosenLoad.LoadOrNew();
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    public void DeleteChosenLoad()
    {
        chosenLoad.Delete();
    }

    /// <summary>
    /// 显示保存界面
    /// </summary>
    public void ShowSavePanel()
    {
        savePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 关闭保存界面
    /// </summary>
    public void CloseSavePanel()
    {
        savePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// 选中存档
    /// </summary>
    /// <param name="chosenSave"></param>
    /// <param name="isNull"></param>
    public void SetChosenSave(saveScript chosenSave, bool isNull)
    {
        this.chosenSave = chosenSave;

        saveButton.onClick.RemoveAllListeners();
        ensureRewriteButton.onClick.RemoveAllListeners();

        saveButton.onClick.AddListener(SaveButtonClicked);
        ensureRewriteButton.onClick.AddListener(SaveOrRewrite);

        saveButton.interactable = true;
    }

    /// <summary>
    /// 点击保存按钮
    /// </summary>
    public void SaveButtonClicked()
    {
        if (chosenSave.archive == null)
        {
            SaveOrRewrite();
        }
        else
        {
            ensureRewritePanel.SetActive(true);
        }
    }

    /// <summary>
    /// 保存或覆盖存档
    /// </summary>
    public void SaveOrRewrite()
    {
        chosenSave.SaveOrRewrite();
    }
}

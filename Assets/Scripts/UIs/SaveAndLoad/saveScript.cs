using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class saveScript : MonoBehaviour
{
    static string path = Application.dataPath + "/Archives/ArchiveScreenShot";

    public Text text;
    public Image image;

    int index = 0;
    public Archive archive = null;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void setUp(int indexnum, Archive archiveInfo)
    {
        index = indexnum;
        archive = archiveInfo;

        string str = string.Format("{0:D3}  ----/--/--  --:--:--", index);
        str = str + "\nNew Archive";

        text.text = str;

        if (archive != null)
        {
            //获取data
            TimeInfo timeInfo = archive.timeInfo;
            LevelInfo levelInfo = archive.levelInfo;
            PlayerInfo playerInfo = archive.playerInfo;

            try
            {
                image.overrideSprite = ArchiveManager.GetSprite(path + "/" + index + ".jpg");
            }
            catch
            {
                Debug.LogWarning("Image loading failed!\nImageName: " + path + "/" + index + ".jpg");
            }

            str = string.Format("{0:D3}  {1:D4}/{2:D2}/{3:D2}  {4:D2}:{5:D2}:{6:D2}\n", index, timeInfo.year, timeInfo.month, timeInfo.day, timeInfo.hour, timeInfo.minute, timeInfo.second);
            str = str + levelInfo.title;
            str = str + "\nLv." + playerInfo.level + "   " + playerInfo.currentExp +"/"+playerInfo.maxExp;

            text.text = str;
        }
        else
        {
            //gameObject.GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// 点击时将自身信息传给salManager
    /// </summary>
    public void OnClick()
    {
        bool isNull = (archive == null);

        SalManager salManager = FindAnyObjectByType<SalManager>();
        salManager.SetChosenSave(this, isNull);

        ///设置可点击状态
        Button[] childButtons = transform.parent.GetComponentsInChildren<Button>();
        foreach (Button childButton in childButtons)
        {
            childButton.interactable = true;
        }
        button.interactable = false;
    }

    /// <summary>
    /// 保存或覆盖存档
    /// </summary>
    public void SaveOrRewrite()
    {
        //保存完成后刷新
        saveListCreateScript saveListCreateScript = GetComponentInParent<saveListCreateScript>();
        Action finishAction = saveListCreateScript.reSaveList;

        //保存
        ArchiveManager archiveManager = FindAnyObjectByType<ArchiveManager>();
        archiveManager.SaveCurrentArchive(index, finishAction);

        PlayerPrefs.SetInt(Consts.CurrentArchivePlayerPrefTag, index);
        PlayerPrefs.Save();
    }
}

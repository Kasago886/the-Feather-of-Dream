using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadScript : MonoBehaviour
{
    static string path = Application.dataPath + "/Archives/ArchiveScreenShot";

    public Text text;
    public Image image;
    public Item ElliesFeather;

    int index = 0;
    Archive archive = null;
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
            image.overrideSprite = null;
        }
    }

    /// <summary>
    /// 点击时将自身信息传给salManager
    /// </summary>
    public void OnClick()
    {
        bool isNull = (archive == null);

        SalManager salManager = FindAnyObjectByType<SalManager>();
        salManager.SetChosenLoad(this, isNull);

        ///设置可点击状态
        Button[] childButtons = transform.parent.GetComponentsInChildren<Button>();
        foreach (Button childButton in childButtons)
        {
            childButton.interactable = true;
        }
        button.interactable = false;
    }

    /// <summary>
    /// 读取或新建存档
    /// </summary>
    public void LoadOrNew()
    {
        if (archive != null)
        {
            PlayerPrefs.SetInt(Consts.CurrentArchivePlayerPrefTag, index);
            PlayerPrefs.Save();

            int level = archive.levelInfo.level;
            ExitPanelManager exitPanelManager = FindAnyObjectByType<ExitPanelManager>();
            exitPanelManager.LoadScene("Level" + level.ToString());
        }
        else
        {
            //新建存档
            ArchiveManager archiveManager = FindAnyObjectByType<ArchiveManager>();
            Archive newArchive = new Archive();
            newArchive.levelInfo.level = 0;

            List<ItemInfo> equipedFeather = new List<ItemInfo>();
            equipedFeather.Add(ElliesFeather.GetItemInfo());
            newArchive.equipedFeather.items = equipedFeather.ToArray();
            newArchive.equipedBrokenFeather.items = new List<ItemInfo>().ToArray();
            newArchive.items.items = new List<ItemInfo>().ToArray();
            newArchive.encyclopedia.items = new List<ItemInfo>().ToArray();

            archiveManager.currentArchive = newArchive;
            archiveManager.SaveCurrentArchive(index);

            PlayerPrefs.SetInt(Consts.CurrentArchivePlayerPrefTag, index);
            PlayerPrefs.Save();

            ExitPanelManager exitPanelManager = FindAnyObjectByType<ExitPanelManager>();
            exitPanelManager.LoadScene("Level0");
        }
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    public void Delete()
    {
        ArchiveManager.DeleteArchive(index);
        setUp(index, null);
        OnClick();
    }
}

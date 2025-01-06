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

    int index;
    Archive archive;

    public void setUp(int indexnum, Archive archiveInfo)
    {
        index = indexnum;
        archive = archiveInfo;

        text.text = string.Format("{0:D3}  ----/--/--  --:--:--", index);

        if (archive != null)
        {
            //获取data
            TimeInfo timeInfo = archive.timeInfo;
            LevelInfo levelInfo = archive.levelInfo;

            image.overrideSprite = ArchiveManager.GetSprite(path + "/" + levelInfo.imageName + ".jpg");

            text.text = string.Format("{0:D3}  {1:D4}/{2:D2}/{3:D2}  {4:D2}:{5:D2}:{6:D2}\n"+levelInfo.title, index, timeInfo.year, timeInfo.month, timeInfo.day, timeInfo.hour, timeInfo.minute, timeInfo.second);
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void clickEvent()
    {
    }
}

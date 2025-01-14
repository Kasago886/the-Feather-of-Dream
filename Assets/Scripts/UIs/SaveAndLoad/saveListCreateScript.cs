using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveListCreateScript : MonoBehaviour
{
    public GameObject save;
    void Awake()
    {
        reSaveList();
    }

    public void reSaveList()
    {
        //删除子物体
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Archive[] archives = ArchiveManager.GetAllArchive();
        int archiveIndex = 0;
        int loadIndex = 0;
        while (archiveIndex < archives.Length)
        {
            //创建实例
            GameObject instance = Instantiate(save,Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(transform,false);

            saveScript script = instance.GetComponent<saveScript>();
            if (archives[archiveIndex].index == loadIndex)
            {
                script.setUp(loadIndex, archives[archiveIndex]);
                archiveIndex++;
            }
            else if (archives[archiveIndex].index < loadIndex)
            {
                script.setUp(loadIndex, null);

                script = transform.GetChild(archives[archiveIndex].index).GetComponent<saveScript>();
                script.setUp(loadIndex, archives[archiveIndex]);
                archiveIndex++;
            }
            else
            {
                script.setUp(loadIndex, null);
            }

            loadIndex++;
            if (loadIndex > 100)
            {
                Debug.LogWarning("Too Many Archives! Archives whose index is above 100 are hided!");
                break;
            }
        }
        //空存档
        GameObject ins = Instantiate(save, Vector3.zero, Quaternion.identity);
        ins.transform.SetParent(transform, false);
        saveScript scr = ins.GetComponent<saveScript>();
        scr.setUp(loadIndex, null);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadListCreateScript : MonoBehaviour
{
    public GameObject load;
    void Awake()
    {
        reLoadList();
    }

    public void reLoadList()
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
            GameObject instance = Instantiate(load,Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(transform,false);

            loadScript script = instance.GetComponent<loadScript>();
            if (archives[archiveIndex].index == loadIndex)
            {
                script.setUp(loadIndex, archives[archiveIndex]);
                archiveIndex++;
            }
            else if (archives[archiveIndex].index < loadIndex)
            {
                script.setUp(loadIndex, null);

                script = transform.GetChild(archives[archiveIndex].index).GetComponent<loadScript>();
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
        GameObject ins = Instantiate(load, Vector3.zero, Quaternion.identity);
        ins.transform.SetParent(transform, false);
        loadScript scr = ins.GetComponent<loadScript>();
        scr.setUp(loadIndex, null);

    }
}

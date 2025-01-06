using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadListCreateScript : MonoBehaviour
{

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
            GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/load"),Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(transform,false);

            loadScript script = instance.GetComponent<loadScript>();
            if (archives[archiveIndex].index == loadIndex)
            {
                script.setUp(loadIndex, archives[archiveIndex]);
                archiveIndex++;
            }
            else
            {
                script.setUp(loadIndex, null);
            }

            loadIndex++;
        }
        Resources.UnloadUnusedAssets();
    }
}

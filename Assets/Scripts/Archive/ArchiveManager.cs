using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArchiveManager : MonoBehaviour
{
    static string path = Application.dataPath + "/Archives";

    private void Start()
    {
        //Debug.Log(path);
        createDictory(path);
    }

    /// <summary>
    /// 保存存档
    /// </summary>
    /// <param name="archive">存档信息</param>
    /// <param name="archiveIndex">存档编号</param>
    static public void SaveArchive(Archive archive, int archiveIndex)
    {
        string data = JsonUtility.ToJson(archive);
        
        string targetPath = path + "/" + archiveIndex + ".json";
        File.WriteAllText(targetPath, data);
    }

    /// <summary>
    /// 读取存档
    /// </summary>
    /// <param name="archiveIndex">存档编号</param>
    /// <returns>包含存档信息的Archive类</returns>
    static public Archive ReadArchive(int archiveIndex)
    {
        foreach (FileInfo file in FindAllJsonFiles(path))
        {
            if (file.Name.Split('.')[0] == archiveIndex.ToString())
            {
                string data = File.ReadAllText(file.FullName);
                Archive archive = JsonUtility.FromJson<Archive>(data);

                return archive;
            }
        }
        return null;
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="archiveIndex">存档编号</param>
    static public void DeleteArchive(int archiveIndex)
    {
        string targetPath = path + "/" + archiveIndex + ".json";
        File.Delete(targetPath);
    }

    /// <summary>
    /// 获取所有存档json
    /// </summary>
    /// <param name="directoryPath">目标路径</param>
    /// <returns></returns>
    static FileInfo[] FindAllJsonFiles(string directoryPath)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
        FileInfo[] files = dirInfo.GetFiles("*.json", SearchOption.AllDirectories);

        return files;
    }
    static public FileInfo[] FindAllJsonFiles()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] files = dirInfo.GetFiles("*.json", SearchOption.AllDirectories);

        return files;
    }

    /// <summary>
    /// 如果文件夹不存在，则创建文件夹
    /// </summary>
    /// <param name="path">要创建的文件夹路径</param>
    static void createDictory(string path)
    {

        //Debug.Log(path);
        if (!Directory.Exists(path))
        {
            Debug.Log(path+" created!");
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 调试：读取存档
    /// </summary>
    static public void DebugArchiveRead(int index)
    {
        Archive archive = ReadArchive(index);

        Debug.Log("level:"+archive.level);
        Debug.Log("feather:" + archive.feather);
        Debug.Log("tenacity:"+archive.tenacity);
        Debug.Log("strength:"+archive.strength);
    }

    /// <summary>
    /// 调试：将0号存档内容保存到1号存档
    /// </summary>
    static public void DebugArchiveSave()
    {
        Archive archive = ReadArchive(0);

        SaveArchive(archive, 1);
    }
}

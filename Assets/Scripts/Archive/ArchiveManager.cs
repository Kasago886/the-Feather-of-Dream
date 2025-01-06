using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ArchiveManager : MonoBehaviour
{
    public Archive currentArchive = null;

    static string path = Application.dataPath + "/Archives";

    public void Awake()
    {
        //Debug.Log(path);
        createDictory(path);

        if (currentArchive == null)
        {
            ReadArchive(0);
        }
    }

    /// <summary>
    /// 设置currentArchive
    /// </summary>
    /// <param name="archiveIndex">存档编号</param>
    public void ReadArchive(int archiveIndex)
    {
        currentArchive = GetArchive(archiveIndex);
    }

    /// <summary>
    /// 保存存档
    /// </summary>
    /// <param name="archive">存档信息</param>
    /// <param name="archiveIndex">存档编号</param>
    static public void SaveArchive(Archive archive, int archiveIndex)
    {
        string data = JsonUtility.ToJson(archive);
        Debug.Log(data);

        string targetPath = path + "/" + archiveIndex + ".json";
        File.WriteAllText(targetPath, data);
    }

    /// <summary>
    /// 获取存档
    /// </summary>
    /// <param name="archiveIndex">存档编号</param>
    /// <returns>包含存档信息的Archive类</returns>
    static public Archive GetArchive(int archiveIndex)
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
    /// 获取所有存档
    /// </summary>
    /// <returns>包含存档信息的Archive数组</returns>
    static public Archive[] GetAllArchive()
    {
        List<Archive> list = new List<Archive>();
        FileInfo[] files = FindAllJsonFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i];
            string data = File.ReadAllText(file.FullName);
            Archive archive = JsonUtility.FromJson<Archive>(data);

            list.Add(archive);
        }

        return list.ToArray();
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
    /// 获取图片sprite
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static public Sprite GetSprite(string path)
    {
        Sprite sprite = null;

        string str = SetImageToString(path);
        Texture2D texture = GetTextureByString(str);
        if (texture != null)
        {
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        return sprite;
    }

    /// <summary>
    /// 将图片转化为字符串
    /// </summary>
    static public string SetImageToString(string imgPath)
    {
        FileStream fs = new FileStream(imgPath, FileMode.Open);
        byte[] imgByte = new byte[fs.Length];
        fs.Read(imgByte, 0, imgByte.Length);
        fs.Close();
        return Convert.ToBase64String(imgByte);
    }

    /// <summary>
    /// 将字符串转换为纹理
    /// </summary>
    static public Texture2D GetTextureByString(string textureStr)
    {
        Texture2D tex = new Texture2D(1, 1);
        byte[] arr = Convert.FromBase64String(textureStr);
        tex.LoadImage(arr);
        tex.Apply();
        return tex;
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
        Archive archive = GetArchive(index);

        Debug.Log("level:"+archive.playerInfo.level);
        Debug.Log("feather:" + archive.playerInfo.feather);
        Debug.Log("tenacity:"+archive.playerInfo.tenacity);
        Debug.Log("strength:"+archive.playerInfo.strength);
    }

    /// <summary>
    /// 调试：将0号存档内容保存到1号存档
    /// </summary>
    static public void DebugArchiveSave()
    {
        Archive archive = GetArchive(0);

        SaveArchive(archive, 1);
    }
}

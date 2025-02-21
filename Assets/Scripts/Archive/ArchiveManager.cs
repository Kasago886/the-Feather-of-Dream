using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;



public enum FlagType
{
    tutorialDone, littleRedRidingHood, level11DreamBottleUsed,weirdDwarf, level21DreamBottleUsed, level21StartDialogRead, whaleKilled, tinWoodmanKilled,
    fragPrinceKilled, fragGuard1Killed, fragGuard2Killed, crazyHunterKilled, burningDocuments1Killed, burningDocuments2Killed, wolfKilled, finalBossKilled,
    contaminateCloneKilled, AssistantDestroyed, drNeprizKilled, enslavedDwarfKilled, dwarfKilled
}

public class ArchiveManager : MonoBehaviour
{
    public string title = "";

    public List<Transform> archivePointers = new();

    [HideInInspector] public Archive currentArchive = null;

    EquipmentPanelManager equipmentPanelManager = null;
    GameObject player;

    static string path = Application.dataPath + "/Archives";
    static string archiveScreenShotPath = Application.dataPath + "/Archives/ArchiveScreenShot";

    public void Awake()
    {
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag);

        //创建路径
        createDictory(path);
        createDictory(archiveScreenShotPath);
        
        //读取当前存档
        if (PlayerPrefs.HasKey(Consts.CurrentArchivePlayerPrefTag))
        {
            int archiveindex = PlayerPrefs.GetInt(Consts.CurrentArchivePlayerPrefTag);
            ReadArchive(archiveindex);
        }
        else
        {
            ReadArchive(0);
        }

        if (currentArchive != null)
        {
            Debug.Log("Level Index:" + currentArchive.index);

            //保存当前关卡信息
            if (title != "")
            {
                currentArchive.levelInfo.title = title;
            }

            //记录点
            if (archivePointers.Count > currentArchive.levelInfo.archivePoint && currentArchive.levelInfo.archivePoint != -1 && player != null)
            {
                Debug.Log(currentArchive.levelInfo.archivePoint);
                player.transform.position = archivePointers[currentArchive.levelInfo.archivePoint].position;
                Assistant assistant = FindAnyObjectByType<Assistant>();
                if (assistant != null)
                {
                    assistant.transform.position = player.transform.position + new Vector3(-1,1);
                }
            }

            //新存档截图
            if (currentArchive.levelInfo.level == -1)
            {
                //EquipmentPanel先更新，防止初始测试Item覆盖存档
                if (equipmentPanelManager != null)
                {
                    if (!equipmentPanelManager.setuped)
                    {
                        equipmentPanelManager.SetupPanel();
                    }
                }

                currentArchive.levelInfo.level = 0;
                SaveCurrentArchive();
            }
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
    /// 保存当前存档
    /// </summary>
    /// <param name="archiveIndex"></param>
    public void SaveCurrentArchive(int archiveIndex = -1, Action finishAction = null, int level = -1, int archivePoint = -1)
    {
        //Index
        if (archiveIndex != -1)
        {
            currentArchive.index = archiveIndex;
        }
        
        //Items
        if (equipmentPanelManager != null)
        {
            currentArchive = equipmentPanelManager.SaveItemsState(currentArchive);
        }

        //TimeInfo
        System.DateTime time = System.DateTime.Now;
        currentArchive.timeInfo.year = time.Year;
        currentArchive.timeInfo.month = time.Month;
        currentArchive.timeInfo.day = time.Day;
        currentArchive.timeInfo.hour = time.Hour;
        currentArchive.timeInfo.minute = time.Minute;
        currentArchive.timeInfo.second = time.Second;

        //level
        if (level != -1)
        {
            currentArchive.levelInfo.level = level;
        }
        if (archivePoint != -1)
        {
            currentArchive.levelInfo.archivePoint = archivePoint;
        }

        //screenShot
        if (currentArchive.levelInfo.level != -1)
        {
            StartCoroutine(CaptureCamera(Camera.main, finishAction));
        }

        //save
        SaveArchive(currentArchive, currentArchive.index);
    }

    /// <summary>
    /// 对相机截图
    /// </summary>
    /// <param name="camera">要被截屏的相机</param>
    /// <param name="finishAction">截屏完成后执行的Action</param>
    IEnumerator CaptureCamera(Camera camera, Action finishAction = null)
    {
        // 获取相机渲染的屏幕尺寸
        int width = Screen.width;
        int height = Screen.height;

        // 创建一个RenderTexture对象，尺寸与屏幕相同，并设置sRGB为true
        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.Default);
        camera.targetTexture = rt;

        // 等待一帧，确保相机渲染完成
        yield return new WaitForEndOfFrame();

        // 激活这个rt, 并从中读取像素。
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示
        camera.targetTexture = null;
        RenderTexture.active = null; // 避免错误
        GameObject.Destroy(rt);

        // 将纹理数据编码为JPG格式的字节数组
        byte[] bytes = screenShot.EncodeToJPG();
        // 设置JPG图片的保存路径
        string filename = Application.dataPath + "/Archives/ArchiveScreenShot/" + currentArchive.index.ToString() + ".jpg";
        System.IO.File.WriteAllBytes(filename, bytes);

        finishAction?.Invoke();
    }

    /// <summary>
    /// 保存存档
    /// </summary>
    /// <param name="archive">存档信息</param>
    /// <param name="archiveIndex">存档编号</param>
    static public void SaveArchive(Archive archive, int archiveIndex)
    {
        string data = JsonConvert.SerializeObject(archive);
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
                Archive archive = JsonConvert.DeserializeObject<Archive>(data);

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
            Archive archive = JsonConvert.DeserializeObject<Archive>(data);

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
    /// 检查状态
    /// </summary>
    /// <param name="flag">状态类型</param>
    /// <param name="setFlag">修改状态，默认不修改</param>
    /// <param name="set">修改后的状态，默认true</param>
    /// <returns>修改前的状态</returns>
    static public bool CheckFlag(FlagType flag, bool setFlag = false, bool set = true)
    {
        ArchiveManager archiveManager = FindAnyObjectByType<ArchiveManager>();
        switch(flag)
        {
            case FlagType.enslavedDwarfKilled:
                if (archiveManager.currentArchive.levelInfo.enslavedDwarfKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.enslavedDwarfKilled = set;
                }
                return false;
            case FlagType.dwarfKilled:
                if (archiveManager.currentArchive.levelInfo.dwarfKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.drNeprizKilled = set;
                }
                return false;
            case FlagType.drNeprizKilled:
                if (archiveManager.currentArchive.levelInfo.drNeprizKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.drNeprizKilled = set;
                }
                return false;
            case FlagType.AssistantDestroyed:
                if (archiveManager.currentArchive.levelInfo.AssistantDestroyed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.AssistantDestroyed = set;
                }
                return false;
            case FlagType.contaminateCloneKilled:
                if (archiveManager.currentArchive.levelInfo.contaminateCloneKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.contaminateCloneKilled = set;
                }
                return false;
            case FlagType.finalBossKilled:
                if (archiveManager.currentArchive.levelInfo.finalBossKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.finalBossKilled = set;
                }
                return false;
            case FlagType.wolfKilled:
                if (archiveManager.currentArchive.levelInfo.wolfKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.wolfKilled = set;
                }
                return false;
            case FlagType.burningDocuments1Killed:
                if (archiveManager.currentArchive.levelInfo.burningDocuments1Killed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.burningDocuments1Killed = set;
                }
                return false;
            case FlagType.burningDocuments2Killed:
                if (archiveManager.currentArchive.levelInfo.burningDocuments2Killed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.burningDocuments2Killed = set;
                }
                return false;
            case FlagType.crazyHunterKilled:
                if (archiveManager.currentArchive.levelInfo.crazyHunterKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.crazyHunterKilled = set;
                }
                return false;
            case FlagType.littleRedRidingHood:
                if (archiveManager.currentArchive.levelInfo.littleRedRidingHood)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.littleRedRidingHood = set;
                }
                return false;

            case FlagType.tutorialDone:
                if (archiveManager.currentArchive.levelInfo.tutorialDone)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.tutorialDone = set;
                }
                return false;

            case FlagType.level11DreamBottleUsed:
                if (archiveManager.currentArchive.levelInfo.level11DreamBottleUsed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.level11DreamBottleUsed = set;
                }
                return false;

            case FlagType.weirdDwarf:
                if (archiveManager.currentArchive.levelInfo.weirdDwarf)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.weirdDwarf = set;
                }
                return false;

            case FlagType.level21DreamBottleUsed:
                if (archiveManager.currentArchive.levelInfo.level21DreamBottleUsed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.level21DreamBottleUsed = set;
                }
                return false;

            case FlagType.level21StartDialogRead:
                if (archiveManager.currentArchive.levelInfo.level21StartDialogRead)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.level21StartDialogRead = set;
                }
                return false;


            case FlagType.whaleKilled:
                if (archiveManager.currentArchive.levelInfo.whaleKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.whaleKilled = set;
                }
                return false;

            case FlagType.tinWoodmanKilled:
                if (archiveManager.currentArchive.levelInfo.tinWoodmanKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.tinWoodmanKilled = set;
                }
                return false;


            case FlagType.fragPrinceKilled:
                if (archiveManager.currentArchive.levelInfo.fragPrinceKilled)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.fragPrinceKilled = set;
                }
                return false;

            case FlagType.fragGuard1Killed:
                if (archiveManager.currentArchive.levelInfo.fragGuard1Killed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.fragGuard1Killed = set;
                }
                return false;
            case FlagType.fragGuard2Killed:
                if (archiveManager.currentArchive.levelInfo.fragGuard2Killed)
                {
                    return true;
                }
                if (setFlag)
                {
                    archiveManager.currentArchive.levelInfo.fragGuard2Killed = set;
                }
                return false;
        }
        Debug.LogError("Flag undefined:" + flag.ToString());
        return false;
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
        string data = JsonConvert.SerializeObject(archive);
        Debug.Log(data);
    }
    static public void DebugArchiveRead(Archive archive)
    {
        string data = JsonConvert.SerializeObject(archive);
        Debug.Log(data);
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

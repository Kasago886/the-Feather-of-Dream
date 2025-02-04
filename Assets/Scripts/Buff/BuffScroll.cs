using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;
[System.Serializable]
public class BuffImformation
{
    public string name;
    [Multiline]
    public string description;
    public GameObject buffPrefab;
}
public class BuffScroll : MonoBehaviour
{
    public Player player;
    public RectTransform contentRectTransform;
    public List<BuffImformation> buffImformation;
    private Dictionary<string, float> buffNameDict;
    private Dictionary<string, BuffImformation> nameToImformation;
    private Dictionary<string, BuffImage> buffDescDict;
    private float oriNumber;
    private void Start()
    {
        buffNameDict = new Dictionary<string, float>();
        buffDescDict = new Dictionary<string, BuffImage>();
        nameToImformation = new Dictionary<string, BuffImformation>();
        for (int i = 0; i < buffImformation.Count; i++)
        {
            if (BuffContainer.buffDictionary.ContainsKey(buffImformation[i].name))
            {
                nameToImformation.Add(buffImformation[i].name, buffImformation[i]);
            }
        }
    }
    void Update()
    {
        AddGameObject();
    }
    private void AddGameObject()
    {
        Dictionary<string, float> newDict = GetBuffNumber();
        List<string> list = new List<string>();
        foreach (var item in buffNameDict)
        {
            if (!newDict.ContainsKey(item.Key))
            {
                Destroy(buffDescDict[item.Key].gameObject);
                buffDescDict.Remove(item.Key);
                list.Add(item.Key);               
            }
        }
        for (int i = 0;i < list.Count;i++)
        {
            //Debug.Log(list[i] + "»¹´æÔÚ");
            buffNameDict.Remove(list[i]);
        }
        foreach (var item in newDict)
        {
            if (!buffNameDict.ContainsKey(item.Key))
            {
                buffNameDict.Add(item.Key, item.Value);
                GameObject g = Instantiate(nameToImformation[item.Key].buffPrefab,contentRectTransform);
                g.GetComponent<BuffImage>().text = nameToImformation[item.Key].name+"\n"+ nameToImformation[item.Key].description;
                buffDescDict.Add(item.Key, g.GetComponent<BuffImage>());
                buffDescDict[item.Key].gameObject.GetComponentInChildren<Text>().text = newDict[item.Key].ToString();
            }
            else
            {
                buffDescDict[item.Key].gameObject.GetComponentInChildren<Text>().text = newDict[item.Key].ToString();
            }
        }
    }
    private Dictionary<string, float> GetBuffNumber()
    {
        Dictionary<string, float> buffNameDict1 = new Dictionary<string, float>();
        for (int i = 0; i < player.buffList.Count; i++)
        {
            if (nameToImformation.ContainsKey(player.buffList[i].name))
            {
                if (buffNameDict1.ContainsKey(nameToImformation[player.buffList[i].name].name))
                {
                    buffNameDict1[nameToImformation[player.buffList[i].name].name]++;
                }
                else
                {
                    buffNameDict1.Add(nameToImformation[player.buffList[i].name].name, 1);
                }
            }
        }
        return buffNameDict1;
    }
}

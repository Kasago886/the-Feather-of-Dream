using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    private Dictionary<string, Text> buffDescDict;
    private float oriNumber;
    private void Start()
    {
        buffNameDict = new Dictionary<string, float>();
        buffDescDict = new Dictionary<string, Text>();
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
        Debug.Log("player.buffList.Count=" + player.buffList.Count);
        AddGameObject();
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Add…À∫€");
            GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>().AddBuff("…À∫€");
        }
    }
    private void AddGameObject()
    {
        Dictionary<string, float> newDict = GetBuffNumber();
        foreach (var item in buffNameDict)
        {
            if (newDict.ContainsKey(item.Key))
            {
                Destroy(buffDescDict[item.Key].gameObject.transform.parent);
                buffDescDict.Remove(item.Key);
                buffNameDict.Remove(item.Key);
            }
        }
        foreach (var item in newDict)
        {
            if (!buffNameDict.ContainsKey(item.Key))
            {
                Debug.Log(item.Key + "is added");
                buffNameDict.Add(item.Key, item.Value);
                GameObject g = Instantiate(nameToImformation[item.Key].buffPrefab);
                g.GetComponent<RectTransform>().SetParent(contentRectTransform);
                g.GetComponent<BuffImage>().text = nameToImformation[item.Key].description;
                buffDescDict.Add(item.Key, g.GetComponentInChildren<Text>());
                buffDescDict[item.Key].text = newDict.Values.ToString();
            }
            else
            {
                buffDescDict[item.Key].text = newDict.Values.ToString();
            }
        }

    }
    private Dictionary<string, float> GetBuffNumber()
    {
        Dictionary<string, float> buffNameDict1 = new Dictionary<string, float>();
        for (int i = 0; i < player.buffList.Count; i++)
        {
            Debug.Log("buff√˚◊÷Œ™" + player.buffList[i].name);
            if (nameToImformation.ContainsKey(player.buffList[i].name))
            {
                Debug.Log("”–…À∫€");
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBuffView : MonoBehaviour
{
    public Enemy enemy;
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
        if (enemy != null)
        {
            AddGameObject();
        }
    }
    public void AddGameObject()
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
        for (int i = 0; i < list.Count; i++)
        {
            buffNameDict.Remove(list[i]);
        }
        foreach (var item in newDict)
        {
            if (!buffNameDict.ContainsKey(item.Key))
            {
                buffNameDict.Add(item.Key, item.Value);
                GameObject g = Instantiate(nameToImformation[item.Key].buffPrefab, contentRectTransform);
                g.GetComponent<BuffImage>().text = nameToImformation[item.Key].name + "\n" + nameToImformation[item.Key].description;
                g.GetComponent<RectTransform>().localScale = new Vector2(g.GetComponent<RectTransform>().localScale.x / 2, g.GetComponent<RectTransform>().localScale.y / 2);
                buffDescDict.Add(item.Key, g.GetComponent<BuffImage>());
                buffDescDict[item.Key].gameObject.GetComponentInChildren<Text>().text = newDict[item.Key].ToString();
                if (BuffContainer.buffDictionary.TryGetValue(item.Key, out Type classType))
                {
                    if (classType.GetInterfaces().Any(i => i == typeof(Energize)))
                    {
                        foreach (var buff in enemy.buffList)
                        {
                            if (buff.name == item.Key)
                            {
                                buffDescDict[item.Key].gameObject.transform.GetChild(1).GetComponent<Text>().text = ((Energize)buff).GetNumber().ToString();
                                buffDescDict[item.Key].text += $"\n已充能{((Energize)buff).GetNumber()}层";
                            }
                        }
                    }
                }
            }
            else
            {
                buffDescDict[item.Key].gameObject.GetComponentInChildren<Text>().text = newDict[item.Key].ToString();
                buffDescDict[item.Key].text = nameToImformation[item.Key].name + "\n" + nameToImformation[item.Key].description;
                if (BuffContainer.buffDictionary.TryGetValue(item.Key, out Type classType))
                {
                    if (classType.GetInterfaces().Any(i => i == typeof(Energize)))
                    {
                        foreach (var buff in enemy.buffList)
                        {
                            if (buff.name == item.Key)
                            {
                                buffDescDict[item.Key].gameObject.transform.GetChild(1).GetComponent<Text>().text = ((Energize)buff).GetNumber().ToString();
                                buffDescDict[item.Key].text += $"\n已充能{((Energize)buff).GetNumber()}层";
                            }
                        }
                    }
                }
            }
        }
    }
    private Dictionary<string, float> GetBuffNumber()
    {
        Dictionary<string, float> buffNameDict1 = new Dictionary<string, float>();
        for (int i = 0; i < enemy.buffList.Count; i++)
        {
            if (nameToImformation.ContainsKey(enemy.buffList[i].name))
            {
                if (buffNameDict1.ContainsKey(nameToImformation[enemy.buffList[i].name].name))
                {
                    buffNameDict1[nameToImformation[enemy.buffList[i].name].name]++;
                }
                else
                {
                    buffNameDict1.Add(nameToImformation[enemy.buffList[i].name].name, 1);
                }
            }
        }
        return buffNameDict1;
    }
}

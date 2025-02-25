using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffImage : MonoBehaviour
{
    private GameObject description;
    [HideInInspector]
    public string text;
    public void Enter()
    {
        description = FindInactiveChild(GameObject.Find("Canvas").GetComponent<Transform>(), "buffDescription");
        description.SetActive(true);
        description.GetComponentInChildren<Text>().text= text;
    }
    public void Exit()
    {
        description = FindInactiveChild(GameObject.Find("Canvas").GetComponent<Transform>(), "buffDescription");
        description.SetActive(false);
    }
    private void OnDestroy()
    {
        description = FindInactiveChild(GameObject.Find("Canvas").GetComponent<Transform>(), "buffDescription");
        description.SetActive(false);
    }
    public GameObject FindInactiveChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            // 检查当前子物体名称是否匹配
            if (child.name == name)
            {
                return child.gameObject;
            }

            // 递归搜索子物体的子物体
            GameObject result = FindInactiveChild(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}

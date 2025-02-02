using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffImage : MonoBehaviour
{
    private GameObject description;
    [HideInInspector]
    public string text;
    private void Start()
    {
        description = GameObject.Find("buffDescription");
    }
    public void Enter()
    {
        description.SetActive(true);
        description.GetComponentInChildren<Text>().text= text;
    }
    public void Exit()
    {
        description.SetActive(false);
    }
    private void OnDestroy()
    {
        if (description != null)
        {
            description.SetActive(false);
        }
    }
}

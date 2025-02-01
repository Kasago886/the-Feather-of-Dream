using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffImage : MonoBehaviour
{
    public GameObject description;
    [HideInInspector]
    public string text;
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

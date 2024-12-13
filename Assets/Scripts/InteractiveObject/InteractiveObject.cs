using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [Header("触发条件")]
    public bool mouseTrigger;
    public bool keyTrigger;
    [Header("触发使用的按键")]
    public string nameOfKey;
    [Header("可触发事件的最远距离")]
    public float maxDistance;
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public static Dictionary<int,GameObject> trigger = new Dictionary<int, GameObject>();
    private void Start()
    {
    }
    void Update()
    {
        if (trigger.Count>0)
        {
            foreach (var item in trigger)
            {
                InteractiveObject interactiveObject = item.Value.GetComponent<InteractiveObject>();
                interactiveObject.ButtonDetect();
                //Debug.Log(interactiveObject.name);
            }
        }
    }
}

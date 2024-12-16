using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public static List<GameObject> trigger;
    private void Start()
    {
        trigger = new List<GameObject>();
    }
    void Update()
    {
        if (trigger.Count>0)
        {
            for (int i = 0; i < trigger.Count; i++)
            {
                InteractiveObject interactiveObject=trigger[i].GetComponent<InteractiveObject>();
                if(Input.GetKeyDown(interactiveObject.nameOfKey))
                {
                    interactiveObject.Interact();
                }
            }
        }
    }
}

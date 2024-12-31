using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public static Dictionary<int,GameObject> trigger;
    private void Start()
    {
        trigger = new Dictionary<int, GameObject>();
    }
    void Update()
    {
        if (trigger.Count>0)
        {
            foreach (var item in trigger)
            {
                InteractiveObject interactiveObject = item.Value.GetComponent<InteractiveObject>();
                if (Input.GetKeyDown(interactiveObject.nameOfKey))
                {
                    interactiveObject.Interact();
                }
            }
        }
    }
}

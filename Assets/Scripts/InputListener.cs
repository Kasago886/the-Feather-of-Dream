using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UnityEvent
using UnityEngine.Events;
using UnityEngine.UIElements;

public class InputListener : MonoBehaviour
{
    [Header("°´¼üÊÂ¼þ")]
    public UnityEvent<float> HorizontalEvent;
    public UnityEvent SpaceEvent;
    public UnityEvent ShiftEvent;
    public UnityEvent CEvent;
    public UnityEvent TabEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalEvent?.Invoke(Input.GetAxis("Horizontal"));
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShiftEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabEvent?.Invoke();
        }
    }
}

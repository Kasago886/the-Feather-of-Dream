using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButtonScript : MonoBehaviour
{
    InputListener listener;
    // Start is called before the first frame update
    void Start()
    {
        listener = FindAnyObjectByType<InputListener>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchPausePanel()
    {
        listener.SwitchPausePanel();
    }
}

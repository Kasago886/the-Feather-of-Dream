using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Descraption1 : MonoBehaviour
{
    public GameObject g;
    void Start()
    {
        g.SetActive(false);
    }

    public void Enter()
    {
        g.SetActive(true);
    }
    public void Out()
    {
        g.SetActive(false);
    }
}

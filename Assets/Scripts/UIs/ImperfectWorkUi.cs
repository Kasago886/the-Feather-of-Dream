using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImperfectWorkUi : MonoBehaviour
{
    public bool b, b1, b2, b3, b4;
    public GameObject g1, g2, g3, g4;
    void Update()
    {
        if (b)
        {
            if (b1)
            {
                g1.SetActive(true);
            }
            if (b2)
            {
                g2.SetActive(true);
            }
            if (b3)
            {
                g3.SetActive(true);
            }
            if (b4)
            {
                g4.SetActive(true);
            }
            if (GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
            {
                b1 = false;
                b2 = false;
                b3 = false;
                b4 = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImperfectWorkUi : MonoBehaviour
{
    public bool b, b1, b2, b3, b4;
    public GameObject g1, g2, g3, g4;
    public float timeNumber1, timeNumber2,timeNumber3, timeNumber4;
    void Update()
    {
        if (b)
        {
            if (b1)
            {
                g1.SetActive(true);
                g1.transform.GetChild(0).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber1;
                g1.transform.GetChild(1).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber1;
                g1.transform.GetChild(2).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber1;
            }
            if (b2)
            {
                g2.SetActive(true);
                g2.transform.GetChild(0).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber2;
                g2.transform.GetChild(1).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber2;
                g2.transform.GetChild(2).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber2;
            }
            if (b3)
            {
                g3.SetActive(true);
                g3.transform.GetChild(0).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber3;
                g3.transform.GetChild(1).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber3;
                g3.transform.GetChild(2).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber3;
            }
            if (b4)
            {
                g4.SetActive(true);
                g4.transform.GetChild(0).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber4;
                g4.transform.GetChild(1).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber4;
                g4.transform.GetChild(2).GetComponent<ImperfectWorkChoice>().timeNumber = timeNumber4;
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

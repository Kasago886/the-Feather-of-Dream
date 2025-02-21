using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuff : MonoBehaviour
{
    Nepriz2 nepriz2;
    private void Start()
    {
        nepriz2 = GetComponent<Nepriz2>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>().AddBuff("è¦´ÃÖ®×÷");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            nepriz2.feathers[0].health -= 21;
        }
    }
}

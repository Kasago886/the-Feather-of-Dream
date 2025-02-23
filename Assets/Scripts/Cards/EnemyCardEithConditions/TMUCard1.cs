using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMUCard1 : MonoBehaviour
{
    public void GetCard()
    {
        PlayerCardController playerCardController= GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        playerCardController.GetCard("Îó½â");
    }
    public void RandomGet()
    {
        if (Random.Range(0, 3) > 1)
        {
            GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>().AddBuff("Îó½â");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMUCard1 : MonoBehaviour
{
    public void GetCard()
    {
        PlayerCardController playerCardController= GameObject.FindAnyObjectByType<PlayerCardController>();
        playerCardController.GetCard("ŒÛΩ‚");
    }
    public void RandomGet()
    {
        if (Random.Range(0, 4) < 1)
        {
            GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>().AddBuff("ŒÛΩ‚");
        }
    }
    public void GetTradege()
    {
        PlayerCardController playerCardController = GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
        if (playerCardController.GetCardNumber("ŒÛΩ‚") > 0)
        {
            playerCardController.GetCard("È‰");
        }
    }
}

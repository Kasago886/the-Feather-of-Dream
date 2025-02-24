using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMUCard1 : MonoBehaviour
{
    public void GetCard()
    {
        PlayerCardController playerCardController= GameObject.Find("CardPanel").GetComponent<PlayerCardController>();
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
        bool b = false;
        foreach(GameObject card in playerCardController.cardsList)
        {
            if (card.GetComponent<Card>().name == "ŒÛΩ‚")
            {
                b = true;
                break;
            }
        }
        playerCardController.GetCard("È‰");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCardDelete : MonoBehaviour
{
    [HideInInspector] public GameObject card;
    public void PointerClick()
    {
        Destroy(card);
        Card oriCard=card.GetComponent<Card>();
        oriCard.Captions($"¶ªÆú{oriCard.GetComponent<Card>().name}", true);
        Destroy(gameObject);
    }
}

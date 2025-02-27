using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCardCancle : MonoBehaviour
{
    [HideInInspector] public GameObject delete;
   public void PointerClick()
    {
        Destroy(delete);
        Destroy(gameObject);                               
    }
}

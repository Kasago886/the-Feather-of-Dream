using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamBottle : InteractiveObject {
    public override void Interact()
    {
        base.Interact();
        Debug.Log("dream+1");
        GetComponent<Animator>().Play("usedBottle");
    }
}

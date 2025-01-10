using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamBottle : InteractiveObject {
    private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public override void Interact()
    {
        base.Interact();
        player.AddDream(1);
        GetComponent<Animator>().Play("usedBottle");
    }
}

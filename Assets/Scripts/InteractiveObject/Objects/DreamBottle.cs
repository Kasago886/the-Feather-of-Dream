using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamBottle : InteractiveObject {
    private Player player_;

    bool used = false;
    protected override void Start()
    {
        base.Start();
        player_ = FindObjectOfType<Player>();
    }

    public override void Interact()
    {
        if (!used)
        {
            base.Interact();
            used = true;
            player_.AddDream(1);
            GetComponent<Animator>().Play("usedBottle");
        }
    }
}

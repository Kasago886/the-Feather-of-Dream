using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ladder : InteractiveObject 
{
    Player p;
    protected override void Start()
    {
        base.Start();
        
        p = player.GetComponent<Player>();
    }
    public override void Interact()
    {
        base.Interact();

        p.rb.velocity = new Vector2(p.rb.velocity.x, 7);
    }
}

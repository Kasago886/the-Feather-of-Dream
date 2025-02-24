using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentAttack1 : MonoBehaviour
{
    private ExperimentPlayer experimentPlayer;
    private void Start()
    {
        experimentPlayer = FindObjectOfType<ExperimentPlayer>();
      
    }
   public void onHit()
    {
        experimentPlayer.gameObject.transform.position += experimentPlayer.gameObject.transform.forward;
    }
}

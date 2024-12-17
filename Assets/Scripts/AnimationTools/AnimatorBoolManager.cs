using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBoolManager : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTrue(string argument)
    {
        animator.SetBool(argument,true);
    }

    public void SetFalse(string argument)
    {
        animator.SetBool(argument,false);
    }

    public void SwitchValue(string argument)
    {
        animator.SetBool(argument,!animator.GetBool(argument));
    }
}

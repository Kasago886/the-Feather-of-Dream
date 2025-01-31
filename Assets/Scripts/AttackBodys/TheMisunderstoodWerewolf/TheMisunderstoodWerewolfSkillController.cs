using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TheMisunderstoodWerewolfSkillController : MonoBehaviour
{
    public float limitR;
    public float speed;
    public void UseSkill()
    {
        TheMisunderstoodWerewolfSkill.instance.useSkill = true;
        TheMisunderstoodWerewolfSkill.instance.limitR = limitR;
        TheMisunderstoodWerewolfSkill.instance.speed = speed;
        TheMisunderstoodWerewolfSkill.instance.number = 2;
    }
}

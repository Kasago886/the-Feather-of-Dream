using UnityEngine;

public class TheMisunderstoodWerewolfSkillController : MonoBehaviour
{
    public float limitR;
    public float speed;
    public void UseSkill()
    {
        if (GameObject.Find("TheMisunderstoodWerewolf") != null)
        {
            TheMisunderstoodWerewolfSkill.useSkill = true;
            TheMisunderstoodWerewolfSkill.limitR = limitR;
            TheMisunderstoodWerewolfSkill.speed = speed;
            TheMisunderstoodWerewolfSkill.number = 2;
        }
    }
}

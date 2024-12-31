using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(EventTrigger))]
public class Card : MonoBehaviour
{
    //作用对象
    public bool effortOnPlayer;//作用于玩家
    public bool effortOnEnmey;//作用于敌方
    public bool effortOnOneEnemy;//作用于一个敌方
    public bool effortOnMoreEnemies;//作用于多个敌方
    public int theNumberOfEffortedEnemies;//作用的敌方个数
    //作用方式
    public bool click;
    public bool dragOnCharactor;


    public UnityEvent[] effects;//卡牌效果
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PointerEnter()
    {

    }
    public void PointerExit()
    {

    }
    public void PointerClick()
    {

    }
    public void Drag()
    {
        transform.position = Input.mousePosition;
    }
    public void EndDrag()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i]?.Invoke();
        }
        Destroy(gameObject);
    }
    private void EffortWhenClick()
    {
        if (effortOnPlayer)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i]?.Invoke();
            }
            Destroy(gameObject);
        }
        if (effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize), new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize));
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
            for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
            {
                Bounds bounds = enemiesThatBeenChoose[i].bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                }
            }
            if (theNumberOfEffortedEnemies > enemiesThatBeenChoose.Length)
            {
                theNumberOfEffortedEnemies = enemiesThatBeenChoose.Length;
            }
            for (int i = 0; i < theNumberOfEffortedEnemies; i++)
            {

            a:
                int n = Random.Range(0, theNumberOfEffortedEnemies);
                for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                {
                    if (n == enemiesWhoHaveBeenEfforted[j])
                    {
                        goto a;
                    }
                }
                Enemy enemy=enemiesWhoInCameralist[i].GetComponent<Enemy>();
                enemy.buffList.AddRange(enemy.buffList);
            }
        }
    }
    private void EffortWhenDarag()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerCardController : MonoBehaviour
{
    [Header("全体卡牌预制体")]
    public GameObject[] cardsList;
    private List<GameObject> activeCards;
    private List<int> activeCardsId;
    [Header("卡牌生成的位置")]
    public RectTransform[] positions;
    private Dictionary<string, int> nameToID;
    private Dictionary<int, ObjectPool<Card>> idToCard;
    // Start is called before the first frame update
    void Start()
    {
        nameToID = new Dictionary<string, int>();
        idToCard = new Dictionary<int, ObjectPool<Card>>();
        activeCards = new List<GameObject>();
        activeCardsId = new List<int>();
        for (int i = 0; i < cardsList.Length; i++)
        {
            ObjectPool<Card> cardPool;
            cardPool = new ObjectPool<Card>(cardsList[i], positions);
            Card card = cardPool.GetFromPool();
            nameToID.Add(card.name,card.id);
            idToCard.Add(card.id, cardPool);
            cardPool.ReturnToPool(card);
        }
    }
    /// <summary>
    /// 每两秒自动检测是否少牌
    /// </summary>
    private void Update()
    {
        if (Time.time % 1 == 0)
        {
            ReduceCard();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetCard(1);
        }
    }
    /// <summary>
    /// 调用本函数增加玩家手牌
    /// </summary>
    /// <param name="id">
    /// 想要增加的手牌的id
    /// </param>
    public void GetCard(int id)
    {
        if(activeCards.Count<positions.Length)
        {
            Debug.Log(activeCards.Count);
            activeCards.Add(idToCard[id].GetFromPool(positions[activeCards.Count]).gameObject);
            activeCardsId.Add(id);
        }
    }
    /// <summary>
    /// 调用本函数增加玩家手牌
    /// </summary>
    /// <param name="cardName">
    /// 想要增加的手牌的名称
    /// </param>
    public void GetCard(string cardName)
    {
        GetCard(nameToID[cardName]);
    }
    /// <summary>
    /// 少牌重新排序
    /// </summary>
    private void ReduceCard()
    {
        bool clear=false;
        for(int i = 0;i < activeCards.Count;i++)
        {
            if(!activeCards[i].activeSelf)
            {
                clear=true;
                idToCard[activeCardsId[i]].ReturnToPool(activeCards[i].GetComponent<Card>());
                activeCardsId.RemoveAt(i);
                activeCards.RemoveAt(i);
            }
        }
        if (clear)
        {
            for (int i = 0; i < activeCards.Count; i++)
            {
                idToCard[activeCardsId[i]].ReturnToPool(activeCards[i].GetComponent<Card>());
            }
            activeCards.Clear();
            foreach (int id in activeCardsId)
            {
                GetCard(id);
            }
        }
    }
    /// <summary>
    /// 调用本函数以减少玩家手牌
    /// </summary>
    /// <param name="id">
    /// 想要减少的卡牌的id
    /// </param>
    /// <param name="reduceNumber">
    /// 想要减少的卡牌数量
    /// </param>
    public void ReduceCard(int id,int reduceNumber=1)
    {
        if (reduceNumber > 1)
        {
            List<int> n = new List<int>();
            for(int i = 0; i < activeCardsId.Count; i++)
            {
                if(id == activeCardsId[i])
                {
                    n.Add(i);
                }
            }
            if (reduceNumber > n.Count)
            {
                reduceNumber = n.Count;
            }
            for(int i = 0; i < reduceNumber; i++)
            {
                activeCards[n[i]].SetActive(false);
            }
            ReduceCard();
        }
        else
        {
            for (int i = 0; i < activeCardsId.Count; i++)
            {
                if (id == activeCardsId[i])
                {
                    activeCards[i].SetActive(false);
                    ReduceCard();
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 调用本函数以减少玩家手牌
    /// </summary>
    /// <param name="name">
    /// 想要减少的卡牌的名字
    /// </param>
    /// <param name="reduceNumber">
    /// 想要减少的卡牌数量
    /// </param>
    public void ReduceCard(string name, int reduceNumber=1)
    {
        ReduceCard(nameToID[name],reduceNumber);
    }
    /// <summary>
    /// 调用本函数清空玩家手牌
    /// </summary>
    public void ClearCard()
    {
        for (int i = 0; i < activeCards.Count; i++)
        {
            idToCard[activeCardsId[i]].ReturnToPool(activeCards[i].GetComponent<Card>());
        }
        activeCards.Clear();
        activeCardsId.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DeleteCard : MonoBehaviour, IPointerClickHandler
{
    public GameObject delete;
    public GameObject cancle;
    private RectTransform rectTransform;
    private GameObject newDelete;
    private GameObject newCancle;
    private bool b;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right&&b==false)
        {
            newDelete=Instantiate(delete,new Vector3(rectTransform.position.x-2,rectTransform.position.y+5,rectTransform.position.z),Quaternion.identity);
            newDelete.transform.parent=transform;
            DCardDelete dCardDelete=newDelete.GetComponent<DCardDelete>();
            dCardDelete.card=gameObject;
            b=true;
        }
        else if(eventData.button == PointerEventData.InputButton.Right && b)
        {
            if (newDelete!=null||newCancle!=null)
            {
                Destroy(newDelete);
            }
            b = false;
        }
    }
}

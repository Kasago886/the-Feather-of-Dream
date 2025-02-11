using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UnityEvent
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum InputListenerState
{
    normal,card, equipment, option, boxMap,death
}

public class InputListener : MonoBehaviour
{
    public bool isBoxMap;
    public PlayerController playerController;
    [HideInInspector]public EquipmentPanelManager equipmentPanelManager;
    GameObject PausePanel;
    GameObject DeathPanel;
    GameObject GrayPanel;

    [HideInInspector] public AnimationBoolManager cardPanelAnimationManager;

    Dictionary<InputListenerState, IListenerState> states = new();
    [HideInInspector] public IListenerState currentState;

    // Start is called before the first frame update
    void Start()
    {
        //找到PausePanel（被隐藏了所以不能用FindGameObjectWithTag）
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                if (obj.CompareTag(Consts.PausePanelTag) && obj.scene == gameObject.scene)
                {
                    PausePanel = obj;
                }
                else if (obj.CompareTag(Consts.DeathPanelTag) && obj.scene == gameObject.scene)
                {
                    DeathPanel = obj;
                }
                else if (obj.CompareTag(Consts.GrayPanelTag) && obj.scene == gameObject.scene)
                {
                    GrayPanel = obj;
                }
            }
        }

        states[InputListenerState.normal] = new NormalListenerState(this);
        states[InputListenerState.card] = new CardListenerState(this);
        states[InputListenerState.equipment] = new EquipmentListenerState(this);
        states[InputListenerState.option] = new OptionListenerState(this);
        states[InputListenerState.boxMap] = new BoxMapListenerState(this);
        states[InputListenerState.death] = new DeathListenerState(this);

        if (isBoxMap)
        {
            currentState = states[InputListenerState.boxMap];
        }
        else
        {
            cardPanelAnimationManager = GameObject.FindGameObjectWithTag(Consts.CardPanelTag).GetComponent<AnimationBoolManager>();
            equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();

            currentState = states[InputListenerState.normal];
        }
    }

    public void StateTransition(InputListenerState state)
    {
        currentState = states[state];
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }

    public void SwitchPausePanel()
    {
        if (PausePanel.activeInHierarchy)
        {
            Time.timeScale = 1.0f;
            PausePanel.SetActive(false);

            if (isBoxMap)
            {
                StateTransition(InputListenerState.boxMap);
                return;
            }
            else
            {
                StateTransition(InputListenerState.normal);
                return;
            }
        }
        else
        {
            Time.timeScale = 0;
            PausePanel.SetActive(true);

            StateTransition(InputListenerState.option);
            return;
        }
    }

    public void SwitchCardPanel()
    {
        if (GrayPanel.activeInHierarchy)
        {
            Time.timeScale = 1.0f;
            GrayPanel.SetActive(false);
            cardPanelAnimationManager.SetFalse("appear");
        }
        else
        {
            Time.timeScale = 0.1f;
            GrayPanel.SetActive(true);
            cardPanelAnimationManager.SetTrue("appear");
        }
    }

    public void OnDeath()
    {
        Time.timeScale = 0;
        DeathPanel.SetActive(true);

        StateTransition(InputListenerState.death);
        return;
    }

    public bool CompareStateType(InputListenerState state)
    {
        return states[state].Equals(currentState);
    }
}

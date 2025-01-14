using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UnityEvent
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum InputListenerState
{
    normal, equipment, option
}

public class InputListener : MonoBehaviour
{
    public PlayerController playerController;
    public AnimationBoolManager cardPanelAnimationManager;
    public EquipmentPanelManager equipmentPanelManager;

    Dictionary<InputListenerState, IListenerState> states = new();
    [HideInInspector] public IListenerState currentState;

    // Start is called before the first frame update
    void Start()
    {
        states[InputListenerState.normal] = new NormalListenerState(this);
        states[InputListenerState.equipment] = new EquipmentListenerState(this);
        states[InputListenerState.option] = new OptionListenerState(this);

        currentState = states[InputListenerState.normal];
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
}

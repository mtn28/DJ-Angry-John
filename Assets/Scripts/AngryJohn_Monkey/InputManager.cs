﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class InputManager : MonoBehaviour 
{
    [SerializeField]
    public KeyMap[] KeyMapping;
    Animator MonkeyAnimator;
	
    void Start()
    {
        MonkeyAnimator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < KeyMapping.Length; i++)
        {
            if (Input.GetKeyDown(KeyMapping[i].key))
            {
                if (KeyMapping[i].type == EventType.Boolean)
                {
                    MonkeyAnimator.SetBool(KeyMapping[i].triggerName, true);
                }   
            }

            if (Input.GetKeyUp(KeyMapping[i].key))
            {
                if (KeyMapping[i].type == EventType.Trigger)
                {
                    MonkeyAnimator.SetTrigger(KeyMapping[i].triggerName);
                }
                else if (KeyMapping[i].type == EventType.Boolean)
                {
                    MonkeyAnimator.SetBool(KeyMapping[i].triggerName, false);
                }   
            }
        }
	}
}

[System.Serializable]
public struct KeyMap
{
    public KeyCode key;
    public string triggerName;
    public EventType type;
}

[System.Serializable]
public enum EventType
{
    Trigger,
    Boolean
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu]
public class ActionType : ScriptableObject
{
    public string Name;
    public int Cost;
    public bool Attack;
    public bool Block;
    public bool ApplyForce { get => Force != Vector2.zero; }
    public Vector2 Force;
}

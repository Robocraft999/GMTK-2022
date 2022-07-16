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
    public bool attack;
    public bool block;
    public bool applyForce { get => force != Vector2.zero; }
    public Vector2 force;
}

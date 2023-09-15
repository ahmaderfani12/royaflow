using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Command Buffer Ref", menuName = "ScriptableObjects/CommandBufferRef", order = 1)]
public class CommandBufferRef : ScriptableObject
{
    public CommandBuffer CB;
}

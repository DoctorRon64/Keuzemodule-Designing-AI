using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[System.Flags]
public enum NodeStatus
{
    Succes,
    Failed,
    Running
}

public abstract class BaseNode
{
    public BaseNode parent;
    protected BlackBoard blackBoard;
    protected abstract TaskStatus OnUpdate();
    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    public virtual void OnReset() { }
    protected virtual void Tick() { }
}

using System;
using System.Collections.Generic;
using UnityEngine;

// 기본 노드 클래스
public abstract class Node
{
    public abstract NodeState Tick();
}

// 선택 노드 (Selector)
public class Selector : Node
{
    private List<Node> _nodes;

    public Selector(List<Node> nodes)
    {
        _nodes = nodes;
    }

    public override NodeState Tick()
    {
        foreach (var node in _nodes)
        {
            var state = node.Tick();
            if (state == NodeState.Success)
            {
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}

// 순서 노드 (Sequence)
public class Sequence : Node
{
    private List<Node> _nodes;

    public Sequence(List<Node> nodes)
    {
        _nodes = nodes;
    }

    public override NodeState Tick()
    {
        foreach (var node in _nodes)
        {
            var state = node.Tick();
            if (state == NodeState.Failure)
            {
                return NodeState.Failure;
            }
        }
        return NodeState.Success;
    }
}

// 조건 노드 (Condition)
public class Condition : Node
{
    private Func<bool> _condition;

    public Condition(Func<bool> condition)
    {
        _condition = condition;
    }

    public override NodeState Tick()
    {
        return _condition() ? NodeState.Success : NodeState.Failure;
    }
}

// 동작 노드 (ActionNode)
public class ActionNode : Node
{
    private Action _action;

    public ActionNode(Action action)
    {
        _action = action;
    }

    public override NodeState Tick()
    {
        _action();
        return NodeState.Success;
    }
}


// 대기 노드 (WaitNode)
public class WaitNode : Node
{
    private float waitTime;
    private float startTime;

    public WaitNode(float time)
    {
        waitTime = time;
    }

    public override NodeState Tick()
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime >= waitTime)
        {
            startTime = 0; // Reset start time for next Tick
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}


// 노드 상태
public enum NodeState
{
    Success,
    Failure,
    Running
}

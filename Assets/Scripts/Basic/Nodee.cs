using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodee
{
    public State state;
    public Nodee parent;
    public List<Nodee> children = new List<Nodee>();
    public Nodee(State _state)
    {
        state = _state;
    }

    public bool RemoveChild(Nodee node)
    {
        return children.Remove(node);
    }

    public Nodee AddChild(State _state)
    {
        Nodee node = new Nodee(_state) { parent = this };
        children.Add(node);
        return node;
    }

}


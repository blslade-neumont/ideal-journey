using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCommand
{
    public virtual void Begin()
    {
        Debug.Log("Beginning GameCommand.");
    }

    public virtual bool IsComplete()
    {
        return false;
    }

    public virtual string AsText()
    {
        return "New GameCommand";
    }

    // TODO: Get SFX???
}

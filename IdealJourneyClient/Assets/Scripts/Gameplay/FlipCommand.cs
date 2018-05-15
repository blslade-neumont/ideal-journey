using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCommand : GameCommand
{
    public override void Begin()
    {
        Debug.Log("Initializing flip command. TODO: Set state here!?!?! Do nothing on purpose???.");
    }

    public override bool IsComplete()
    {
        return Input.GetKeyUp(KeyCode.Space) || Input.touches.Length> 0; // TODO: REPLACE THIS
    }

    public override string AsText()
    {
        return "FLIP!!!";
    }
}

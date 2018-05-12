using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SEngine_State<T>
{
    public T m_owner;

    virtual public void Enter()
    {

    }

    virtual public void Update()
    {

    }

    virtual public void Exit()
    {

    }
}


public class SEngine_StackStateMachine<T>
{
    Dictionary<string, SEngine_State<T>> m_states = new Dictionary<string, SEngine_State<T>>();

    Stack<SEngine_State<T>> m_stateStack = new Stack<SEngine_State<T>>();
    SEngine_State<T> m_lastState = null;
    SEngine_State<T> m_state = null;

    public void Update()
    {
        m_state = m_stateStack.Peek();
        if (m_state != null)
        {
            m_state.Update();
        }

        if (m_lastState != m_state)
        {
            m_state.Enter();
            m_lastState = m_state;
        }
    }

    public void AddState(string stateID, SEngine_State<T> state)
    {
        m_states[stateID] = state;
    }

    public void PushState(string stateID)
    {
        if (m_states.ContainsKey(stateID))
        {
            SEngine_State<T> state = m_states[stateID];
            m_stateStack.Push(state);
        }
    }

    public void PopState()
    {
        m_stateStack.Pop();
    }

    public void SetState(string stateID)
    {
        if (m_states.ContainsKey(stateID))
        {
            SEngine_State<T> state = m_states[stateID];
            if (state != m_state)
            {
                if (m_state != null)
                {
                    m_state.Exit();
                }
                m_state = state;
                m_state.Enter();
                m_state.Update();
            }
        }
    }
}

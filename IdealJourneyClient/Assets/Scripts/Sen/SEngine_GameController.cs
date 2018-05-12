using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneMover))]
public class SEngine_GameController : MonoBehaviour {

    [Header("Configuration")]
    [Header("Internal Information")]
    [Header("Turn Duration Information")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_currentTimeToCompleteAction   = 10.0f;
    [SerializeField] [Range(1, 100)] private int m_decay = 5;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeToCompleteAction   = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeToCompleteAction   = 10.0f;

    [Header("Turn Delay Information")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_currentTimeTilNextTurn        = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeTilNextTurn        = 0.5f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeTilNextTurn        = 2.0f;
    
    [Header("Debug Mode")]
    [SerializeField] private bool m_debugMode = false;

    // Private internal information
    private float       m_timeElapsed = 0.0f;
    private int         m_turnCount = 0;
    private int         m_currentState = -1;
    private String[]    m_commands;
    private bool        m_waitingForNextTurn = false;

    // Private sibling components
    private SceneMover m_sceneMover;

	// Use this for initialization
	void Awake () {
        // Get sibling components
        m_sceneMover = GetComponent<SceneMover>();

        m_commands = new String[]{
            "TAP",
        };

        GetRandomState();
	}
	
	// Update is called once per frame
	void Update () {
        // Iterate timer
        m_timeElapsed += Time.deltaTime;

        // Decide what the timer is doing
        if (m_waitingForNextTurn)
        {
            // The timer exceeded the delay for the next turn
            if(m_timeElapsed > m_currentTimeTilNextTurn)
            {
                SetCompletionTime();
                GetRandomState();
                m_timeElapsed = 0.0f;
                m_waitingForNextTurn = false;

                if(m_debugMode)
                {
                    print(m_commands[m_currentState] + " IT!");
                }
            }
            return;
        }
        else
        {
            // Should the complete the function, add to score, reset timer, and destroy current state
            if (ActionIsComplete())
            {
                // Reset Timer
                m_timeElapsed = 0.0f;
                // Iterate score
                ++m_turnCount;
                // start delay
                m_waitingForNextTurn = true;
                // Set turn delay
                SetTurnDelay();
            }
            // Action was not completed
            else if (m_timeElapsed > m_currentTimeToCompleteAction)
            {
                EndGame();
            }
        }
    }
    private static float QuadraticOut(float t)
    {
        return -(t * (t - 2.0f));
    }

    private void SetCompletionTime()
    {
        float t = Mathf.Clamp01(m_turnCount / m_decay);
        m_currentTimeToCompleteAction = Mathf.Lerp(m_maximumTimeToCompleteAction, m_minimumTimeToCompleteAction, QuadraticOut(t));
    }


    private void SetTurnDelay()
    {
        float timeLeft = m_currentTimeToCompleteAction - m_timeElapsed;
        m_currentTimeTilNextTurn = Mathf.Clamp(timeLeft, m_minimumTimeTilNextTurn, m_maximumTimeTilNextTurn);
    }

    /// <summary>
    /// Processes the action required for someone to carry out
    /// </summary>
    /// <returns>A boolean describing the completion of a requirement</returns>
    private bool ActionIsComplete()
    {
        // needs implementation

        // If action succeeded, return true
        if(Input.GetKey(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    private void GetRandomState()
    {
        m_currentState = 0;
    }

    private void EndGame()
    {
        m_sceneMover.MoveToEnd();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneMover))]
public class GameController : MonoBehaviour
{
    [Header("Configuration")]
    [Header("Internal Information")]
    [Header("Turn Duration Information")]
    [SerializeField] [Range(1, 100)] private int m_decay = 5;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeToCompleteAction   = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeToCompleteAction   = 10.0f;

    [Header("Turn Delay Information")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeTilNextTurn        = 0.5f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeTilNextTurn = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_startBreakTime = 3.0f;

    private GameCommand[] m_commands;
    private MyTimer       m_timer;
    private int           m_currentCommandIndex = -1;
    private System.Random m_randGen = new System.Random();

    private SceneMover m_sceneMover;

    public bool IsWaitingForAction { get; private set; }
    public int CompletedActions { get; private set; }

    public GameCommand CurrentCommand
    {
        get { return m_commands[m_currentCommandIndex]; }
    }

    public float TimeLeft
    {
        get { return m_timer.TimeRemaining; }
    }

    private void Awake ()
    {
        GetSiblingComponents();
        InitializeCommands();
        InitializeTimer();
    }

    private void InitializeCommands()
    {
        m_commands = new GameCommand[]
        {
            //new TapCommand(),
            //new ShakeCommand(),
            //new FlipCommand(),
            //new TurnCommand(),
            new SwipeCommand()
        };
    }

    private void GetSiblingComponents()
    {
        m_sceneMover = GetComponent<SceneMover>();
    }

    private void InitializeTimer()
    {
        m_timer = new MyTimer(m_startBreakTime, OnBreakOver); // start with a break :)
    }

    private void OnOutOfActionTime()
    {
        EndGame();
    }

    private void OnBreakOver()
    {
        BeginAction();
    }

    private void BeginAction()
    {
        SetupActionState();
        SetupActionTimer();
    }

    private void SetupActionState()
    {
        PickNextCommand();
        InitializeNewCommand();
        IsWaitingForAction = true;
    }

    private void InitializeNewCommand()
    {
        CurrentCommand.Begin();
    }

    private void BeginBreak()
    {
        SetupBreakState();
        SetupBreakTimer();
    }

    private void SetupBreakState()
    {
        IsWaitingForAction = false;
    }

    private void SetupActionTimer()
    {
        m_timer.CycleTime = CalculateCompletionTime();
        m_timer.OnCycle = OnOutOfActionTime;
    }

    private void SetupBreakTimer()
    {
        m_timer.CycleTime = CalculateTimeUntilNextTurn();
        m_timer.OnCycle = OnBreakOver;
    }

    private void Update ()
    {
        m_timer.TickTimer(Time.deltaTime);
        if (IsWaitingForAction && ActionIsComplete())
        {
            OnActionCompletedSuccessfully();
        }
    }

    private void OnActionCompletedSuccessfully()
    {
        ++CompletedActions;
        BeginBreak();
    }

    private float CalculateCompletionTime()
    {
        float t = Mathf.Clamp01(CompletedActions / (float)m_decay);
        return Mathf.Lerp(m_maximumTimeToCompleteAction, m_minimumTimeToCompleteAction, QuadraticOut(t));
    }

    private float CalculateTimeUntilNextTurn()
    {
        return Mathf.Clamp(m_timer.TimeRemaining, m_minimumTimeTilNextTurn, m_maximumTimeTilNextTurn);
    }

    private bool ActionIsComplete()
    {
        return CurrentCommand.IsComplete();
    }

    private void PickNextCommand()
    {
        m_currentCommandIndex = m_randGen.Next(m_commands.Length);
    }

    private void EndGame()
    {
        m_sceneMover.MoveToEnd();
    }

    private static float QuadraticOut(float t)
    {
        return -(t * (t - 2.0f));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SceneMover), typeof(AudioSource))]
public class SEngine_GameController : MonoBehaviour
{
    [Header("Configuration")]
    [Header("Internal Information")]
    [Header("Turn Duration Information")]
    [SerializeField] [Range(1, 100)] private int m_decay = 5;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeToCompleteAction = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeToCompleteAction = 10.0f;

    [Header("Turn Delay Information")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeTilNextTurn = 0.5f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeTilNextTurn = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_startBreakTime = 3.0f;

    [Header("Debug - Exposed Variables")]
    [Tooltip("0: Tap\n1: Shake\n2: Flip\n3: Turn\n4: Swipe")]
    [SerializeField] private int m_actionLogSize;
    [SerializeField] private int[] m_actionLog;
    [SerializeField] private float[] m_actionWeight;

    private GameCommand[] m_commands;
    private MyTimer m_timer;
    private int m_currentCommandIndex = -1;

    private SceneMover m_sceneMover;
    private AudioSource m_successSFX;

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

    private void Awake()
    {
        GetSiblingComponents();
        InitializeCommands();
        InitializeTimer();
    }

    private void InitializeCommands()
    {
        m_commands = new GameCommand[]
        {
            new TapCommand(),
            new ShakeCommand(),
            new FlipCommand(),
            new TurnCommand(),
            new SwipeCommand()
        };

        m_actionLog = new int[m_actionLogSize];
        for(int x = 0; x < m_actionLogSize; ++x)
        {
            m_actionLog[x] = -1;
        }
    }

    private void AddToActionLog(int actionIndex)
    {
        for(int x = m_actionLogSize - 2; x > -1; --x)
        {
            m_actionLog[x + 1] = m_actionLog[x];
        }
        m_actionLog[0] = actionIndex;
        ExamineActionLogForRepitition();
    }

    private void ExamineActionLogForRepitition()
    {
        // Stores the frequencies of numbers
        int[] frequencies = new int[m_commands.Length];
        
        for(int x = 0; x < m_actionLogSize; ++x)
        {
            int commandIndex = m_actionLog[x];

            // Ignores invalid locations from initial set up
            if(commandIndex == -1)
            {
                continue;
            }

            // Iterate frequency
            ++frequencies[commandIndex];
        }

        // Calculate inverses

        // Calculate percentages
        // Create frequency board (cached)
        // Pick from frequency board
    }
    private void GetSiblingComponents()
    {
        m_successSFX = GetComponent<AudioSource>();
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
        NotifyActionToDoToUser();
    }

    private void NotifyActionToDoToUser()
    {
        CurrentCommand.PlayVoiceOverSFX();
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

    private void Update()
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
        NotifySuccessToUser();
        BeginBreak();
    }

    private void NotifySuccessToUser()
    {
        AudioHelper.PlaySFX(m_successSFX);
        VibrateHelper.Vibrate();
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
        //AddToActionLog();
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
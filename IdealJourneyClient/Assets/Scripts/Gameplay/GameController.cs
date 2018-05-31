using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SceneMover), typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    [Header("Configuration")]
    [Header("Internal Information")]
    [Header("Turn Duration Information")]
    [SerializeField] [Range(1, 100)] private int m_decay = 5;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_minimumTimeToCompleteAction   = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_maximumTimeToCompleteAction   = 10.0f;

    [Header("Turn Delay Information")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_startBreakTime = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float m_alwaysTimeUntilNextTurn = 3.0f;
    [SerializeField] [Range(0.0f, 3.0f)] private float m_minPitch = 0.75f;
    [SerializeField] [Range(0.0f, 3.0f)] private float m_maxPitch = 1.5f;
    [SerializeField] [Range(0.0f, 3.0f)] private float m_minUVSpeed = 0.1f;
    [SerializeField] [Range(0.0f, 3.0f)] private float m_maxUVSpeed = 0.5f;
    [SerializeField] private AudioSource m_youLoseSFX;
    [SerializeField] private int[] m_milestones = new int[0];
    [SerializeField] private MilestoneText m_milestoneText;
    [SerializeField] private float m_milestoneTime = 1.0f;

    private const int MAX_COMMAND_LOG_LENGTH = 5;
    private GameCommand[] m_commands;
    private int[] m_previousCommandIndices;
    private MyTimer       m_timer;
    private int           m_currentCommandIndex = -1;
    private System.Random m_randGen = new System.Random();
    private AudioSource[] m_sourcesToScale;
    private int m_nextMilestone = 0;

    private SceneMover m_sceneMover;
    private AudioSource m_successSFX;

    public bool IsWaitingForAction { get; private set; }
    public bool DidFail { get; private set; }
    public int CompletedActions { get; private set; }

    public static int LastScore;

    private float CurrentLerpValue
    {
        get
        {
            return QuadraticOut(Mathf.Clamp01(CompletedActions / (float)m_decay));
        }
    }

    private float m_lastCompletedAtPercent = 0.0f;
    public float FillAmount
    {
        get
        {
            return IsWaitingForAction ? m_timer.TimeRemaining / m_timer.CycleTime
                                      : Mathf.Lerp(m_lastCompletedAtPercent, 1.0f, (m_timer.TimeElapsed / m_timer.CycleTime));
        }
    }

    public GameCommand CurrentCommand
    {
        get { return m_commands[m_currentCommandIndex]; }
    }

    public float TimeLeft
    {
        get { return m_timer.TimeRemaining; }
    }

    private ParticleSystem m_cachedMilestoneParticles;
    private ParticleSystem MilestoneParticles
    {
        get
        {
            if (m_cachedMilestoneParticles == null) { m_cachedMilestoneParticles = GameObject.FindGameObjectWithTag(Tags.MILESTONE_PARTICLES).GetComponent<ParticleSystem>(); }
            return m_cachedMilestoneParticles;
        }
    }

    private MyUVScroll m_cachedUVScroll;
    private MyUVScroll UVScroll
    {
        get
        {
            if (m_cachedUVScroll == null) { m_cachedUVScroll = GameObject.FindGameObjectWithTag(Tags.BACKGROUND_OBJECT).GetComponent<MyUVScroll>(); InitUVSpeed(); }
            return m_cachedUVScroll;
        }
    }

    private void Awake ()
    {
        GetSiblingComponents();
        InitializeCommands();
        InitializeCommandLog();
        InitializeTimer();
    }

    private void Start()
    {
        GrabSourcesToScale(); // Needs to be in start so it can grab instantiated on awake audio sources
        InitAudioSpeeds();
    }

    private void InitUVSpeed()
    {
        UVScroll.uvAnimationRate.x = m_minUVSpeed;
    }

    private void InitAudioSpeeds()
    {
        foreach (AudioSource audio in m_sourcesToScale)
        {
            audio.pitch = m_minPitch;
        }
    }

    private void GrabSourcesToScale()
    {
        m_sourcesToScale = new AudioSource[]
        {
            GameObject.FindGameObjectWithTag(Tags.GAMEPLAY_MUSIC).GetComponent<AudioSource>(),
            GameObject.FindGameObjectWithTag(Tags.SHAKE_SFX).GetComponent<AudioSource>(),
            GameObject.FindGameObjectWithTag(Tags.TURN_SFX).GetComponent<AudioSource>(),
            GameObject.FindGameObjectWithTag(Tags.FLIP_SFX).GetComponent<AudioSource>(),
            GameObject.FindGameObjectWithTag(Tags.SWIPE_SFX).GetComponent<AudioSource>(),
            GameObject.FindGameObjectWithTag(Tags.TAP_SFX).GetComponent<AudioSource>(),
            m_youLoseSFX,
            m_successSFX
        };
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
    }

    private void InitializeCommandLog()
    {
        m_previousCommandIndices = new int[MAX_COMMAND_LOG_LENGTH];
        for (int i = 0; i < m_previousCommandIndices.Length; ++i)
        {
            m_previousCommandIndices[i] = m_randGen.Next(m_commands.Length);
        }
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
        DidFail = true;
        LastScore = CompletedActions;
        AudioHelper.PlayVoice(m_youLoseSFX);
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
        ScaleWithSpeed();
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

    private void ScaleWithSpeed()
    {
        UVScroll.uvAnimationRate.x = Mathf.Lerp(m_minUVSpeed, m_maxUVSpeed, CurrentLerpValue);
        float newPitch = Mathf.Lerp(m_minPitch, m_maxPitch, CurrentLerpValue);
        foreach (AudioSource audio in m_sourcesToScale)
        {
            audio.pitch = newPitch;
        }
    }

    private void Update ()
    {
        m_timer.TickTimer(Time.deltaTime);
        if (!DidFail && IsWaitingForAction && ActionIsComplete())
        {
            OnActionCompletedSuccessfully();
        }
    }

    private void OnActionCompletedSuccessfully()
    {
        ++CompletedActions;
        m_lastCompletedAtPercent = m_timer.TimeRemaining / m_timer.CycleTime;
        HandleMilestones();
        NotifySuccessToUser();
        BeginBreak();
    }

    private void HandleMilestones()
    {
        if (HasAchievedMilestone())
        {
            CompleteMilestone();
        }
    }

    private void CompleteMilestone()
    {
        NotifyMilestoneSuccessToUser();
        ++m_nextMilestone;
    }

    private void NotifyMilestoneSuccessToUser()
    {
        MilestoneParticles.gameObject.SetActive(true);
        MilestoneParticles.Play();
        m_milestoneText.Begin(m_milestoneTime, m_milestones[m_nextMilestone] + "!");
    }

    private bool HasAchievedMilestone()
    {
        if (m_nextMilestone >= m_milestones.Length) { return false; } // no milestones left to achieve

        return CompletedActions >= m_milestones[m_nextMilestone];
    }

    private void NotifySuccessToUser()
    {
        AudioHelper.PlaySFX(m_successSFX);
        VibrateHelper.Vibrate();
    }

    private float CalculateCompletionTime()
    {
        return Mathf.Lerp(m_maximumTimeToCompleteAction, m_minimumTimeToCompleteAction, CurrentLerpValue);
    }

    private float CalculateTimeUntilNextTurn()
    {
        return m_alwaysTimeUntilNextTurn;
    }

    private bool ActionIsComplete()
    {
        return CurrentCommand.IsComplete();
    }

    private void PickNextCommand()
    {
        if (m_commands.Length == 0) { Debug.LogError("ERROR: No commands to pick from!"); }
        else if (m_commands.Length == 1) { m_currentCommandIndex = 0; }
        else
        {
            float[] workingArray = new float[m_commands.Length];
            const float MIN = 1.0f;
            for (int i = 0; i < workingArray.Length; ++i)
            {
                workingArray[i] = MIN + m_previousCommandIndices.Count(n => n == i);
            }

            float sum = workingArray.Sum();
            for (int i = 0; i < workingArray.Length; ++i)
            {
                workingArray[i] = sum - workingArray[i];
            }

            sum = workingArray.Sum();
            for (int i = 0; i < workingArray.Length; ++i)
            {
                workingArray[i] /= sum;
            }

            for (int i = 1; i < workingArray.Length; ++i)
            {
                workingArray[i] = workingArray[i - 1] + workingArray[i];
            }

            float choice = (float)m_randGen.NextDouble();
            int chosenIndex = m_commands.Length - 1;

            for (int i = chosenIndex; i >= 0; --i)
            {
                if (choice <= workingArray[i])
                {
                    chosenIndex = i;
                }
            }

            m_currentCommandIndex = chosenIndex;

            for (int x = MAX_COMMAND_LOG_LENGTH - 2; x > -1; --x)
            {
                m_previousCommandIndices[x + 1] = m_previousCommandIndices[x];
            }
            m_previousCommandIndices[0] = m_currentCommandIndex;
        }
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

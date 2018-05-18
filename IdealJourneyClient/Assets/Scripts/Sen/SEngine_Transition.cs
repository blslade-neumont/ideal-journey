using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SceneMover))]
[RequireComponent(typeof(AudioSource))]
public class SEngine_Transition : MonoBehaviour {

    public enum Direction
    {
        IN,
        OUT
    }

    [Header("Configuration")]
    
    [Header("External References")]
    [SerializeField] private RawImage m_image = null;
    [SerializeField] private AudioClip m_in;
    [SerializeField] private AudioClip m_out;

    [Header("Internal Information")]
    [SerializeField] private float m_time = 1.25f;
    [SerializeField] private Direction m_direction = Direction.IN;
    [SerializeField] [Range(100.0f, 0.0f)] private float m_sizeIn = 0.0f;
    [SerializeField] private bool m_startOnAwake = true;

    [Header("Debug Information")]
    [SerializeField] private bool m_debugMode = false;
    [SerializeField] private string m_nextScene = "";


    // Private Internal Information
    private float m_timer = 0.0f;
    private float m_size = 0.0f;
    private bool m_transition = false;

    // Private Sibling Components
    private SceneMover m_sceneMover;
    private AudioSource m_audioSource;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_sceneMover = GetComponent<SceneMover>();

        m_timer = m_time;
        m_size = 0.0f;

        if (m_startOnAwake)
        {
            StartCoroutine(TransitionRoutine());
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            StartTransition(m_time + 0.5f, Direction.OUT, true);
        }
    }

    public void StartTransition(float time, Direction direction, bool transition = false)
    {
        m_time = time;
        m_timer = m_time;
        m_size = 0.0f;
        m_direction = direction;
        m_transition = transition;

        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        if (!m_audioSource.isPlaying)
        {
            if (m_direction == Direction.IN)
            {
                m_audioSource.clip = m_in;
            }
            else
            {
                m_audioSource.clip = m_out;
            }
            m_audioSource.Play();
        }

        while (m_timer > 0.0f)
        {
            m_timer = m_timer - Time.deltaTime;
            float t = (m_direction == Direction.IN) ? (m_timer / m_time) : 1.0f - (m_timer / m_time);
            t = Mathf.Pow(t, 3.0f);
            t = Mathf.Clamp01(t);
            m_size = m_sizeIn * t;

            Rect rect = Rect.zero;
            rect.xMin = -m_size;
            rect.xMax = 1.0f + m_size;
            rect.yMin = -m_size;
            rect.yMax = 1.0f + m_size;
            m_image.uvRect = rect;

            yield return null;
        }
        
        if(m_debugMode)
        {
            print("Transition done");
        }

        if (m_transition)
        {
            SceneManager.LoadScene(m_nextScene);
        }

        StopCoroutine(TransitionRoutine());


        yield return null;
    }
}

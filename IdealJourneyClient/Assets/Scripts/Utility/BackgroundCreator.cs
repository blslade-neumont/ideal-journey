using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundCreator : MonoBehaviour
{
    [SerializeField] private GameObject m_backgroundObject;

    private void Start()
    {
        GameObject backgroundObject = GameObject.FindGameObjectWithTag(Tags.BACKGROUND_OBJECT);
        if (backgroundObject == null)
        {
            backgroundObject = Instantiate(m_backgroundObject);
        }

        backgroundObject.GetComponent<MyUVScroll>().uvAnimationRate.x = SceneManager.GetActiveScene().name == SceneNames.GAME ? 0.1f : 0.0f;
    }
}

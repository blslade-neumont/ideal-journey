using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    [SerializeField] private GameObject m_backgroundMusicPrefab;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag(Tags.TITLE_MUSIC) == null)
        {
            Instantiate(m_backgroundMusicPrefab).GetComponent<AudioSource>().enabled = AudioHelper.BGMEnabled;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MyUVScroll : MonoBehaviour {

    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";
    private Renderer myRenderer;

    Vector2 uvOffset = Vector2.zero;

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (myRenderer.enabled)
        {
            myRenderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}

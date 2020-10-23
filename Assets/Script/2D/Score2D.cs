using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class Score2D : NetworkBehaviour
{
    [SyncVar]
    public int score;

    private TextMesh mesh;

    void Start()
    {
        mesh = GetComponent<TextMesh>();
    }
    
    public void RaiseScore()
    {
        Debug.Log("Raising Score");
        score++;
        mesh.text = score.ToString();
    }
}

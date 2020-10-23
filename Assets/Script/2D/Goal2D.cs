using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class Goal2D : NetworkBehaviour
{
    public Score2D score;

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.gameObject.tag == "Ball") Score(co.gameObject);
    }

    [Command]
    private void Score(GameObject ball)
    {
        ball.transform.position = new Vector3(0, 0, ball.transform.position.z);
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        score.RaiseScore();
    }
}

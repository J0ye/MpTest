using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float destroyAfter = 1;
    public bool debug = false;

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(debug) Debug.Log(gameObject + " hit " + other.gameObject);
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.SendMessage("Hit");
            NetworkServer.Destroy(gameObject);
        }
        else if(!other.gameObject.CompareTag("Player"))
        {
            // Everything else then a Player or an enemy
            NetworkServer.Destroy(gameObject);
        }
    }
}

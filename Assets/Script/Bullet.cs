using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
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

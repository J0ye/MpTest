using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager2D : NetworkManager
{
    public GameObject ballprefab;
    public Vector2 leftSpawn = -Vector2.right;
    public Vector2 rightSpawn = Vector2.right;
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        Vector2 spawn = numPlayers == 0 ? leftSpawn : rightSpawn;
        GameObject player = Instantiate(playerPrefab, spawn, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        if(numPlayers == 2)
        {
            GameObject ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }
    } 
}

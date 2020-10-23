using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager2D : NetworkManager
{
    [Header("Game Settings")]
    public GameObject ballprefab;
    public Vector2 leftSpawn = -Vector2.right;
    public Vector2 rightSpawn = Vector2.right;

    private Dictionary<GameObject, int> players = new Dictionary<GameObject, int>();
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        Vector2 spawn = numPlayers == 0 ? leftSpawn : rightSpawn;
        GameObject player = Instantiate(playerPrefab, spawn, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);
        players.Add(player, conn.connectionId);

        if(numPlayers == 2)
        {
            GameObject ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (!RemovePlayer(conn.connectionId)) Debug.LogError("This was never a player. There is an error in the Manager2D logic.");

        base.OnServerDisconnect(conn);
    }

    public List<GameObject> GetPlayers()
    {
        List<GameObject> temp = new List<GameObject>();

        foreach(KeyValuePair<GameObject, int> kvp in players)
        {
            temp.Add(kvp.Key);
        }

        return temp;
    }

    private bool RemovePlayer(int id)
    {
        foreach(KeyValuePair<GameObject, int> kvp in players)
        {
            if(id == kvp.Value)
            {
                players.Remove(kvp.Key);
                return true;
            }
        }

        return false;
    }
}

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : NetworkManager
{
    public List<GameObject> players = new List<GameObject>();
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        players.Add(player);
        player.name = "Player" + players.Count.ToString();
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}

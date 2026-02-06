using System;
using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class MainNetworkManager : NetworkManager
{
    private List<Player> _players = new();
    private readonly List<int> usedColors = new();

    private bool _isGamePlaying = false;

    public override void OnStartHost()
    {
        base.OnStartHost();
        Player.OnChange += Player_OnReadyChange;
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        Player.OnChange += Player_OnReadyChange;
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        int assignedColor = -1;
        for (int i = 0; i < 8; i++)
        {
            if (!usedColors.Contains(i))
            {
                assignedColor = i;
                break;
            }
        }

        if (assignedColor == -1) return;

        GameObject playerObj = Instantiate(playerPrefab);

        Player player = playerObj.GetComponent<Player>();
        _players.Add(player);
        player.CmdSetColor(assignedColor);

        NetworkServer.AddPlayerForConnection(conn, playerObj);

        usedColors.Add(assignedColor);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            Player player = conn.identity.GetComponent<Player>();
            if (player != null)
            {
                usedColors.Remove(player.ColorId);
                _players.Remove(player);
            }
        }

        base.OnServerDisconnect(conn);
    }

    private void Player_OnReadyChange(Player player)
    {
        if (_isGamePlaying)
        {
            if(_players.Count() - _players.Count(p => p.isDead) <= 1)
            {
                StopGame();
            }
        }
        else
        { 
            if(_players.Count >= 2 && _players.All(p => p.isReady))
            {
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        _isGamePlaying = true;
        Gameplayer.Instance.StartGame();
    }

    private void StopGame()
    {
        _isGamePlaying = false;
        
        var last = _players.First(p => !p.isDead);

        Gameplayer.Instance.RpcShowResults(last);
        Gameplayer.Instance.StopGame();

        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(2);
        foreach (var player in _players)
        {
            player.CmdRestart();
        }
    }
}
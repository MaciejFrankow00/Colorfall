using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Tile : NetworkBehaviour
{
    [SerializeField] private ColorsDatabase _colorsDatabase;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private string _playerTag = "Player";

    private int _colorIndex = -1;
    private List<GameObject> _intersectingPlayers = new();
    private bool _isDeadly = false;

    [Server]
    public void SetColorId(int id)
    {
        _isDeadly = false;
        _colorIndex = id;
        RpcSetColor(_colorsDatabase.GetColor(id).color);
    }

    [Server]
    public void Restart()
    {
        _isDeadly = false;
        _colorIndex = -1;
        RpcSetColor(Color.white);
    }

    [ClientRpc]
    private void RpcSetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    [Server]
    public void SetToDeadly(int exception)
    {
        if(exception == _colorIndex) return;

        _isDeadly = true;

        RpcSetColor(Color.black);

        for(int i = 0; i < _intersectingPlayers.Count; i++)
        {
            _intersectingPlayers[i].GetComponent<Player>().ServerKill();
        }
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_isDeadly)
            {
                collision.gameObject.GetComponent<Player>().ServerKill();
            }
            else
            {
                _intersectingPlayers.Add(collision.gameObject);
            }
        }
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_intersectingPlayers.Contains(collision.gameObject))
            {
                _intersectingPlayers.Remove(collision.gameObject);
            }
        }
    }
}

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

    [ClientRpc]
    public void SetColorId(int id)
    {
        _isDeadly = false;
        _colorIndex = id;
        _spriteRenderer.color = _colorsDatabase.GetColor(id).color;
    }

    [ClientRpc]
    public void Restart()
    {
        _isDeadly = false;
        _colorIndex = -1;
        _spriteRenderer.color = Color.white;
    }

    [ClientRpc]
    public void SetToDeadly(int exception)
    {
        if(exception == _colorIndex) return;

        _isDeadly = true;

        _spriteRenderer.color = Color.black;

        for(int i = 0; i < _intersectingPlayers.Count; i++)
        {
            _intersectingPlayers[i].GetComponent<Player>().CmdKill();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_playerTag))
        {
            if (_isDeadly)
            {
                collision.gameObject.GetComponent<Player>().CmdKill();
            }
            else
            {
                _intersectingPlayers.Add(collision.gameObject);
            }
        }
    }

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

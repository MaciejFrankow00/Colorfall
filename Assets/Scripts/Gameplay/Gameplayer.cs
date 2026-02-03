using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Gameplayer : NetworkBehaviour
{
    public static Gameplayer Instance { get; private set;}

    public event Action<int> OnColorSet;
    public event Action<int> OnCountdownChange;
    public event Action OnGameStart;
    public event Action OnGameStop;
    public event Action<Player> OnWinner;

    [Header("Game objects")]
    [SerializeField] private ColorsDatabase _colorsDatabase;
    [SerializeField] private List<Tile> _tiles;

    [Header("Game config")]
    [SerializeField] private int _colorAmount;
    [SerializeField] private int _timeToMove;
    [SerializeField] private int _timeToReset;

    private Coroutine _gameCorutine;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(_colorsDatabase == null) return;

        if(_colorAmount > _colorsDatabase.ColorsAmount)
        {
            _colorAmount = _colorsDatabase.ColorsAmount;
        }
    }
#endif

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        StopGame();
    }

    [Server]
    public void StartGame()
    {
        _gameCorutine = StartCoroutine(GameLoop());
        RpcSendGameStart();
    }

    [Server]
    public void StopGame()
    {
        if(_gameCorutine == null) return;

        StopCoroutine(_gameCorutine);
        RpcRestartTiles();
        RpcSendGameStop();
    }

    [ClientRpc]
    public void RpcShowResults(Player winner)
    {
        OnWinner?.Invoke(winner);
    }

    [ClientRpc]
    private void RpcSendSetColor(int color)
    {
        OnColorSet?.Invoke(color);
    }

    [ClientRpc]
    private void RpcSendCountdown(int time)
    {
        OnCountdownChange?.Invoke(time);
    }

    [ClientRpc]
    private void RpcSendGameStop()
    {
        OnGameStop?.Invoke();
    }

    [ClientRpc]
    private void RpcSendGameStart()
    {
        OnGameStart?.Invoke();
    }

    [ClientRpc]
    private void RpcRestartTiles()
    {
        for(int i = 0;i < _tiles.Count; i++)
        {
            _tiles[i].Restart();
        }
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            RandomizeTilesColors();
            
            int selectedColor = RandomColor();

            RpcSendSetColor(selectedColor);

            for(int i = _timeToMove; i >= 0; i--)
            {
                yield return new WaitForSeconds(1);
                RpcSendCountdown(i);
            }
            
            foreach(Tile tile in _tiles)
            {
                tile.SetToDeadly(selectedColor);
            }

            yield return new WaitForSeconds(_timeToReset);
        }
    }

    private void RandomizeTilesColors()
    {
        List<Tile> unasignedTiles = new();
        for(int i = 0; i < _tiles.Count; i++)
        {
            unasignedTiles.Add(_tiles[i]);
        }
        int currentColor = 0;

        while(unasignedTiles.Count > 0)
        {
            var randomTile = unasignedTiles[UnityEngine.Random.Range(0,unasignedTiles.Count)];

            unasignedTiles.Remove(randomTile);

            randomTile.SetColorId(currentColor);

            currentColor++;
            if(currentColor >= _colorAmount)
            {
                currentColor -= _colorAmount;
            }
        }
    }

    public int RandomColor()
    {
        return UnityEngine.Random.Range(0, _colorAmount);
    }
}

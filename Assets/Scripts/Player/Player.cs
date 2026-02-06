using System;
using Mirror;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player local;
    public static event Action<Player> OnChange;
    public event Action OnPlayerChange;

    public event Action<bool> OnDeadStatusChange;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ColorsDatabase _colorsDB;

    [SyncVar(hook = nameof(OnColorAssigned))] public int ColorId = -1;
    [SyncVar(hook = nameof(OnReadyChange))] public bool isReady;
    [SyncVar(hook = nameof(OnIsDeadChange))] public bool isDead;

    private void Awake()
    {
        // _deadSprite.gameObject.SetActive(false);
    }

    [Command]
    public void CmdToggleReady()
    {
        isReady = !isReady;
    }

    [Command]
    public void CmdSetReady(bool ready)
    {
        isReady = ready;
    }

    [Command(requiresAuthority = false)]
    public void CmdRestart()
    {
        isDead = false;
        isReady = false;
    }

    [Server]
    public void CmdSetColor(int color)
    {
        ColorId = color;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        local = this;
        RpcSendData();
    }
    private void OnReadyChange(bool oldVal, bool newVal)
    {
        RpcSendData();
    }

    private void OnColorAssigned(int oldVal, int newVal)
    {
        _spriteRenderer.color = _colorsDB.GetColor(newVal).color;
    }

    private void OnIsDeadChange(bool oldVal, bool newVal)
    {
        OnDeadStatusChange?.Invoke(newVal);

        RpcSendData();
    }

    void RpcSendData()
    {
        OnPlayerChange?.Invoke();
        OnChange?.Invoke(this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    [Server]
    public void ServerKill()
    {
        if(isDead) return;
        isDead = true;
    }
}
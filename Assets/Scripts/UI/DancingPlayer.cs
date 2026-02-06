using UnityEngine;

public class DancingPlayer : MonoBehaviour
{
    [SerializeField] private float _rotationTime = 1;
    [SerializeField] private float _targetScale = 12;
    [SerializeField] private Transform _target;

    private float _t;

    void Awake()
    {
        _t = -_rotationTime;
    }

    private void Update()
    {
        _t += Time.deltaTime;
        if( _t > _rotationTime) _t = -_rotationTime;
        _target.localScale = new Vector3(Mathf.Sign(_t),1,1) * _targetScale;
    }
}

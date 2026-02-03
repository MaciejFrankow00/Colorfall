using UnityEngine;

[CreateAssetMenu(fileName = "ColorsDatabase", menuName = "Scriptable Objects/ColorsDatabase")]
public class ColorsDatabase : ScriptableObject
{
    [SerializeField] private ColorScheme[] _colors;
    public int ColorsAmount => _colors.Length;

    public ColorScheme GetColor(int index)
    {
        if(index < 0 || index > _colors.Length) return new ColorScheme();

        return _colors[index];
    }
}

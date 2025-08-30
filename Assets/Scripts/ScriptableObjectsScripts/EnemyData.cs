
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Data",menuName = "Enemy Data",order = 0)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private Sprite _sprite;
    
    [SerializeField]
    private string _name;

    [SerializeField]
    private int _points = 10;
    
    [SerializeField]
    private EAtakType _wekness;

    public Sprite Sprite => _sprite;

    public string Name => _name;

    public int Points => _points;

    public EAtakType Wekness => _wekness;
}

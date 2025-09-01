using Utils;

public static class GameEvents
{
    public delegate void OnEnemyKilled(IEnemy enemy);

    public static OnEnemyKilled EnemyKilled;

    public static GlobalVariable<int> Points = new(0);
}
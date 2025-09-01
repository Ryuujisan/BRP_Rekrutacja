using UnityEngine;

public enum GameLocalization
{
    SWAMPS,
    DUNGEON,
    CASTLE,
    CITY,
    TOWER
}

public class GameControlller : MonoBehaviour
{
    [SerializeField]
    private GameLocalization currentGameLocalization;

    private bool _isPaused;

    public GameLocalization CurrentGameLocalization
    {
        get => currentGameLocalization;

        set => currentGameLocalization = value;
    }

    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;
            Time.timeScale = _isPaused ? 0f : 1f;
        }
    }

    public bool IsCurrentLocalization(GameLocalization localization)
    {
        return CurrentGameLocalization == localization;
    }

    #region Singleton

    private static GameControlller _instance;

    public static GameControlller Instance
    {
        get
        {
            if (_instance == null) _instance = FindFirstObjectByType<GameControlller>();
            return _instance;
        }
        set => _instance = value;
    }

    private void Awake()
    {
        Instance = this;
    }

    #endregion
}
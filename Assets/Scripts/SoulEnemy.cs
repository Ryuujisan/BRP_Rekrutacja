using UiInput;
using UnityEngine;
using UnityEngine.UI;

public enum EAtakType
{
    Melee,
    Range
}

public class SoulEnemy : MonoBehaviour, IEnemy
{
    [SerializeField]
    private GameObject InteractionPanelObject;

    [SerializeField]
    private GameObject ActionsPanelObject;

    [SerializeField]
    private SpriteRenderer EnemySpriteRenderer;

    private SpawnPoint _enemyPosition;

    public EnemyData Data { get; private set; }

    public bool KilledByWeakness { get; private set; }

    public SpawnPoint GetEnemyPosition()
    {
        return _enemyPosition;
    }

    public GameObject GetEnemyObject()
    {
        return gameObject;
    }

    public void SetupEnemy(EnemyData data, SpawnPoint spawnPoint)
    {
        Data = data;
        EnemySpriteRenderer.sprite = data.Sprite;
        _enemyPosition = spawnPoint;
        gameObject.SetActive(true);
    }

    private void ActiveCombatWithEnemy()
    {
        ActiveInteractionPanel(false);
        ActiveActionPanel(true);
    }

    private void ActiveInteractionPanel(bool active)
    {
        InteractionPanelObject.SetActive(active);
    }

    private void ActiveActionPanel(bool active)
    {
        ActionsPanelObject.SetActive(active);
        if (active)
        {
            //EventSystem.current.SetSelectedGameObject(ActionsPanelObject);
            var firstButton = ActionsPanelObject.transform.GetChild(0).gameObject.GetComponent<Button>();
            StartCoroutine(UISelectHelper.GiveFocus(firstButton));
        }
    }

    private void UseBow()
    {
        // USE BOW
        KilledByWeakness = Data.Wekness == EAtakType.Range;
        GameEvents.EnemyKilled?.Invoke(this);
    }

    private void UseSword()
    {
        KilledByWeakness = Data.Wekness == EAtakType.Range;
        GameEvents.EnemyKilled?.Invoke(this);
        // USE SWORD
    }

    #region OnClicks

    public void Combat_OnClick()
    {
        ActiveCombatWithEnemy();
    }

    public void Bow_OnClick()
    {
        UseBow();
    }

    public void Sword_OnClick()
    {
        UseSword();
    }

    #endregion
}


public interface IEnemy
{
    EnemyData Data { get; }

    bool KilledByWeakness { get; }
    SpawnPoint GetEnemyPosition();
    GameObject GetEnemyObject();
}
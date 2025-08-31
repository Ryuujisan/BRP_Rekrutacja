using UiInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EAtakType
{
    Melee, Range
}

public class SoulEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject InteractionPanelObject;
    [SerializeField] private GameObject ActionsPanelObject;
    [SerializeField] private SpriteRenderer EnemySpriteRenderer;

    public EnemyData Data => _data;
    
    public bool KilledByWeakness { get; private set; }
    private SpawnPoint _enemyPosition;
    private EnemyData _data;
    public void SetupEnemy(EnemyData data, SpawnPoint spawnPoint)
    {
        _data = data;
        EnemySpriteRenderer.sprite = data.Sprite;
        _enemyPosition = spawnPoint;
        gameObject.SetActive(true);
    }

    public SpawnPoint GetEnemyPosition()
    {
        return _enemyPosition;
    }

    public GameObject GetEnemyObject()
    {
        return this.gameObject;
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
        KilledByWeakness = _data.Wekness == EAtakType.Range;
        GameEvents.EnemyKilled?.Invoke(this);
    }

    private void UseSword()
    {
        KilledByWeakness = _data.Wekness == EAtakType.Range;
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
    SpawnPoint GetEnemyPosition();
    GameObject GetEnemyObject();
    
    EnemyData Data { get; }
    
    bool KilledByWeakness { get;}
}

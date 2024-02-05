using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour, IInteractionObject
{
    [SerializeField] private EnemyConfigurationSO enemyConfig;
    protected LootSO lootSO;

    private GameObject parentRoom;

    private string enemyName;
    private float maxHealth;
    private float health;
    private float armor;
    private float magicResistance;

    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    private int interactionDistance = 9999;
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyAttack enemyAttack;
    private Sprite enemySprite;
    private Dictionary<string, string> contentToDisplay;
    private bool displayPopup = true;
    Tween enterTweener, exitTweener;
    public float MaxHealth { get => maxHealth; }
    public float Health { get => health; }
    public string Name { get => enemyName; }
    public Sprite Sprite { get => enemySprite; }
    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }


    private void Awake()
    {
        enemySprite = enemyConfig.Sprite;
        maxHealth = enemyConfig.Health;
        armor = enemyConfig.Armor;
        magicResistance = enemyConfig.MagicResistance;
        enemyName = enemyConfig.EnemyName;

        lootSO = enemyConfig.Loot;

        physicalDamageMultiplier = 100 / (100 - armor);
        magicDamageMultiplier = 100 / (100 - magicResistance);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void SetEnemy(GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
        gameObject.transform.SetParent(parentRoom.transform);

        if (enemyMovement != null)
        {
            enemyMovement.SetEnemyMovement(enemyConfig.MovementSpeed, enemyConfig.MinMoveInterval, enemyConfig.MaxMoveInterval, parentRoom);
        }
        if(enemyAttack != null)
        {
            enemyAttack.SetEnemyAttack(enemyConfig.EnemyName,enemyConfig.AttackSpeed, enemyConfig.AttackRange, enemyConfig.TriggerRange, enemyConfig.PhysicalDamage, enemyConfig.MagicDamage, enemyConfig.TrueDamage);
        }
    }

    void Update()
    {
        if(health <= 0)
        {
            Die();
        }
    }


    public void Damage(float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = Mathf.RoundToInt(physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage);
        health -= totalDamage;
        ConsolePanel.instance.EnemyTakeDamage(Name, totalDamage);
    }

    protected virtual void Die()
    {
        Debug.LogWarning("DEAD");
        GameEvents.instance.EnemyClick(null);

        LootManager.instance.CreateLoot(gameObject.transform.position, lootSO.GetContainer(enemyName),lootSO.LootTimeExisting);

        if (enemyConfig.Boss)
        {
            GameEvents.instance.ActivateTeleport();
        }
        gameObject.SetActive(false);
    }

    public void ObjectInteraction(GameObject interactingObject = null)
    {
        DoInteraction();
    }

    public void DoInteraction()
    {
        GameEvents.instance.EnemyClick(this);
        GameEvents.instance.OnCancelGameObjectAction += OnCancelGameObject;
    }

    public void OnMouseEnterObject(Color highLightColor)
    {
        Material[] objectMaterials;
        objectMaterials = gameObject.GetComponent<Renderer>().materials;
        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != highLightColor)
                {
                    if (enterTweener == null)
                    {
                        enterTweener = objMaterial.DOColor(highLightColor, "_Color", 0.5f);
                    }
                    if (exitTweener != null)
                    {
                        exitTweener.Rewind();
                    }

                    enterTweener.Play();
                }
            }
        }
        if (displayPopup)
        {
            SetContentToDisplay(new Dictionary<string, string> { { "Name", enemyName } });
            UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
            displayPopup = false;
        }
    }

    public void OnMouseExitObject()
    {
        Material[] objectMaterials;
        objectMaterials = gameObject.GetComponent<Renderer>().materials;
        if (objectMaterials != null)
        {
            foreach (Material objMaterial in objectMaterials)
            {
                if (objMaterial.color != Color.white)
                {
                    if (exitTweener == null)
                    {
                        exitTweener = objMaterial.DOColor(Color.white, "_Color", 0.5f);
                    }

                    if (enterTweener != null)
                    {
                        enterTweener.Rewind();
                    }

                    exitTweener.Play();
                }
            }
        }
        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
        displayPopup = true;
    }

    private void OnCancelGameObject()
    {
        GameEvents.instance.EnemyClick(null);
        GameEvents.instance.OnCancelGameObjectAction -= OnCancelGameObject;
    }


    private void SetContentToDisplay(Dictionary<string, string> contentDictionary)
    {
        contentToDisplay = new Dictionary<string, string> { };
        foreach (KeyValuePair<string, string> li in contentDictionary)
        {
            contentToDisplay.Add(li.Key, li.Value);
        }
    }
}

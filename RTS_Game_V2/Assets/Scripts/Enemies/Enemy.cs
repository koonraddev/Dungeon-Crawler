using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour, IInteractiveObject
{
    [SerializeField] private EnemyConfigurationSO enemyConfig;
    [SerializeField] private Color highLightObjectColor;
    [SerializeField] private Renderer[] renderers;
    protected LootSO lootSO;
    
    private GameObject parentRoom;

    private string enemyName;
    private float maxHealth, health, armor, magicResistance;

    private float physicalDamageMultiplier;
    private float magicDamageMultiplier;

    private int interactionDistance = 9999, timeAfterDeath = 10;
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] EnemyAnimation enemyAnimation;
    private Sprite enemySprite;
    private Dictionary<string, string> contentToDisplay;
    public float MaxHealth { get => maxHealth; }
    public float Health { get => health; }
    public string Name { get => enemyName; }
    public Sprite Sprite { get => enemySprite; }
    public GameObject GameObject => gameObject;
    public int InteractionDistance { get => interactionDistance; }
    public Dictionary<string, string> ContentToDisplay { get => contentToDisplay; }

    private bool dead;
    private bool doDeathStuff;
    public bool Dead { get => dead; }

 
    private void Awake()
    {
        gameObject.SetActive(false);
        doDeathStuff = true;
    }

    private void OnEnable()
    {
        if (dead)
        {
            health = maxHealth;
            doDeathStuff = true;
            dead = false;
        }

    }

    public void SetEnemy(EnemyConfigurationSO enemyConfig ,GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
        this.enemyConfig = enemyConfig;
        enemySprite = enemyConfig.Sprite;
        maxHealth = enemyConfig.Health;
        health = maxHealth;
        armor = enemyConfig.Armor;
        magicResistance = enemyConfig.MagicResistance;
        enemyName = enemyConfig.EnemyName;

        lootSO = enemyConfig.Loot;

        physicalDamageMultiplier = StatisticalUtility.DamageMultiplier(armor);
        magicDamageMultiplier = StatisticalUtility.DamageMultiplier(magicResistance);


        if (enemyMovement != null)
        {
            enemyMovement.SetEnemyMovement(enemyConfig.MovementSpeed, enemyConfig.MinMoveInterval, enemyConfig.MaxMoveInterval, parentRoom);
        }
        
        if(enemyAttack != null)
        {
            enemyAttack.SetEnemyAttack(enemyConfig.EnemyName,enemyConfig.AttackSpeed, enemyConfig.AttackRange, enemyConfig.TriggerRange, enemyConfig.PhysicalDamage, enemyConfig.MagicDamage, enemyConfig.TrueDamage, enemyConfig.ProjectileAttack, enemyConfig.ProjectilePrefab);
        }

        if(enemyAnimation != null)
        {
            enemyAnimation.SetEnemyAnimator(enemyConfig.AttackSpeed, enemyConfig.MovementSpeed);
        }
    }

    void Update()
    {
        dead = health <= 0;

        if(health <= 0 && doDeathStuff)
        {
            Die();
        }
    }


    public void Damage(float physicalDamage, float magicDamage, float trueDamage)
    {
        float totalDamage = Mathf.RoundToInt(physicalDamage * physicalDamageMultiplier + magicDamage * magicDamageMultiplier + trueDamage);
        health -= totalDamage;
        enemyAnimation.GetHitAnimation();
        ConsolePanel.instance.EnemyTakeDamage(Name, totalDamage);
    }

    protected virtual void Die()
    {
        doDeathStuff = false;
        GameEvents.instance.EnemyClick(null);
        LootManager.instance.CreateLoot(gameObject.transform.position, lootSO.GetContainer(enemyName),parentRoom,lootSO.LootTimeExisting);
        enemyMovement.Dead = true;
        enemyAttack.Dead = true;

        enemyAnimation.DeathAnimation();
        if (enemyConfig.Boss)
        {
            GameEvents.instance.ActivateTeleport();
        }

        Invoke(nameof(WaitAndDeactivate), timeAfterDeath);
    }

    private void WaitAndDeactivate()
    {
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

    private void OnMouseEnter()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(highLightObjectColor, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        SetContentToDisplay(new Dictionary<string, string> { { "Name", enemyName } });
        UIMessageObjectPool.instance.DisplayMessage(this, UIMessageObjectPool.MessageType.POPUP);
    }

    private void OnMouseExit()
    {
        foreach (var item in renderers)
        {
            item.material.DOColor(Color.white, "_BaseColor", 0.5f).SetAutoKill(true).Play();
        }

        GameEvents.instance.CloseMessage(gameObject.GetInstanceID());
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

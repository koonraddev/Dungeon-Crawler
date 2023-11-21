using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BuffManager : MonoBehaviour
{
    private PlayerBasicStatistics playerBaseStats;
    //PUBLIC
    private float playerHP;
    public float PlayerHP
    {
        get => playerHP;
    }
    public PlayerBasicStatistics PlayerBasicStatistics
    {
        get { return playerBaseStats; }
        set 
        { 
            playerBaseStats = value;
        }
    }

    Dictionary<StatisticType, bool> buffs;
    Dictionary<StatisticType, bool> debuffs;

    public static BuffManager instance;

    private List<Buff> buffList;
    private List<Buff> itemsToRemove;

    private bool countBuffTimes = false;
    private bool playerSpawned;

    public List<Buff> Buffs 
    { 
        get => buffList; 
        set 
        {
            foreach (var item in buffList)
            {
                GameEvents.instance.BuffDeactivate(item.StatType, item.StatValue);
            }

            StartCoroutine(WaitForCharacterSpawned(value));
        } 
    }
    private void Awake()
    {
        itemsToRemove = new();
        buffList = new();
        instance = this;
        buffs = new Dictionary<StatisticType, bool>();
        debuffs = new Dictionary<StatisticType, bool>();

        foreach (StatisticType type in Enum.GetValues(typeof(StatisticType)))
        {
            buffs.Add(type, false);
            debuffs.Add(type, false);
        }
    }


    private void OnEnable()
    {
        GameEvents.instance.OnLastRoomReady += CountBuffTimes;
        GameEvents.instance.OnLoadNextLevel += DoNotCountBuffTimes;
        GameEvents.instance.OnPlayerSpawn += OnPlayerSpawned;
        GameEvents.instance.OnStartLevel += CountBuffTimes;
        GameEvents.instance.OnUpdateCurrentHP += OnUpdateHP;
    }

    private void OnUpdateHP(float hpValue)
    {
        playerHP = hpValue;
    }

    private void CountBuffTimes()
    {
        countBuffTimes = true;
    }

    private void DoNotCountBuffTimes()
    {
        countBuffTimes = false;
    }

    private void Update()
    {
        if (countBuffTimes)
        {
            itemsToRemove.Clear();

            foreach (var item in buffList)
            {
                item.Update();
                if (item.TimeLeft <= 0)
                {
                    BuffEnd(item.StatType, item.StatValue);
                    itemsToRemove.Add(item);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                buffList.Remove(itemToRemove);
            }
        }
    }

    private void OnPlayerSpawned()
    {
        playerSpawned = true;
    }

    private IEnumerator WaitForCharacterSpawned(List<Buff> loadedBuffs)
    {
        yield return new WaitUntil(()=> playerSpawned);

        List<Buff> buffs = loadedBuffs;
        foreach (var item in buffs)
        {
            LoadBuff(item.StatType, item.StatValue, item.TimeLeft);
        }
    }



    private void LoadBuff(StatisticType statType, float statValue, float duration)
    {
        Buff bf = new(statType, statValue, duration);
        buffList.Add(bf);
        GameEvents.instance.BuffActivate(statType, statValue, duration);
    }

    public bool Buff(Dictionary<StatisticType, float> buffsDict, float duration)
    {
        var notZero = buffsDict.Where(a => a.Value != 0);

        Debug.Log(notZero);

        foreach (var item in notZero)
        {
            if(item.Key > 0)
            {
                if (!buffs[item.Key])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (!buffs[item.Key])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        foreach (var item in notZero)
        {
            if(item.Key > 0)
            {
                buffs[item.Key] = true;
            }
            else
            {
                debuffs[item.Key] = true;
            }
            Buff bf = new(item.Key, item.Value, duration);
            buffList.Add(bf);
            GameEvents.instance.BuffActivate(item.Key, item.Value, duration);
        }

        return true;
    }

    public void BuffEnd(StatisticType statType, float value)
    {

        if (value > 0)
        {
            buffs[statType] = false;
        }
        else
        {
            debuffs[statType] = false;
        }
        GameEvents.instance.BuffDeactivate(statType, value);
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= CountBuffTimes;
        GameEvents.instance.OnLoadNextLevel -= DoNotCountBuffTimes;
        GameEvents.instance.OnPlayerSpawn -= OnPlayerSpawned;
        GameEvents.instance.OnStartLevel -= CountBuffTimes;
        GameEvents.instance.OnUpdateCurrentHP -= OnUpdateHP;
    }
}

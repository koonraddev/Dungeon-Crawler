using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BuffManager : MonoBehaviour
{

    //Temporary
    public StatisticsSO playerBaseStats;
    //
    Dictionary<StatisticType, bool> buffs;
    Dictionary<StatisticType, bool> debuffs;

    public static BuffManager instance;

    private List<Buff> buffList;
    private List<Buff> itemsToRemove;

    public List<Buff> Buffs 
    { 
        get => buffList; 
        set 
        {
            List<Buff> loadedBuffs = value;
            foreach (var item in loadedBuffs)
            {
                //to do odczytywanie
            }
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

    private void Update()
    {
        itemsToRemove.Clear();

        foreach (var item in buffList)
        {
            item.Update();
            if(item.TimeLeft <= 0)
            {
                Debuff(item.StatType, item.StatValue);
                itemsToRemove.Add(item);
            }
        }

        foreach (var itemToRemove in itemsToRemove)
        {
            buffList.Remove(itemToRemove);
        }
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

    public void Debuff(StatisticType statType, float value)
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
}

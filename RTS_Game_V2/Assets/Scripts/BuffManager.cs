using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BuffManager : MonoBehaviour
{
    Dictionary<StatisticType, bool> buffs;
    Dictionary<StatisticType, bool> debuffs;

    public static BuffManager instance;

    private void Awake()
    {
        instance = this;
        buffs = new Dictionary<StatisticType, bool>();
        debuffs = new Dictionary<StatisticType, bool>();

        foreach (StatisticType type in Enum.GetValues(typeof(StatisticType)))
        {
            buffs.Add(type, false);
            debuffs.Add(type, false);
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

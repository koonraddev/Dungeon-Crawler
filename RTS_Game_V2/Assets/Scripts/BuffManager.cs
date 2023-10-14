using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public bool Buff(Dictionary<StatisticType, float> buffsDict, float duration)
    {
        foreach (var item in buffsDict)
        {
            if(item.Key > 0)
            {
                if (buffs[item.Key])
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
                if (buffs[item.Key])
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        foreach (var item in buffsDict)
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

    public void Debuff(Dictionary<StatisticType, float> debuffsDict)
    {
        foreach (var item in debuffsDict)
        {
            if (item.Key > 0)
            {
                buffs[item.Key] = false;
            }
            else
            {
                debuffs[item.Key] = false;
            }
            GameEvents.instance.BuffDeactivate(item.Key, item.Value);
        }
    }
}

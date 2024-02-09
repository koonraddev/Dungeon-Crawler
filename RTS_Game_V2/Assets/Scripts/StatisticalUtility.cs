using System.Collections;
using System.Collections.Generic;

public static class StatisticalUtility
{
    public static float CalculateDamageMultiplier(float defenceValue)
    {
        if (defenceValue >= 0)
        {
            return 100 / (100 + defenceValue);
        }
        else
        {
            return 2 - (100 / (100 - defenceValue));
        }
    }

    public static float CalculateHealthRegeneration(float maxHP, float points, float percentage)
    {
        return points + (maxHP * (percentage / 100));
    }
}

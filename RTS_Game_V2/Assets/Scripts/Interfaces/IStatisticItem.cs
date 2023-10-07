using System.Collections.Generic;
using UnityEngine;

public interface IStatisticItem
{
    public string Name { get;}
    public string Description { get; }
    public Sprite Sprite { get; }
    public Dictionary<StatisticType, float> Statistics { get;}
}

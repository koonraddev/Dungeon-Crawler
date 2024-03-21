using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoomMarkSettings
{
    [SerializeField] private RoomMarkType roomMarkType;
    [SerializeField] private Color markColor;

    public RoomMarkType RoomMarkType { get => roomMarkType; }
    public Color MarkColor { get => markColor; }
}

public class RoomMark : MonoBehaviour
{
    [SerializeField] private Color activeRoomColor, unactiveRoomColor;
    [SerializeField] private Image foregroundMarkImage, backgroundMarkImage;
    [SerializeField] private List<RoomMarkSettings> markSettingList;

    public void SetRoom(RoomMarkType roomMarkType)
    {
        backgroundMarkImage.color = unactiveRoomColor;
        foreach (var item in markSettingList)
        {
            if(item.RoomMarkType == roomMarkType)
            {
                foregroundMarkImage.color = item.MarkColor;
            }
        }
        DeactivateRoom();
        gameObject.SetActive(false);

    }

    public void ActivateRoom()
    {
        gameObject.SetActive(true);
        backgroundMarkImage.color = activeRoomColor;
    }

    public void DeactivateRoom()
    {
        backgroundMarkImage.color = unactiveRoomColor;
    }
}

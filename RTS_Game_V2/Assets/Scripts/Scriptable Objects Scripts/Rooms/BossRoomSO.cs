using UnityEngine;

[CreateAssetMenu(fileName = "newBossRoom", menuName = "Scriptable Objects/Room/Boss Room")]
public class BossRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private EnemyConfigurationSO bossObject;

    public override PortalSO Portal { get; }

    public override GameObject Plane => roomPlane;

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        if (bossObject != null)
        {
            GameObject boss = Instantiate(bossObject.EnemyPrefab, roomGameObject.transform.position,Quaternion.identity);
            boss.transform.SetParent(roomGameObject.transform);
            boss.GetComponent<Enemy>().SetEnemy(bossObject,roomGameObject);
            boss.SetActive(true);
        }
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.BOSS);
    }
}

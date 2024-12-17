using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Camera minicamera;        // 小地图摄像机
    public Transform player;         // 当前追踪的对象
    public Transform miniplayerIcon; // 小地图上的玩家图标

    void Start()
    {
        if (player == null || minicamera == null || miniplayerIcon == null)
        {
            Debug.LogError("请确保已设置 MinimapController 的字段！");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // 更新小地图摄像机位置（保持高度不变）
            minicamera.transform.position = new Vector3(player.position.x, minicamera.transform.position.y, player.position.z);

            // 更新小地图玩家图标方向
            miniplayerIcon.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
        }
    }

    // 动态切换追踪目标
    public void SetPlayerTarget(Transform newPlayer)
    {
        if (newPlayer != null)
        {
            player = newPlayer;
        }
        else
        {
            Debug.LogWarning("新追踪目标为空！");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class navMove : MonoBehaviour
{
    public SpeedCalculator speedCalculator;
    public Transform[] targetPositions; // 存储 4 个目标点
    public string[] destinationNames = { "家", "公司", "图书馆", "银行" }; // 目标点名称

    public LineRenderer lineRenderer;

    // UI 元素
    public Text destinationText; // 目标点名称
    public Text distanceText;    // 距离
    public Text directionText;   // 方位
    public Text etaText;         // 预计时间

    private NavMeshAgent navmesh;
    private float stoppingDistance = 1.0f; // 到达目标的判断距离

    private bool hasArrived = false; // 标记是否已到达目标点

    private int curIdx = -1;

    private void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();

        if (navmesh == null)
        {
            Debug.LogError("NavMeshAgent 组件未找到！");
            return;
        }

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer 组件未设置！");
            return;
        }

        ResetUI();
    }

    private void Update()
    {
        // 切换目标点
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetDestination(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetDestination(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetDestination(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetDestination(3);

        // 检查是否到达目标
        CheckArrival();

        // 更新路径线和 UI
        if (!hasArrived) // 在未到达目标时更新
        {
            SetPosition();
            UpdateUI(GetCurrentTargetIndex());
        }
    }

    int GetCurrentTargetIndex()
    {
        return curIdx;
    }


    // 设置目标点
    void SetDestination(int index)
    {
        if (targetPositions == null || targetPositions.Length <= index || targetPositions[index] == null)
        {
            Debug.LogWarning($"目标点 {index + 1} 未设置！");
            return;
        }

        curIdx = index;

        navmesh.isStopped = false; // 重新启用导航
        hasArrived = false; // 重置状态

        navmesh.SetDestination(targetPositions[index].position);
        UpdateUI(index);
    }

    // 更新 LineRenderer 显示路径
    void SetPosition()
    {
        if (navmesh.path.corners != null && navmesh.path.corners.Length > 0)
        {
            lineRenderer.positionCount = navmesh.path.corners.Length;
            for (int i = 0; i < navmesh.path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, navmesh.path.corners[i]);
            }
        }
    }

    // 检查是否到达目标点
    void CheckArrival()
    {
        if (!navmesh.pathPending && navmesh.remainingDistance <= stoppingDistance)
        {
            navmesh.isStopped = true; // 停止导航
            hasArrived = true;

            lineRenderer.positionCount = 0; // 清空路径

            distanceText.text = "距离: 已到达目标";
            etaText.text = "预计时间: --";
            directionText.text = "方位: --";
        }
    }

    // 更新 UI 信息
    void UpdateUI(int index = -1)
    {
        if (index >= 0 && index < targetPositions.Length)
        {
            destinationText.text = $"目标: {destinationNames[index]}";

            float distance = 0;

            // 计算当前位置与目标点的距离
            if (targetPositions[index] != null)
            {
                distance = Vector3.Distance(transform.position, targetPositions[index].position);
                distanceText.text = $"距离: {distance:F1} 米";
            }
            else
            {
                distanceText.text = "距离: 未知";
            }



            // 方位
            if (navmesh.hasPath)
            {
                Vector3 direction = (targetPositions[index].position - transform.position).normalized;
                directionText.text = $"方位: {GetDirectionText(direction)}";
            }

            // 速度和预计时间
            float currentSpeed = speedCalculator.currentSpeed;

            float eta = (currentSpeed > 0 && navmesh.remainingDistance > 0) ? distance / currentSpeed : 0;
            etaText.text = $"预计时间: {(eta > 0 ? eta.ToString("F1") : "未知")} 秒";
        }
    }

    void ResetUI()
    {
        destinationText.text = "目标: 未选择";
        distanceText.text = "距离: --";
        directionText.text = "方位: --";
        etaText.text = "预计时间: --";
    }

    // 获取方位字符串
    string GetDirectionText(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // 计算角度
        if (angle < 0) angle += 360; // 将负角度转换为正角度

        // 细化方位描述
        if (angle >= 337.5f || angle < 22.5f) return "北";
        if (angle >= 22.5f && angle < 67.5f) return $"北偏东 {angle - 22.5f:F0} 度";
        if (angle >= 67.5f && angle < 112.5f) return "东";
        if (angle >= 112.5f && angle < 157.5f) return $"南偏东 {angle - 112.5f:F0} 度";
        if (angle >= 157.5f && angle < 202.5f) return "南";
        if (angle >= 202.5f && angle < 247.5f) return $"南偏西 {angle - 202.5f:F0} 度";
        if (angle >= 247.5f && angle < 292.5f) return "西";
        if (angle >= 292.5f && angle < 337.5f) return $"北偏西 {angle - 292.5f:F0} 度";

        return "未知方向";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class navMove : MonoBehaviour
{
    public SpeedCalculator speedCalculator;
    public Transform[] targetPositions; // 存储 4 个目标点
    public string[] destinationNames = { "家", "公司", "图书馆", "银行" }; // 目标点名称

    public LineRenderer fullLineRenderer;    // 用于渲染完整路径的 LineRenderer
    public LineRenderer partialLineRenderer; // 用于渲染 5 米直线的 LineRenderer
    public GuideController guideController;

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
            if (guideController.isNavLineActive)
            {
                RenderFullPath();
            }
            else
            {
                RenderPartialPath();
            }
            UpdateUI(curIdx);
        }
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
    void RenderFullPath()
    {
        if (navmesh.path.corners != null && navmesh.path.corners.Length > 0)
        {
            fullLineRenderer.positionCount = navmesh.path.corners.Length;
            for (int i = 0; i < navmesh.path.corners.Length; i++)
            {
                fullLineRenderer.SetPosition(i, navmesh.path.corners[i]);
            }
        }
    }

    // 十字路口部分 begin
    void RenderPartialPath()
    {
        // 检测附近 20 米范围内是否有交通信号灯
        if (!IsTrafficLightNearby(20f))
        {
            // 没有检测到交通信号灯，清空渲染
            partialLineRenderer.positionCount = 0;
            return;
        }

        // 检测到交通信号灯，继续渲染逻辑
        if (navmesh.path == null || navmesh.path.corners.Length < 2)
        {
            partialLineRenderer.positionCount = 0; // 没有足够路径点时，清空路径
            return;
        }

        Vector3 currentPosition = transform.position;
        float remainingDistance = 15f; // 最大渲染距离 15 米
        float accumulatedDistance = 0f; // 已累加的距离

        Vector3 nextCorner = navmesh.path.corners[1];

        // 开始渲染
        List<Vector3> renderPoints = new List<Vector3> { currentPosition };

        // 遍历路径点，计算累加路径长度
        for (int i = 1; i < navmesh.path.corners.Length; i++)
        {
            Vector3 start = navmesh.path.corners[i - 1];
            Vector3 end = navmesh.path.corners[i];

            // 计算当前路径段的长度
            float segmentDistance = Vector3.Distance(start, end);

            // 判断是否超过剩余的渲染距离
            if (accumulatedDistance + segmentDistance >= remainingDistance)
            {
                // 计算部分路径点（截断当前段以满足 5 米的限制）
                float ratio = (remainingDistance - accumulatedDistance) / segmentDistance;
                Vector3 partialPoint = Vector3.Lerp(start, end, ratio);
                renderPoints.Add(partialPoint);
                break;
            }
            else
            {
                // 当前路径段完全包含在 5 米内
                renderPoints.Add(end);
                accumulatedDistance += segmentDistance;
            }
        }

        // 更新 LineRenderer 渲染
        partialLineRenderer.positionCount = renderPoints.Count;
        for (int i = 0; i < renderPoints.Count; i++)
        {
            partialLineRenderer.SetPosition(i, renderPoints[i]);
        }
    }

    /// <summary>
    /// 检测附近范围内是否存在交通信号灯
    /// </summary>
    /// <param name="radius">检测半径</param>
    /// <returns>是否有交通信号灯</returns>
    bool IsTrafficLightNearby(float radius)
    {
        // 获取范围内的所有碰撞体
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in hitColliders)
        {
            // 判断碰撞体是否是交通信号灯（通过 Tag 或 Layer）
            if (collider.CompareTag("traffic_light")) // 需要为交通灯对象设置 Tag
            {
                return true;
            }
        }
        return false;
    }



    // 十字路口部分 end

    // 检查是否到达目标点
    void CheckArrival()
    {
        if (!navmesh.pathPending && navmesh.remainingDistance <= stoppingDistance)
        {
            navmesh.isStopped = true; // 停止导航
            hasArrived = true;

            fullLineRenderer.positionCount = 0; // 清空路径
            partialLineRenderer.positionCount = 0;

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

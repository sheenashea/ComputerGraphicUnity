using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMove : MonoBehaviour
{
    public Transform[] targetPositions; // 存储 4 个目标点
    public LineRenderer lineRenderer;

    private NavMeshAgent navmesh;
    private float stoppingDistance = 1.0f; // 到达目标的判断距离

    private void Start()
    {
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.enabled = true;
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
    }

    private void Update()
    {
        // 检查按键输入，切换目标点
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetDestination(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetDestination(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetDestination(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetDestination(3);

        // 更新路径线
        SetPosition();

        // 检查是否到达目标点附近
        CheckArrival();
    }

    // 设置目标点
    void SetDestination(int index)
    {
        if (targetPositions == null || targetPositions.Length <= index || targetPositions[index] == null)
        {
            Debug.LogWarning($"目标点 {index + 1} 未设置！");
            return;
        }

        // 设置目标位置
        navmesh.SetDestination(targetPositions[index].position);
        Debug.Log($"已设置目标点 {index + 1}: {targetPositions[index].name}");
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

    // 检查是否到达目标点附近
    void CheckArrival()
    {
        if (navmesh.remainingDistance <= stoppingDistance && !navmesh.pathPending)
        {
            Debug.Log("已到达目标点附近，清空导航路径！");
            lineRenderer.positionCount = 0; // 清空 LineRenderer
        }
    }
}

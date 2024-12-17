using UnityEngine;
using UnityEngine.UI;

public class SpeedCalculator : MonoBehaviour
{
    public float currentSpeed { get; private set; } // 当前速度，只读
    private Vector3 lastPosition; // 记录上一帧的位置

    public Text speedText; // UI Text，用于显示速度

    private float updateInterval = 1.0f; // 更新间隔（单位：秒）
    private float timer = 0f; // 计时器

    void Start()
    {
        // 初始化当前位置
        lastPosition = transform.position;
        currentSpeed = 0f;

        // 检查 speedText 是否绑定
        if (speedText == null)
        {
            Debug.LogWarning("SpeedText UI 未设置，请在 Inspector 面板中绑定。");
        }
    }

    void Update()
    {
        timer += Time.deltaTime; // 累计时间

        if (timer >= updateInterval) // 每 1 秒更新一次
        {
            CalculateSpeed();
            UpdateSpeedUI();
            timer = 0f; // 重置计时器
        }
    }

    void CalculateSpeed()
    {
        // 通过位置变化和时间间隔计算速度
        float distanceTraveled = Vector3.Distance(transform.position, lastPosition);
        currentSpeed = distanceTraveled / updateInterval; // 1 秒内移动的距离除以 1 秒

        // 更新上一帧的位置
        lastPosition = transform.position;
    }

    void UpdateSpeedUI()
    {
        if (speedText != null)
        {
            speedText.text = $"当前速度: {currentSpeed:F2} 米/秒";
        }
    }
}

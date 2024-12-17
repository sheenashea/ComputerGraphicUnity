using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideController : MonoBehaviour
{
    public GameObject navLine;         // 连贯导航线对象
    public GameObject lineSign;        // 指示路标对象
    public GameObject compassUI;       // 指南针 UI 对象
    public Text guideModeText;         // 文本控件，显示当前导航模式

    private bool isARWindowActive;     // 用于判断 AR 窗口是否激活
    public bool isNavLineActive { get; private set; } = false;

    void Start()
    {
        // 初始化默认状态为按键 5 的效果
        ShowNavigationLine();
        isNavLineActive = true;
    }

    void OnEnable()
    {
        isARWindowActive = true; // 当脚本所在的对象激活时，设置为 true
    }

    void OnDisable()
    {
        isARWindowActive = false; // 当脚本所在的对象关闭时，设置为 false
    }

    void Update()
    {
        if (!isARWindowActive) return; // 只有 AR 窗口激活时，才监听按键

        // 按下 5 键，显示连贯导航线
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            isNavLineActive = true;
            ShowNavigationLine();
        }
        // 按下 6 键，显示指示路标
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            isNavLineActive = false;
            ShowLineSign();
        }
        // 按下 7 键，显示指南针
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ShowCompass();
        }
    }

    // 显示连贯导航线，隐藏其他 UI
    void ShowNavigationLine()
    {
        if (navLine != null) navLine.SetActive(true);
        if (lineSign != null) lineSign.SetActive(false);
        if (compassUI != null) compassUI.SetActive(false);

        UpdateGuideModeText("连贯导航线");
    }

    // 显示指示路标，隐藏其他 UI
    void ShowLineSign()
    {
        if (navLine != null) navLine.SetActive(false);
        if (lineSign != null) lineSign.SetActive(true);
        if (compassUI != null) compassUI.SetActive(false);

        UpdateGuideModeText("指示路标");
    }

    // 显示指南针，隐藏其他 UI
    void ShowCompass()
    {
        if (navLine != null) navLine.SetActive(false);
        if (lineSign != null) lineSign.SetActive(false);
        if (compassUI != null) compassUI.SetActive(true);

        UpdateGuideModeText("指南针");
    }

    // 更新导航模式的文本显示
    void UpdateGuideModeText(string mode)
    {
        if (guideModeText != null)
        {
            guideModeText.text = $"指引模式: {mode}";
        }
        else
        {
            Debug.LogWarning("GuideModeText 未设置，请在 Inspector 中分配 UI 文本组件。");
        }
    }
}

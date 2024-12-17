using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    public GameObject navLine;     // 连贯导航线对象
    public GameObject compassUI;   // 指南针 UI 对象

    private bool isARWindowActive; // 用于判断 AR 窗口是否激活

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

        // 按下 5 键，启用连贯导航线，隐藏指南针
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ShowNavigationLine();
        }

        // 按下 7 键，启用指南针，隐藏连贯导航线
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ShowCompass();
        }
    }

    // 显示连贯导航线，隐藏指南针
    void ShowNavigationLine()
    {
        if (navLine != null) navLine.SetActive(true);
        if (compassUI != null) compassUI.SetActive(false);

        Debug.Log("显示连贯导航线");
    }

    // 显示指南针，隐藏连贯导航线
    void ShowCompass()
    {
        if (navLine != null) navLine.SetActive(false);
        if (compassUI != null) compassUI.SetActive(true);

        Debug.Log("显示指南针");
    }
}

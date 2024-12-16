using UnityEngine;

public class ARWindowController : MonoBehaviour
{
    // AR 窗口的引用
    public GameObject arWindow;

    void Start()
    {
        // 确保初始状态是隐藏的
        if (arWindow != null)
        {
            arWindow.SetActive(false);
        }
    }

    void Update()
    {
        // 监听 M 键的按下
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleARWindow();
        }
    }

    // 切换 AR 窗口的显示与隐藏状态
    void ToggleARWindow()
    {
        if (arWindow != null)
        {
            bool isActive = arWindow.activeSelf; // 当前状态
            arWindow.SetActive(!isActive);      // 切换状态
        }
    }
}

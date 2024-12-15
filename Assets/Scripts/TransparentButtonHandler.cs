using UnityEngine;

public class TransparentButtonHandler : MonoBehaviour
{
    // public GameObject minimapUI;  // 小地图 UI
    // public GameObject bigMapUI;  // 大地图 UI

    void Update()
    {
        // 检测按下 M 键
        if (Input.GetKeyDown(KeyCode.M))
        {
            // 切换小地图和大地图的显示状态
            // bool isBigMapActive = bigMapUI.activeSelf;
            // bigMapUI.SetActive(!isBigMapActive);
            // minimapUI.SetActive(isBigMapActive);

            Debug.Log("M key pressed, toggling map view.");
        }
    }
}

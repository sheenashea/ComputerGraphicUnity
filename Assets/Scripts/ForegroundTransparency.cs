using UnityEngine;
using UnityEngine.UI;

public class ForegroundTransparency : MonoBehaviour
{
    public Image foregroundImage; // 拖入Awesome Mask下的Foreground的Image组件
    public float transparencyStep = 0.1f; // 每次按键调节的步进值

    private float currentTransparency = 1f; // 当前透明度

    void Start()
    {
        if (foregroundImage != null)
        {
            currentTransparency = foregroundImage.color.a; // 获取初始透明度
        }
    }

    void Update()
    {
        // 按 + 键增加透明度
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus))
        {
            ChangeTransparency(transparencyStep);
        }

        // 按 - 键减少透明度
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ChangeTransparency(-transparencyStep);
        }
    }

    void ChangeTransparency(float step)
    {
        if (foregroundImage != null)
        {
            currentTransparency = Mathf.Clamp01(currentTransparency + step); // 限定在0到1之间
            Color newColor = foregroundImage.color; // 获取当前颜色
            newColor.a = currentTransparency;       // 修改透明度
            foregroundImage.color = newColor;       // 应用修改后的颜色
        }
    }
}

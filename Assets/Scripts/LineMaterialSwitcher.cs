using UnityEngine;

public class LineMaterialSwitcher : MonoBehaviour
{
    public LineRenderer line1;           // 第一条线的 LineRenderer
    public LineRenderer line2;           // 第二条线的 LineRenderer

    public Material line1BlueMaterial;   // 第一条线的蓝色材质
    public Material line1OrangeMaterial; // 第一条线的橘黄色材质

    public Material line2BlueMaterial;   // 第二条线的蓝色材质
    public Material line2OrangeMaterial; // 第二条线的橘黄色材质

    private bool isOrange = false;       // 当前材质状态

    void Update()
    {
        // 检测 Tab 键输入
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMaterial();
        }
    }

    void ToggleMaterial()
    {
        isOrange = !isOrange;

        if (isOrange)
        {
            // 切换到橘黄色材质
            line1.material = line1OrangeMaterial;
            line2.material = line2OrangeMaterial;
            Debug.Log("切换到橘黄色材质");
        }
        else
        {
            // 切换回蓝色材质
            line1.material = line1BlueMaterial;
            line2.material = line2BlueMaterial;
            Debug.Log("切换回蓝色材质");
        }
    }
}

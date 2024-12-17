using UnityEngine;

public class SwitchViewController : MonoBehaviour
{
    public GameObject firstPersonController; // 第一人称控制器
    public GameObject thirdPersonController; // 第三人称控制器

    public GameObject cameraRigMainCamera;

    private bool isFirstPerson = false; // 当前是否为第一人称视角

    public float firstPersonHeight = 0.1f; // 第一人称视角高度
    public float thirdPersonHeight = 0.1f; // 第三人称视角高度

    public MinimapController minimapController;


    void Start()
    {
        // 确保初始状态，第三人称激活，第一人称关闭
        if (firstPersonController != null && thirdPersonController != null)
        {
            firstPersonController.SetActive(false);
            thirdPersonController.SetActive(true);
            cameraRigMainCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("未分配控制器，请在 Inspector 面板中设置！");
        }
    }

    void Update()
    {
        // 按下 M 键切换视角
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleView();
        }
    }

    void ToggleView()
    {
        if (firstPersonController == null || thirdPersonController == null)
        {
            Debug.LogWarning("控制器未正确设置！");
            return;
        }

        isFirstPerson = !isFirstPerson;

        if (isFirstPerson)
        {
            // 第一人称位置：头部高度
            firstPersonController.transform.position = thirdPersonController.transform.position +
                Vector3.up * firstPersonHeight;
            firstPersonController.transform.rotation = thirdPersonController.transform.rotation;

            thirdPersonController.SetActive(false);
            firstPersonController.SetActive(true);
            cameraRigMainCamera.gameObject.SetActive(false);
            minimapController.SetPlayerTarget(firstPersonController.transform);
        }
        else
        {
            // 第三人称位置：角色背后一定距离和高度
            thirdPersonController.transform.position = firstPersonController.transform.position;
            thirdPersonController.transform.rotation = firstPersonController.transform.rotation;

            firstPersonController.SetActive(false);
            thirdPersonController.SetActive(true);
            cameraRigMainCamera.gameObject.SetActive(true);
            minimapController.SetPlayerTarget(thirdPersonController.transform);
        }
    }

}

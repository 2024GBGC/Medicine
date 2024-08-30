using UnityEngine;

public class MaintainFixedAspectRatio : MonoBehaviour
{
    // 고정할 화면 비율 1280:720 (즉, 16:9)
    private float targetAspectRatio = 16f / 9f;

    void Start()
    {
        UpdateAspectRatio();
    }

    void Update()
    {
        // 매 프레임마다 화면 비율을 유지하도록 체크
        if (Screen.width / (float)Screen.height != targetAspectRatio)
        {
            UpdateAspectRatio();
        }
    }

    void UpdateAspectRatio()
    {
        int targetHeight= Screen.height;
        int targetWidth  = Mathf.RoundToInt(targetHeight / targetAspectRatio);

        if (targetWidth > Screen.width)
        {
            targetWidth = Screen.width;
            targetHeight = Mathf.RoundToInt(targetWidth * targetAspectRatio);
        }

        Screen.SetResolution(targetWidth, targetHeight, false);
    }
}
using UnityEngine;

public class MaintainFixedAspectRatio : MonoBehaviour
{
    // 고정할 화면 비율 1280:720 (즉, 16:9)
    private float targetAspectRatio = 16f / 9f;
    
    // 이전 화면 크기 저장
    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        UpdateAspectRatio();
        StartCoroutine(CheckScreenSizeChange());
    }

    System.Collections.IEnumerator CheckScreenSizeChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초마다 확인

            if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
            {
                UpdateAspectRatio();
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
            }
        }
    }

    void UpdateAspectRatio()
    {
        int targetHeight = Screen.height;
        int targetWidth = Mathf.RoundToInt(targetHeight / targetAspectRatio);

        if (targetWidth > Screen.width)
        {
            targetWidth = Screen.width;
            targetHeight = Mathf.RoundToInt(targetWidth * targetAspectRatio);
        }

        Screen.SetResolution(targetWidth, targetHeight, false);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PillManager : MonoBehaviour
{
    [SerializeField] private Image pillCapsuleImage;
    public Color _color;
    
    public float jumpHeight = 10f;  // 튀어오르는 높이
    public float jumpDuration = 0.5f;  // 튀어오르는 시간

    private void Start()
    {
        pillCapsuleImage.color = _color;
        StartJump();
    }

    public void StartJump()
    {
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        Vector3 originalPosition = transform.localPosition;
        Vector3 targetPosition = originalPosition + Vector3.up * jumpHeight;
        
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, Mathf.Sin((elapsed / jumpDuration) * Mathf.PI));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        Destroy(this.gameObject);
    }
}

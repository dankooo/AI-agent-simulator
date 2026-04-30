using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float moveSpeed = 6f;
    private Vector3 targetPosition;
    private bool moving;

    public void MoveToStation(Transform target)
    {
        if (target == null) return;
        targetPosition = target.position;
        moving = true;
    }

    public void SetEmotion(string emotion)
    {
        Debug.Log($"Robot emotion: {emotion}");
    }

    public void PlayPowerOff() => SetEmotion("...");
    public void PlaySuccess() => SetEmotion("OK!");
    public void PlayError() => SetEmotion("ERR");

    private void Update()
    {
        if (!moving) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f) moving = false;
    }
}

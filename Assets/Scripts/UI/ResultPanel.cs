using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public void Show(bool success, int spentEnergy, string rank, string comment)
    {
        gameObject.SetActive(true);
        Debug.Log($"Result: {(success ? "SUCCESS" : "FAIL")}, spent={spentEnergy}, rank={rank}, {comment}");
    }

    public void Hide() => gameObject.SetActive(false);
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class AnswerPanel : MonoBehaviour
{
    public Action<int> OnAnswerSelected;
    public Action OnNotEnoughDataSelected;

    public List<string> CurrentOptions { get; private set; } = new();

    public void Show(List<string> options)
    {
        CurrentOptions = options ?? new List<string>();
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);

    public void ChooseAnswer(int index) => OnAnswerSelected?.Invoke(index);

    public void ChooseNotEnoughData() => OnNotEnoughDataSelected?.Invoke();
}

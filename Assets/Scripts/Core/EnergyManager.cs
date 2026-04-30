using System;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public int MaxEnergy = 100;
    public int CurrentEnergy { get; private set; }

    public event Action<int, int> OnEnergyChanged;
    public event Action OnEnergyDepleted;
    public event Action OnEnergyLow;
    public event Action OnEnergyCritical;

    public void ResetEnergy(int amount)
    {
        MaxEnergy = amount;
        CurrentEnergy = amount;
        OnEnergyChanged?.Invoke(CurrentEnergy, MaxEnergy);
    }

    public bool CanSpend(int cost) => CurrentEnergy >= cost;

    public bool Spend(int cost)
    {
        if (!CanSpend(cost)) return false;
        CurrentEnergy -= cost;
        OnEnergyChanged?.Invoke(CurrentEnergy, MaxEnergy);

        float pct = MaxEnergy == 0 ? 0 : (float)CurrentEnergy / MaxEnergy;
        if (pct <= 0f) OnEnergyDepleted?.Invoke();
        else if (pct <= 0.2f) OnEnergyCritical?.Invoke();
        else if (pct <= 0.4f) OnEnergyLow?.Invoke();
        return true;
    }

    public bool IsEmpty() => CurrentEnergy <= 0;
}

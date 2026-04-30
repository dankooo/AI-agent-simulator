using UnityEngine;
using UnityEngine.UI;

public class StationController : MonoBehaviour
{
    public string ActionId;
    public string DisplayName;
    public int Cost = 5;
    public Button button;
    public MissionManager missionManager;

    public void OnStationClicked()
    {
        if (missionManager == null) return;
        missionManager.PerformAction(ActionId);
    }

    public void SetInteractable(bool enabled)
    {
        if (button != null) button.interactable = enabled;
    }

    public void SetLowEnergyHighlight(bool low)
    {
        if (button == null) return;
        var colors = button.colors;
        colors.normalColor = low ? new Color(1f, 0.65f, 0.3f) : Color.white;
        button.colors = colors;
    }
}

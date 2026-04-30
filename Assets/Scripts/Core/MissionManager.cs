using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public EnergyManager energyManager;
    public StickerBoard stickerBoard;
    public RobotController robotController;
    public AnswerPanel answerPanel;
    public ResultPanel resultPanel;

    public int answerSelectionCost = 10;
    public int immediateAnswerCost = 5;

    private MissionsCollection missionCollection;
    private MissionData currentMission;
    private int missionIndex;
    private int startEnergy;

    private void Awake()
    {
        LoadMissions();
        HookUI();
    }

    private void Start()
    {
        StartMission(0);
    }

    private void HookUI()
    {
        if (answerPanel == null) return;
        answerPanel.OnAnswerSelected += SelectAnswer;
        answerPanel.OnNotEnoughDataSelected += SelectNotEnoughData;
        answerPanel.Hide();
    }

    private void LoadMissions()
    {
        var text = Resources.Load<TextAsset>("Missions/missions");
        missionCollection = text == null ? new MissionsCollection { missions = new List<MissionData>() } : JsonUtility.FromJson<MissionsCollection>(text.text);
    }

    public void StartMission(int index)
    {
        if (missionCollection?.missions == null || missionCollection.missions.Count == 0) return;

        missionIndex = Mathf.Clamp(index, 0, missionCollection.missions.Count - 1);
        currentMission = missionCollection.missions[missionIndex];
        startEnergy = currentMission.startEnergy;

        stickerBoard.Clear();
        energyManager.ResetEnergy(currentMission.startEnergy);
        resultPanel.Hide();
        answerPanel.Hide();
        robotController.SetEmotion(":)");
        Debug.Log($"Mission started: {currentMission.title} | {currentMission.request}");
    }

    public void RestartMission() => StartMission(missionIndex);

    public void NextMission()
    {
        int next = missionIndex + 1;
        if (next >= missionCollection.missions.Count) next = 0;
        StartMission(next);
    }

    public void PerformAction(string actionId)
    {
        if (currentMission == null || !energyManager.Spend(5)) return;

        var sticker = currentMission.stickers?.Find(s => s.id == actionId);
        if (sticker != null)
        {
            stickerBoard.AddSticker(sticker);
            robotController.SetEmotion("FOUND");
        }

        if (energyManager.IsEmpty())
        {
            CompleteMission(false);
        }
    }

    public void OpenAnswerSelection()
    {
        if (!energyManager.Spend(answerSelectionCost)) return;
        answerPanel.Show(currentMission.answerOptions);
    }

    public void SelectAnswer(int answerIndex)
    {
        bool hasData = stickerBoard.HasRequiredStickers(currentMission.requiredStickers);
        bool success = hasData && answerIndex == currentMission.correctAnswerIndex;
        CompleteMission(success);
    }

    public void SelectNotEnoughData()
    {
        bool hasData = stickerBoard.HasRequiredStickers(currentMission.requiredStickers);
        CompleteMission(!hasData);
    }

    public void CompleteMission(bool success)
    {
        answerPanel.Hide();
        if (success) robotController.PlaySuccess();
        else robotController.PlayError();

        int spent = startEnergy - energyManager.CurrentEnergy;
        string rank = spent <= 20 ? "S" : spent <= 35 ? "A" : "B";
        string comment = success ? "Answer delivered." : "Context lost / wrong answer.";
        resultPanel.Show(success, spent, rank, comment);
    }
}

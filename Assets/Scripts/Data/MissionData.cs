using System;
using System.Collections.Generic;

[Serializable]
public class MissionData
{
    public string id;
    public string title;
    public string request;
    public int startEnergy;
    public List<string> requiredStickers;
    public List<string> optimalActions;
    public List<string> availableActions;
    public List<StickerData> stickers;
    public List<string> answerOptions;
    public int correctAnswerIndex;
}

[Serializable]
public class StickerData
{
    public string id;
    public string text;
}

[Serializable]
public class MissionsCollection
{
    public List<MissionData> missions;
}

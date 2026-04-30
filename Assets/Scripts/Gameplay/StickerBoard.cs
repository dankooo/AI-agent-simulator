using System.Collections.Generic;
using UnityEngine;

public class StickerBoard : MonoBehaviour
{
    private readonly Dictionary<string, StickerData> stickers = new();

    public void AddSticker(StickerData sticker)
    {
        if (sticker == null || string.IsNullOrEmpty(sticker.id)) return;
        stickers[sticker.id] = sticker;
    }

    public bool HasSticker(string stickerId) => stickers.ContainsKey(stickerId);

    public bool HasRequiredStickers(List<string> requiredIds)
    {
        if (requiredIds == null || requiredIds.Count == 0) return true;
        foreach (var id in requiredIds)
        {
            if (!stickers.ContainsKey(id)) return false;
        }
        return true;
    }

    public void Clear() => stickers.Clear();
}

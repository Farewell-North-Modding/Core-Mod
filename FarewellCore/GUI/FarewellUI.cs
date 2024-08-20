using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace FarewellCore.GUI;

[RegisterTypeInIl2Cpp]
public class FarewellUI : MonoBehaviour
{
    private void Start()
    {
        var canvasScaler = gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        gameObject.AddComponent<HorizontalLayoutGroup>();
    }

    public static FarewellUI Create(string name)
    {
        var go = new GameObject(name);
        var ui = go.AddComponent<FarewellUI>();
        var canvas = go.AddComponent<Canvas>();
        canvas.sortingOrder = 20000;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        go.AddComponent<GraphicRaycaster>();
        return ui;
    }
}
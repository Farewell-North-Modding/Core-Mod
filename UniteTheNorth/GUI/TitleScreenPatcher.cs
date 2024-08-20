using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace UniteTheNorth.GUI;

[RegisterTypeInIl2Cpp]
public class TitleScreenPatcher : MonoBehaviour
{
    private const string LogoLocation = "/UniteTheNorth/UniteTheNorth.png";
    
    public static void Patch(string sceneName)
    {
        if(sceneName != "Main Menu")
            return;
        new GameObject("UTNTitleScreenPatcher").AddComponent<TitleScreenPatcher>();
    }

    private void Start()
    {
        ReplaceArtwork();
        Invoke(nameof(AddConnectUI), 2f);
        UniteTheNorth.Logger.Msg("Applied Title Screen Patches");
    }

    private static void ReplaceArtwork()
    {
        var fullPath = MelonEnvironment.ModsDirectory + LogoLocation;
        if (!File.Exists(fullPath))
        {
            UniteTheNorth.Logger.Warning("Couldn't find custom title screen artwork");
            return;
        }
        var data = File.ReadAllBytes(fullPath);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(data);
        var rect = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
        var obj = GameObject.Find("Logo");
        if(obj == null)
            UniteTheNorth.Logger.Warning("Couldn't find logo game object");
        else
            obj.GetComponent<Image>().sprite = sprite;
    }

    private void AddConnectUI()
    {
        //var uiBase = UniversalUI.RegisterUI("com.limo.UniteTheNorth", () => { });
        //var connectPanel = new ConnectPanel(uiBase);
    }
}
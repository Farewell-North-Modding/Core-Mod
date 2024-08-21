using FarewellCore.GUI;
using Il2CppKBCore.Localization;
using Il2CppKBCore.UI;
using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using MelonLoader.Utils;
using UnityEngine;
using UnityEngine.UI;

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
        ReplaceButtons();
        UniteTheNorth.Logger.Msg("Applied Title Screen Patches");
    }

    private static void CreateConnectUI(bool isHost)
    {
        // Initialize Panel
        var canvas = GameObject.Find("Canvas");
        var container = canvas.transform.GetChild(0).gameObject;
        var panel = FarewellUI.CreatePanel(canvas.transform);
        panel.AddHeader(isHost ? "Host Server" : "Join Server");
        // Initialize Components
        var username = panel.AddInputField("Enter Username", label: "Username", onValueChanged: val =>
        {
            UniteTheNorth.Logger.Msg($"Username: {val}");
        });
        var address = panel.AddInputField("Enter IP Address", label: "Server Address", onValueChanged: val =>
        {
            UniteTheNorth.Logger.Msg($"Address: {val}");
        });
        address.transform.parent.parent.parent.gameObject.SetActive(!isHost);
        var port = panel.AddInputField("4657", label: "Server Port", onValueChanged: val =>
        {
            UniteTheNorth.Logger.Msg($"Port: {val}");
        });
        var password = panel.AddInputField("No Password", label: "Server Password", onValueChanged: val =>
        {
            UniteTheNorth.Logger.Msg($"Password: {val}");
        });
        var bottomBar = panel.AddLayout(true);
        var cancel = bottomBar.AddBoxButton("Cancel", () =>
        {
            container.SetActive(true);
            Destroy(panel.gameObject);
        }, navigation: FarewellUI.CreateNavigationPath());
        var start = bottomBar.AddBoxButton(isHost ? "Host" : "Connect", () => UniteTheNorth.Logger.Msg("Ohio"), navigation: FarewellUI.CreateNavigationPath());
        // Finish Up
        canvas.GetComponent<UIFocus>().SetSelection(username);
        container.SetActive(false);
        panel.gameObject.transform.localPosition = new Vector3(0, 300, 0);
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

    private void ReplaceButtons()
    {
        // Host Button
        var continueObj = GameObject.Find("Continue");
        continueObj.transform.name = "Host";
        var host = continueObj.GetComponent<Button>();
        DestroyImmediate(host.GetComponentInChildren<LocalizedTextMeshPro>());
        var hostTmp = host.GetComponentInChildren<RTLTextMeshPro>();
        hostTmp.originalText = "Host Server";
        hostTmp.text = "Host Server";
        hostTmp.SetText("Host Server");
        host.onClick = new Button.ButtonClickedEvent();
        host.onClick.AddListener(new Action(() =>
        {
            CreateConnectUI(true);
        }));
        // Join Button
        var newGameObj = GameObject.Find("New Game");
        newGameObj.transform.name = "Join";
        var join = newGameObj.GetComponent<Button>();
        DestroyImmediate(join.GetComponentInChildren<LocalizedTextMeshPro>());
        var joinTmp = join.GetComponentInChildren<RTLTextMeshPro>();
        joinTmp.originalText = "Join Server";
        joinTmp.text = "Join Server";
        joinTmp.SetText("Join Server");
        join.onClick = new Button.ButtonClickedEvent();
        join.onClick.AddListener(new Action(() =>
        {
            CreateConnectUI(false);
        }));
    }
}
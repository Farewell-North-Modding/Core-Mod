using FarewellCore.GUI;
using Il2CppFarewellNorth.UI.Menu.Main;
using Il2CppKBCore.Localization;
using Il2CppKBCore.UI;
using Il2CppRTLTMPro;
using Il2CppTMPro;
using MelonLoader;
using MelonLoader.Utils;
using UniteTheNorth.Networking;
using UniteTheNorth.Systems;
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
        var focus = canvas.GetComponent<UIFocus>();
        var container = canvas.transform.GetChild(0).gameObject;
        var panel = FarewellUI.CreatePanel(canvas.transform);
        panel.AddHeader(isHost ? "Host Server" : "Join Server");
        // Initialize Components
        var username = panel.AddInputField("Enter Username", LocalNetworkManager.Username, label: "Username", onValueChanged: val =>
        {
            LocalNetworkManager.Username = val;
        });
        var address = panel.AddInputField("Enter IP Address", LocalNetworkManager.Address, label: "Server Address", onValueChanged: val =>
        {
            LocalNetworkManager.Address = val;
        });
        address.transform.parent.parent.parent.gameObject.SetActive(!isHost);
        var port = panel.AddInputField("4657", LocalNetworkManager.Port != "4657" ? LocalNetworkManager.Port : "", label: "Server Port", onValueChanged: val =>
        {
            LocalNetworkManager.Port = val;
        });
        port.contentType = TMP_InputField.ContentType.IntegerNumber;
        var password = panel.AddInputField("No Password", LocalNetworkManager.Password, label: "Server Password", onValueChanged: val =>
        {
            LocalNetworkManager.Password = val;
        });
        password.contentType = TMP_InputField.ContentType.Password;
        var bottomBar = panel.AddLayout(true);
        bottomBar.AddBoxButton("Cancel", () =>
        {
            container.SetActive(true);
            Destroy(panel.gameObject);
        }, navigation: FarewellUI.CreateNavigationPath());
        bottomBar.AddBoxButton(isHost ? "Host" : "Join", () =>
        {
            // Check for invalid values
            if (LocalNetworkManager.Username == "")
            {
                focus.SetSelection(username);
                return;
            }
            if (LocalNetworkManager.Address == "" && !isHost)
            {
                focus.SetSelection(address);
                return;
            }
            if (LocalNetworkManager.Port == "" || !int.TryParse(LocalNetworkManager.Port, out var portNumber) || portNumber < 0 || portNumber > 65535)
            {
                focus.SetSelection(port);
                return;
            }
            if (!isHost)
            {
                TryConnect(canvas);
                return;
            }
            container.SetActive(true);
            Destroy(panel.gameObject);
            canvas.transform.parent.GetComponent<MainMenuController>().ContinueGame();
        });
        // Finish Up
        focus.SetSelection(username);
        container.SetActive(false);
        LocalNetworkManager.HostServer = isHost;
        panel.gameObject.transform.localPosition = new Vector3(0, 300, 0);
    }

    private static void TryConnect(GameObject canvas)
    {
        canvas.SetActive(false);
        FarewellUI.CreateFarewellUI(ui =>
        {
            var panel = ui.AddPanel();
            panel.transform.localPosition = new Vector3(0, 100, 0);
            panel.AddHeader("Connecting").alignment = TextAlignmentOptions.Center;
            var status = panel.AddLabel("Connection is being established...");
            LocalNetworkManager.SupplyStatusLabel(status);
            status.alignment = TextAlignmentOptions.Bottom;
            var cancelButton = panel.AddBoxButton("Cancel", () =>
            {
                Client.Instance?.NetClient?.DisconnectAll();
            });
            ui.GetComponent<UIFocus>().SetSelection(cancelButton._button);
            LocalNetworkManager.InitializeLocal();
        });
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
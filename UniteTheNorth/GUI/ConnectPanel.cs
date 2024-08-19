using UniteTheNorth.Systems;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Panels;

namespace UniteTheNorth.GUI;

public class ConnectPanel : PanelBase
{
    public ConnectPanel(UIBase owner) : base(owner) { }
    
    public override string Name => "Unite the North Connection Settings";
    public override int MinWidth => 400;
    public override int MinHeight => 200;
    public override Vector2 DefaultAnchorMin => new(.5f, .5f);
    public override Vector2 DefaultAnchorMax => new(.5f, .5f);

    protected override void ConstructPanelContent()
    {
        UIFactory.SetLayoutElement(ContentRoot, minWidth: 400, minHeight: 200);
        UIFactory.CreateToggle(ContentRoot, "HostServer", out var serverToggle, out var hostToggleText);
        serverToggle.onValueChanged.AddListener(new Action<bool>(val =>
        {
            LocalNetworkManager.HostServer = val;
        }));
        hostToggleText.text = "Host Server?";
        UIFactory.SetLayoutElement(serverToggle.gameObject, minWidth: 400, minHeight: 25);
        var port = UIFactory.CreateInputField(ContentRoot, "Port", "Port (Default 4657)");
        UIFactory.SetLayoutElement(port.GameObject, minWidth: 400, minHeight: 25);
        port.OnValueChanged += val => LocalNetworkManager.Port = val == "" ? "4657" : "";
        var address = UIFactory.CreateInputField(ContentRoot, "Address", "Server Address");
        UIFactory.SetLayoutElement(address.GameObject, minWidth: 400, minHeight: 25);
        address.OnValueChanged += val => LocalNetworkManager.Address = val;
        var username = UIFactory.CreateInputField(ContentRoot, "Username", "Your Username");
        UIFactory.SetLayoutElement(username.GameObject, minWidth: 400, minHeight: 25);
        username.OnValueChanged += val => LocalNetworkManager.Username = val;
        var password = UIFactory.CreateInputField(ContentRoot, "Password", "Server Password");
        UIFactory.SetLayoutElement(password.GameObject, minWidth: 400, minHeight: 25);
        password.OnValueChanged += val => LocalNetworkManager.Password = val;
        var infoText = UIFactory.CreateLabel(ContentRoot, "InfoText", "The Address is ignored if you are the host!");
        UIFactory.SetLayoutElement(infoText.gameObject, minWidth: 400, minHeight: 25);
    }
}
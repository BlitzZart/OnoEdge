//#if ENABLE_UNET


using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


[RequireComponent(typeof(NetworkManager))]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class NetworkUI : MonoBehaviour
{
    public NetworkManager manager;
    [SerializeField]
    public bool showGUI = true;
    [SerializeField]
    public int offsetX;
    [SerializeField]
    public int offsetY;
    [SerializeField]
    public float scale = 2.0f;

    public float scaleScale = 0.5f;

    // Runtime variable
    bool showServer = false;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    // try to scale UI to device sceen
    private void AutoScale()
    {
        scale = Screen.width / 210.0f * scaleScale;
    }

    void Update()
    {
        if (!showGUI)
            return;

        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                manager.StartServer();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                manager.StartHost();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                manager.StartClient();
            }
        }
        if (NetworkServer.active && NetworkClient.active)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                manager.StopHost();
            }
        }
        AutoScale();
    }

    void OnGUI()
    {
        if (!showGUI)
            return;

        GUI.skin.label.fontSize = 
            GUI.skin.button.fontSize = 
            GUI.skin.textField.fontSize = (int)(8 * scale);

        int xpos = 10 + offsetX;
        int ypos = 40 + offsetY;
        int spacing = (int)(24 * scale);

        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "LAN Host(H)"))
            {
                manager.StartHost();
            }
            ypos += spacing;

            if (GUI.Button(new Rect(xpos, ypos, 105 * scale, 20 * scale), "LAN Client(C)"))
            {
                manager.StartClient();
            }


            manager.networkAddress = GUI.TextField(new Rect((xpos + 100) * scale, ypos, 95 * scale, 20 * scale), manager.networkAddress);
            ypos += spacing;

            //GUI.Label(new Rect(xpos, ypos, 100 * scale, 20 * scale), "Own IP: " + Network.natFacilitatorIP);
            //ypos += spacing;
            GUI.Label(new Rect(xpos, ypos, 100 * scale, 20 * scale), "Own IP: " + Network.player.ipAddress);
            ypos += spacing;


            if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "LAN Server Only(S)"))
            {
                manager.StartServer();
            }
            ypos += spacing;

        }
        else
        {
            if (NetworkServer.active)
            {
                GUI.Label(new Rect(xpos, ypos, 300 * scale, 20 * scale), "Server: port=" + manager.networkPort);
                ypos += spacing;
            }
            if (NetworkClient.active)
            {
                GUI.Label(new Rect(xpos, ypos, 300 * scale, 20 * scale), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                ypos += spacing;
            }
        }

        if (NetworkClient.active && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Client Ready"))
            {
                ClientScene.Ready(manager.client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }

        if (NetworkServer.active || NetworkClient.active)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Stop (X)"))
            {
                manager.StopHost();
            }
            ypos += spacing;
        }

        if (!NetworkServer.active && !NetworkClient.active)
        {
            ypos += 10;

            if (manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Enable Match Maker (M)"))
                {
                    manager.StartMatchMaker();
                }
                ypos += spacing * 2;
                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Close App"))
                {
                    Application.Quit();
                }
                ypos += spacing;
            }
            else
            {
                if (manager.matchInfo == null)
                {
                    if (manager.matches == null)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Create Internet Match"))
                        {
                            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
                        }
                        ypos += spacing;

                        GUI.Label(new Rect(xpos, ypos, 100 * scale, 20 * scale), "Room Name:");
                        manager.matchName = GUI.TextField(new Rect((xpos + 100) * scale, ypos, 95 * scale, 20 * scale), manager.matchName);
                        ypos += spacing;

                        ypos += 10;

                        if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Find Internet Match"))
                        {
                            manager.matchMaker.ListMatches(0, 20, "", manager.OnMatchList);
                        }
                        ypos += spacing;
                    }
                    else
                    {
                        foreach (var match in manager.matches)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Join: " + match.name + " | " + match.currentSize))
                            {
                                manager.matchName = match.name;
                                manager.matchSize = (uint)match.currentSize;
                                manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                            }
                            ypos += spacing;
                        }
                    }
                }

                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Change MM server"))
                {
                    showServer = !showServer;
                }
                if (showServer)
                {
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100 * scale, 20 * scale), "Local"))
                    {
                        manager.SetMatchHost("localhost", 1337, false);
                        showServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100 * scale, 20 * scale), "Internet"))
                    {
                        manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                        showServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100 * scale, 20 * scale), "Staging"))
                    {
                        manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                        showServer = false;
                    }
                }

                ypos += spacing;

                GUI.Label(new Rect(xpos, ypos, 300 * scale, 20 * scale), "MM Uri: " + manager.matchMaker.baseUri);
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200 * scale, 20 * scale), "Disable Match Maker"))
                {
                    manager.StopMatchMaker();
                }
                ypos += spacing;
            }
        }
    }
}
//#endif //ENABLE_UNET

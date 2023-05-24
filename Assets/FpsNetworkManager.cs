using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FpsNetworkManager : NetworkManager
{
    [SerializeField] private GameObject EnterAddressPanel = null, landingPage = null, lobbyUI = null;
    [SerializeField] private TMP_InputField AddressField = null;
    [SerializeField] private GameObject StartGameButton = null;
    [SerializeField] private GameObject PlayerGO = null;
    public List<PlayerScript> PlayersList = new List<PlayerScript>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        PlayerScript PlayerStartPrefab = conn.identity.GetComponent<PlayerScript>();
        PlayersList.Add(PlayerStartPrefab);

        if (PlayersList.Count == 2)
        {
            StartGameButton.SetActive(true);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        PlayerScript playerStartPrefab = conn.identity.GetComponent<PlayerScript>();
        PlayersList.Remove(playerStartPrefab);
        StartGameButton.SetActive(false);
    }

    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinButton()
    {
        EnterAddressPanel.SetActive(true);
        landingPage.SetActive(true);
    }

    public void JoinLobby()
    {
        NetworkManager.singleton.networkAddress = AddressField.text;
        NetworkManager.singleton.StartClient();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        lobbyUI.SetActive(true);
        EnterAddressPanel.SetActive(false);
        landingPage.SetActive(false);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        landingPage.SetActive(true);
        lobbyUI.SetActive(false);
        EnterAddressPanel.SetActive(false);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    public void StartGame()
    {
        ServerChangeScene("GameScene");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (SceneManager.GetActiveScene().name.StartsWith("GameScene"))
        {
            foreach (PlayerScript player in PlayersList)
            {
                var connectionTC = player.connectionToClient;
                GameObject PlayerP = Instantiate(PlayerGO, GetStartPosition().transform.position, Quaternion.identity);
                NetworkServer.ReplacePlayerForConnection(connectionTC, PlayerP);
            }
        }
    }
}

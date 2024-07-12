using FishNet.Managing;
using FishNet.Managing.Transporting;
using FishNet.Transporting.UTP;
using System.Net;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

/// <summary>
/// Master Class that controls most, if not all, Gaming Services-related function.<br></br>
/// This is everything from authentication to multiplayer connections, so it'll likely be quite big.
/// </summary>
public class ServicesManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public TransportManager transportManager;
    public FishyUnityTransport localTransport, relayTransport;




    [Tooltip("If true at awake, we'll take the game to be in offline mode. Otherwise, we'll check if the user can reach the internet.")]
    public bool offlineMode;
    [HideInInspector] public bool offlineModeFlag;

    public static ServicesManager servicesManager;

    public async void Awake()
    {
        if (!InitialiseManager())
        {
            return;
        }
        if (offlineMode || Application.internetReachability == NetworkReachability.NotReachable)
        {
            offlineModeFlag = true;
            offlineMode = offlineModeFlag;
            Debug.LogWarning("Launching game in Offline Mode!");
            FinishInitialising();
            return;
        }

        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (ServicesInitializationException e)
        {
            throw e;
        }
        PlayerAccountLogin();
    }
    public async void PlayerAccountLogin()
    {
        try
        {
           await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch (PlayerAccountsException e)
        {
            throw e;
        }

        print(PlayerAccountService.Instance.AccessToken);

        try
        {
            var so = new SignInOptions()
            {
            };
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken,so);
        }
        catch (AuthenticationException e)
        {
            throw e;
        }
    }

    public void FinishInitialising()
    {
        DontDestroyOnLoad(gameObject);
        UIManager.Instance.SendLoadScreen(1, LoadMainMenu);
    }
    public async void LoadMainMenu()
    {
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneContainer.Instance.mainMenu.BuildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public bool InitialiseManager()
    {
        if(servicesManager == null)
        {
            servicesManager = this;
            print("Assigned the Services Manager");
            return true;
        }
        else
        {
            print("Destroyed an extra Services Manager.");
            return false;
        }
    }
}

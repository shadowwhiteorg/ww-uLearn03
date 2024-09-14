using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    //private string m_LogoAddress;

    [SerializeField]
    private AssetReferenceSprite m_LogoAssetReference;

    private AsyncOperationHandle<Sprite> m_LogoLoadOpHadle;

    private static AsyncOperationHandle<SceneInstance> m_SceneLoadOpHandle;

    public static GameManager Instance {get; private set;}
    
    public static int s_CurrentLevel = 0;

    public static int s_MaxAvailableLevel = 5;

    // The value of -1 means no hats have been purchased
    public static int s_ActiveHat = 0;

    [SerializeField] private Image m_gameLogoImage;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnEnable()
    {
        // When we go to the 
        s_CurrentLevel = 0;

        //var logoResourceRequest = Resources.LoadAsync<Sprite>("LoadyDungeonsLogo");
        //logoResourceRequest.completed += (asyncOperation) =>
        //{
        //    m_gameLogoImage.sprite = logoResourceRequest.asset as Sprite;
        //};


        if (!m_LogoAssetReference.RuntimeKeyIsValid())
            return;

        m_LogoLoadOpHadle = Addressables.LoadAssetAsync<Sprite>(m_LogoAssetReference);

        m_LogoLoadOpHadle.Completed += OnLogoLoadComplete;

    }

    private void OnLogoLoadComplete(AsyncOperationHandle<Sprite> handle)
    {
        Debug.Log($"Logo Load Status: " + handle.Status);

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            m_gameLogoImage.sprite = handle.Result;
        }
    }

    public void ExitGame()
    {
        s_CurrentLevel = 0;
    }

    public void SetCurrentLevel(int level)
    {
        s_CurrentLevel = level;
    }

    public static void LoadNextLevel()
    {
        // we will change the scene loading type to addressable system
        //SceneManager.LoadSceneAsync("LoadingScene");

        m_SceneLoadOpHandle = Addressables.LoadSceneAsync("LoadingScene", activateOnLoad: true);

    }

    public static void LevelCompleted()
    {
        s_CurrentLevel++;

        // Just to make sure we don't try to go beyond the allowed number of levels.
        s_CurrentLevel = s_CurrentLevel % s_MaxAvailableLevel;

        LoadNextLevel();
    }

    public static void ExitGameplay()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}

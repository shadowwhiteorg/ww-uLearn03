using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Used for the Hat selection logic
public class PlayerConfigurator : MonoBehaviour
{

    [SerializeField]
    private Transform m_HatAnchor;

    private ResourceRequest m_HatLoadingRequest;

    // following variable may cause future priblems like typos so we will replace it with AssetReference
    //[SerializeField]
    //private string m_Address;

    // AssetReference struct will change to prevent the selection of wrong type of assets
    //[SerializeField]
    //private AssetReference m_HatAssetReference;

    //[SerializeField] 
    //AssetReferenceGameObject m_HatAssetReference;

    private GameObject m_HatInstance;


    private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;

    void Start()
    {
        //SetHat(string.Format("Hat{0:00}", GameManager.s_ActiveHat));
        LoadInRandomHat();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Destroy(m_HatInstance);
            Addressables.ReleaseInstance(m_HatLoadOpHandle);

            LoadInRandomHat();
        }
    }


    private void LoadInRandomHat()
    {
        int randomIndex = Random.Range(0, 6);
        string hatAddress = string.Format("Hat{0:00}", randomIndex);

        m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(hatAddress);
        m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }

    //public void SetHat(string hatKey)
    //{
    //    /// we will change the following lines to replace the resource loading system with the addressable system
    //    //m_HatLoadingRequest = Resources.LoadAsync(hatKey);
    //    //m_HatLoadingRequest.completed += OnHatLoaded;

    //    // due to m_address variable has removed, this line is obsolute
    //    //m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(m_Address);

    //    if (!m_HatAssetReference.RuntimeKeyIsValid())
    //        return;

    //    m_HatLoadOpHandle = m_HatAssetReference.LoadAssetAsync<GameObject>();

    //    m_HatLoadOpHandle.Completed += OnHatLoadComplete;

    //}

    private void OnHatLoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        Debug.Log($"Async Operation Handle Status; "+ handle.Status);

        //if (handle.Status == AsyncOperationStatus.Succeeded)
        //    Instantiate(handle.Result, m_HatAnchor);

        m_HatInstance = Instantiate(handle.Result, m_HatAnchor);


    }

    private void OnHatLoaded(AsyncOperation asyncOperation)
    {
        Instantiate(m_HatLoadingRequest.asset as GameObject, m_HatAnchor, false);
    }

    private void OnDisable()
    {
        //if (m_HatLoadingRequest != null)
        //    m_HatLoadingRequest.completed -= OnHatLoaded;

        m_HatLoadOpHandle.Completed -= OnHatLoadComplete;

    }
}

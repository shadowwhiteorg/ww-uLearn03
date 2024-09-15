using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Reflection;

// Used for the Hat selection logic
public class PlayerConfigurator : MonoBehaviour
{

    [SerializeField]
    private Transform m_HatAnchor;


    // following variable may cause future priblems like typos so we will replace it with AssetReference
    //[SerializeField]
    //private string m_Address;

    // AssetReference struct will change to prevent the selection of wrong type of assets
    //[SerializeField]
    //private AssetReference m_HatAssetReference;

    //[SerializeField] 
    //AssetReferenceGameObject m_HatAssetReference;

    private ResourceRequest m_HatLoadingRequest;

    private GameObject m_HatInstance;

    //private List<string> m_Keys = new List<string>() { "Hats", "Seasonal" };
    private List<string> m_Keys = new List<string>() { "Hats", "Fancy" };

    //private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;
    // we will use async operation handle as a list.
    //private AsyncOperationHandle<IList<GameObject>> m_HatsLoadOpHandle;

    private AsyncOperationHandle<IList<IResourceLocation>> m_HatsLocationsOpHandle;
    private AsyncOperationHandle<GameObject> m_HatLoadOpHandle;



    void Start()
    {
        //SetHat(string.Format("Hat{0:00}", GameManager.s_ActiveHat));
        //LoadInRandomHat();

        //m_HatsLoadOpHandle = Addressables.LoadAssetsAsync<GameObject>(m_Keys, null, Addressables.MergeMode.Intersection);
        m_HatsLocationsOpHandle = Addressables.LoadResourceLocationsAsync(m_Keys, Addressables.MergeMode.Intersection);
        m_HatsLocationsOpHandle.Completed += OnHatLocationsLoadComplete;
        LoadInRandomHat(m_HatsLocationsOpHandle.Result);

    }

    private void OnHatLocationsLoadComplete(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        IList<IResourceLocation> results = handle.Result;
        for (int i = 0; i < results.Count; i++)
        {
            Debug.Log("Hat: " + results[i].PrimaryKey);
        }
    }


    private void Update()
    {
        //if (Input.GetMouseButtonUp(1))
        //{
        //    Destroy(m_HatInstance);
        //    Addressables.ReleaseInstance(m_HatLoadOpHandle);

        //    LoadInRandomHat();
        //}
        if (Input.GetMouseButtonUp(1))
        {
            Destroy(m_HatInstance);

            //LoadInRandomHat(m_HatsLoadOpHandle.Result);

            Addressables.Release(m_HatLoadOpHandle);

            LoadInRandomHat(m_HatsLocationsOpHandle.Result);

        }
    }


    private void LoadInRandomHat(IList<IResourceLocation> resourceLocations)
    {
        int randomIndex = Random.Range(0, resourceLocations.Count);
        IResourceLocation randomHatPrefab = resourceLocations[randomIndex];

        m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(randomHatPrefab);
        m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    }


    private void OnHatLoadComplete(AsyncOperationHandle<GameObject> handle)
    {
        Debug.Log($"Async Operation Handle Status: " +  handle.Status);

        //if (handle.Status == AsyncOperationStatus.Succeeded)
        //    Instantiate(handle.Result, m_HatAnchor);

        //m_HatInstance = Instantiate(handle.Result, m_HatAnchor);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            m_HatInstance = Instantiate(handle.Result, m_HatAnchor);
        }

    }

    private void OnHatLoaded(AsyncOperation asyncOperation)
    {
        Instantiate(m_HatLoadingRequest.asset as GameObject, m_HatAnchor, false);
    }

    private void OnDisable()
    {
        //if (m_HatLoadingRequest != null)
        //    m_HatLoadingRequest.completed -= OnHatLoaded;

        //m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
        //m_HatsLoadOpHandle.Completed -= OnHatsLoadComplete;

        m_HatLoadOpHandle.Completed -= OnHatLoadComplete;
        m_HatsLocationsOpHandle.Completed -= OnHatLocationsLoadComplete;


    }


    //private void OnHatsLoadComplete(AsyncOperationHandle<IList<GameObject>> handle)
    //{
    //    Debug.Log("Async Operation Handle Status: " + m_HatsLoadOpHandle.Status);

    //    if(m_HatsLoadOpHandle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        IList<GameObject> results = handle.Result;
    //        for (int i = 0; i < results.Count; i++)
    //        {
    //            Debug.Log("Hat: " + results[i].name);


    //        }
    //        LoadInRandomHat(results);
    //    }
    //}

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

    //private void LoadInRandomHat()
    //{
    //    int randomIndex = Random.Range(0, 6);
    //    string hatAddress = string.Format("Hat{0:00}", randomIndex);

    //    m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(hatAddress);
    //    m_HatLoadOpHandle.Completed += OnHatLoadComplete;
    //}

    //private void LoadInRandomHat(IList<GameObject> prefabs)
    //{
    //    int randomIndex = Random.Range(0, prefabs.Count);
    //    GameObject randomHatPrefab = prefabs[randomIndex];
    //    m_HatInstance = Instantiate(randomHatPrefab, m_HatAnchor);
    //}
}

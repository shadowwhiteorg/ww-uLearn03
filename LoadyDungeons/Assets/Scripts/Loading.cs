using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    //private AsyncOperation m_SceneOperation;

    private static AsyncOperationHandle<SceneInstance> m_SceneLoadingOpHandle;

    [SerializeField]
    private Slider m_LoadingSlider;

    [SerializeField]
    private GameObject m_PlayButton, m_LoadingText;

    private void Awake()
    {
        StartCoroutine(loadNextLevel("Level_0" + GameManager.s_CurrentLevel));
    }

    private IEnumerator loadNextLevel(string level)
    {
        //m_SceneOperation = SceneManager.LoadSceneAsync(level);
        //m_SceneOperation.allowSceneActivation = false;
        m_SceneLoadingOpHandle = Addressables.LoadSceneAsync(level, activateOnLoad: true);

        while (!m_SceneLoadingOpHandle.IsDone)
        {
            m_LoadingSlider.value = m_SceneLoadingOpHandle.PercentComplete;

            if (m_SceneLoadingOpHandle.PercentComplete >= 0.9f && !m_PlayButton.activeInHierarchy)
                m_PlayButton.SetActive(true);

            yield return null;
        }

        Debug.Log($"Loaded Level {level}");
    }

    // Function to handle which level is loaded next
    //public void GoToNextLevel()
    //{
    //    m_SceneOperation.allowSceneActivation = true;
    //}
}

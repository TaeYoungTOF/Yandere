using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    TitleScene,
    GameScene,
}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private string[] dialogues;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator LoadAsync(SceneName sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOp.allowSceneActivation = false;

        switch (sceneName)
        {
            case SceneName.GameScene:
                loadingSlider.value = 0;
                progressText.text = "0%";
                dialogueText.text = dialogues[Random.Range(0, dialogues.Length)];
        
                loadingPanel.SetActive(true);
                break;
            default:
                loadingPanel.SetActive(false);
                Debug.Log("[SceneLoader] Loading...");
                break;
        }

        while (!asyncOp.isDone)
        {
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);

            if (sceneName == SceneName.GameScene)
            {
                UpdateProgress(progress);
            }

            if (asyncOp.progress >= 0.9f)
            {
                if (sceneName == SceneName.GameScene)
                {
                    SetComplete();
                    yield return new WaitForSeconds(0.3f);
                }
                
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
        
        Debug.Log("[SceneLoader] Load Complete");
        loadingPanel.SetActive(false);
    }

    private void UpdateProgress(float progress)
    {
        loadingPanel.SetActive(true);
        
        loadingSlider.value = progress;
        progressText.text = $"{(int)(progress * 100)}%";
    }

    private void SetComplete()
    {
        progressText.text = "Start!";
    }
}

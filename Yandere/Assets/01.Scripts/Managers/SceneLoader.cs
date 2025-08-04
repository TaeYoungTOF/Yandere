using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
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
    [SerializeField] private Transform loadingCircle;
    [SerializeField] private float rotateDuration = 2f;

    [SerializeField] private string[] dialogues;

    private bool _isLoading;

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

    /*public async Task LoadAsync(SceneName sceneName)
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
                    await Task.Delay(300);
                }
                
                asyncOp.allowSceneActivation = true;
            }

            await Task.Yield();
        }
        
        Debug.Log("[SceneLoader] Load Complete");
        loadingPanel.SetActive(false);
    }*/
    
    public async Task LoadAsync(SceneName sceneName)
    {
        if (_isLoading) return;

        _isLoading = true;

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOp.allowSceneActivation = false;

        switch (sceneName)
        {
            case SceneName.GameScene:
                RotateCircle();
                loadingSlider.value = 0;
                progressText.text = "LOADING... 0%";
                dialogueText.text = dialogues[Random.Range(0, dialogues.Length)];
        
                loadingPanel.SetActive(true);
                break;
            default:
                loadingPanel.SetActive(false);
                Debug.Log("[SceneLoader] Loading...");
                break;
        }

        while (asyncOp.progress < 0.9f)
        {
            if (sceneName == SceneName.GameScene)
            {
                float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
                UpdateProgress(progress);
            }

            await Task.Yield();
        }

        if (sceneName == SceneName.GameScene)
        {
            UpdateProgress(1f);
            SetComplete();
            await Task.Delay(3000);
        }

        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
        {
            await Task.Yield();
        }
        
        loadingPanel.SetActive(false);
        _isLoading = false;
    }

    private void RotateCircle()
    {
        if (!loadingCircle) return;
        
        loadingCircle.DORotate(new Vector3(0, 0, -360), rotateDuration, RotateMode.FastBeyond360)
                     .SetLoops(-1, LoopType.Restart)
                     .SetEase(Ease.Linear);
    }

    private void UpdateProgress(float progress)
    {
        loadingPanel.SetActive(true);
        
        loadingSlider.value = progress;
        progressText.text = $"LOADING... {(int)(progress * 100)}%";
    }

    private void SetComplete()
    {
        progressText.text = "Start!";
    }
}

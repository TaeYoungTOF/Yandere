using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text progressText;

    [SerializeField] private string[] dialogues;

    private void Start()
    {
        loadingSlider.value = 0;
        progressText.text = "0%";
        dialogueText.text = dialogues[Random.Range(0, dialogues.Length)];
        
        _loadingPanel.SetActive(false);
    }

    public void UpdateProgress(float progress)
    {
        _loadingPanel.SetActive(true);
        
        loadingSlider.value = progress;
        progressText.text = $"{(int)(progress * 100)}%";
    }

    public void SetComplete()
    {
        progressText.text = "Start!";
    }
}

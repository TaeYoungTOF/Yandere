using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BossStage : MonoBehaviour
{
    [SerializeField] private GameObject _bossStageUI;
    [SerializeField] private TextMeshProUGUI _bossStageText;
    [SerializeField] private Image _bossStageGrow;
    [SerializeField] private Animator _bossWarning;
    [SerializeField] private Animator _bossGrow;
    
    private void Start()
    {
        _bossStageUI.SetActive(false);
    }
}

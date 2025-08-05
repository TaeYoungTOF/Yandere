using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UI_BossStage : MonoBehaviour
{
    [SerializeField] private GameObject _bossStageUI;
    [SerializeField] private TMP_Text _bossNameText;

    [SerializeField] private Transform _warningText;
    
    [SerializeField] private Animator _bossGlow;

    private Tween _tween;
    
    private void Start()
    {
        _bossStageUI.SetActive(false);
    }
    
    public void CallBossWarning(string bossName)
    {
        _bossNameText.text = bossName;
        UIAnimation();
        
        _bossStageUI.SetActive(true);

        StartCoroutine(CloseUI());
    }
    
    private void UIAnimation()
    {
        float duration = 5f; // 이동 시간 (초)
        Vector3 startX = _warningText.localPosition;
        float endX = -1123; // 원하는 값으로 조정
        
            // 무한 루프 마퀴 애니메이션
        _tween = _warningText.DOLocalMoveX(endX, duration)
                             .SetEase(Ease.Linear)
                             .SetLoops(-1, LoopType.Restart)
                             .OnComplete(() =>
                             { 
                                 _warningText.localPosition = startX;
                             });
        
    }

    private IEnumerator CloseUI()
    {
        yield return new WaitForSeconds(100f);
        _bossStageUI.SetActive(false);
        _tween.Kill();

        yield return null;
    }
}

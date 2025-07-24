using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_BossController : MonoBehaviour
{
    private IBossPattern currentPattern;
    private List<IBossPattern> patternPool;
    private IBossPattern lastUsedPattern;
    
    private float idleTime = 3f;
    private bool isInIdle = false;
    
    void Start()
    {
        patternPool = new List<IBossPattern>(GetComponents<IBossPattern>());
        StartCoroutine(NextPatternRoutine());
    }

    private IEnumerator NextPatternRoutine()
    {
        while (true)
        {
            isInIdle = true;
            PlayIdleAnim();
            yield return new WaitForSeconds(idleTime);
            
            isInIdle = false;
            currentPattern = SelectNextPattern();
            currentPattern?.Execute();
            
            // 패턴이 스스로 끝나면 NextPatternRoutine() 다시 호출되도록 설정
            yield return new WaitUntil(() => currentPattern != null && currentPattern.IsDone);
        }
    }
    
    private IBossPattern SelectNextPattern()
    {
        var candidates = patternPool.FindAll(p => p.CanExecute());

        Debug.Log($"[SelectNextPattern] 후보 패턴 수: {candidates.Count}");

        if (candidates.Count == 0)
        {
            Debug.LogWarning("[SelectNextPattern] 실행 가능한 패턴이 없습니다.");
            return null;
        }

        var selected = candidates[Random.Range(0, candidates.Count)];
        Debug.Log($"[SelectNextPattern] 선택된 패턴: {selected.GetType().Name}");
        return selected;
    }
    private void PlayIdleAnim()
    {
        // GUN_IDLE 애니메이션 재생
    }

    public void Die()
    {
        StopAllCoroutines();
    }
}

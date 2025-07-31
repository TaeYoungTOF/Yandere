using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern4_Projectile : MonoBehaviour
{

    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private float effectRadius = 3f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float slowAmount = 0.3f;
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private float smokelifeTime = 5f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveTime;
    
    private bool hasExploded = false;
    private bool isRotating = false;

    public void Init(Vector3 target, float height, float duration)
    {
        startPos = transform.position;
        targetPos = target;
        moveTime = duration;
        StartCoroutine(MoveArc(height));
    }

    private IEnumerator MoveArc(float height)
    {
        float timer = 0f;

        while (timer < moveTime)
        {
            float t = timer / moveTime;
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);
            pos.y += height * 4 * (t - t * t); // 포물선 곡선

            transform.position = pos;
            timer += Time.deltaTime;
            yield return null;
        }

        OnImpact();
    }

    private void OnImpact()
    {
        if (hasExploded) return;
        hasExploded = true;
        
        // 2. 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.Play("InGame_EnemyBoss4Pattern1_SmokeSFX");
            Destroy(effect, 3f); // 파티클 길이에 따라 조정
        }

        // 3. 지속 피해 코루틴 시작
        StartCoroutine(Explode());
    }

   
    private IEnumerator Explode()
    {
        // 1. 폭발 이펙트
        SpawnExplosionEffect();

        // 2. 폭탄 자체 회전 시작
        isRotating = true;

        // 3. 사운드
        //SoundManager.Instance.Play("");

        // 4. 데미지 루프
        float timer = 0f;
        while (timer < duration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius, LayerMask.GetMask("Player"));
            foreach (var hit in hits)
            {
                var player = hit.GetComponent<Player>();
                if (player != null)
                {
                    StageManager.Instance.Player.stat.GetBonusMoveSpeed(-slowAmount);
                    // player.ApplySmokeBlind();
                }
            }

            timer += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (isRotating)
        {
            // 회전 속도 조절
            transform.Rotate(Vector3.forward * -720f * Time.deltaTime);
        }
    }
    private void SpawnExplosionEffect()
    {
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, smokelifeTime); // 이펙트 파괴 시간 설정
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}

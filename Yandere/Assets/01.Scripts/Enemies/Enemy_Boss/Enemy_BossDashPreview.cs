using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossDashPreview : MonoBehaviour
{
    private LineRenderer line;

    public Transform boss;
    public Vector2 direction;
    public float length = 5f;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    public void ShowDashDirection()
    {
        Vector3 startPos = boss.position;
        Vector3 endPos = startPos + (StageManager.Instance.Player.transform.position - startPos).normalized * 5f;

        GameObject lineObj = new GameObject("DashPreviewLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);

        lr.startWidth = 0.15f;
        lr.endWidth = 0.15f;
        lr.material = new Material(Shader.Find("Sprites/Default")); // 기본 머티리얼 사용
        lr.startColor = new Color(1f, 0f, 0f, 0.4f);  // 불투명도 조절됨 (0.4는 반투명)
        lr.endColor = new Color(1f, 0f, 0f, 0.4f);

        // 👉 몇 초 후 자동 제거
        Destroy(lineObj, 1f); // 1초 뒤 사라짐
    }

    public void Hide()
    {
        line.enabled = false;
    }
}

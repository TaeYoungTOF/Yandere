using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public RectTransform rectTransform;
    private Rect safeArea;
    private Vector2 minAnchor;
    private Vector2 maxAnchor;

    private void Awake()
    {
        //safeArea를 받아서 min 앵커와 max 앵커에 Position 부여
        //픽셀로 반환되니 앵커에 넣기 위해서는 비율로 변환 필요
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        //인스펙터 프로퍼티에 집어넣을 수 있게 비율로 변환 및 할당
        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
   
}
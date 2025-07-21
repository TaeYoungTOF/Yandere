using UnityEngine;
using UnityEngine.UI;

// 각 업적 UI 항목을 구성하는 요소들을 담는 클래스

[System.Serializable]
public class AchievementUIItem
{
    public TMPro.TextMeshProUGUI descriptionText; // 업적 설명 텍스트
    public TMPro.TextMeshProUGUI progressText;    // 진행률 텍스트 (예: "5/10")
    public Slider progressBar;                    // 진행률 슬라이더 바
    public GameObject starIcon;                   // 완료 시 활성화될 별 아이콘
}




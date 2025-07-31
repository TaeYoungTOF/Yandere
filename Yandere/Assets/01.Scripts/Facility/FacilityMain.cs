using TMPro;
using UnityEngine;

public class FacilityMain : Facility
{
   [Header("UI 표시")]
   [SerializeField] private TextMeshProUGUI stackCountText;                                            // UI쪽에서 현재 카운트 스택 텍스트표시
   [SerializeField] private TextMeshProUGUI timerText;                                                 // UI쪽에서 현재 남은 시간 텍스트표시

   [SerializeField] private GameObject[] _photos = new GameObject[12];
   
   protected override void Init()
   {
      base.Init();
      
      currentLevel++;
      
      for (int i = 0; i < _photos.Length; i++)
      {
         _photos[i].SetActive(false);
      }
   }

   public void SetPhoto()
   {
      while (true)
      {
         int randomIndex = Random.Range(0, _photos.Length);
         
         if (!_photos[randomIndex].activeSelf)
         {
            _photos[randomIndex].SetActive(true);
            break;
         }
      }
      UpdateUI();
   }
   
   public void OnClickPhoto()
   {
      SoundManager.Instance.Play("LobbyClick01_SFX");
      ResourceManager.Instance.UseStack(amount);
      UpdateUI();
   }

   public void UpdateTimeUI(string formatted)
   {
      if (timerText != null)
         timerText.text = formatted;
   }

   public void UpdateStackUI(int currentStack, int maxStack)
   {
      if (stackCountText != null)
         stackCountText.text = $"{currentStack.ToString()} / {maxStack.ToString()}";
   }
}

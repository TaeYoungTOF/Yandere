using UnityEngine;

public class UIManager_Title : MonoBehaviour
{
    public static UIManager_Title Instance { get; private set; }
    
    public UI_MainHUD mainHUD;
    public UI_Popup popUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        popUp.Init();
    }

    public void UpdateUI()
    {
        mainHUD.UpdateUI();
    }
}

using UnityEngine; 


public class UI_LevelUp : MonoBehaviour
{
    [SerializeField] private ParticleSystem levleUpParticle;
    
    public void ShowLevelUpParticle()
    {
        levleUpParticle.Play();
    }
    
}

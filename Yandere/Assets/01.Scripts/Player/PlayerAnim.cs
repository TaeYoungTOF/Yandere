using UnityEngine;
public enum targetDirectType
{
    forward = 0,
    backward = 1,
    right = 2,
    left = 3,
}

public enum AniType
{
    Idle = 0,
    Move = 1,
    //Attack1 = 2,
    //Attack2 = 3,
    //Hurt = 4,
}

public class PlayerAnim : MonoBehaviour
{
    public targetDirectType targetType;
    public Animator[] targetAnimators; //0- forward 1- backward

    public void SetDirection(targetDirectType dir)
    {
        targetType = dir;

        for (int i = 0; i < targetAnimators.Length; i++)
        {
            targetAnimators[i].gameObject.SetActive(i == (int)dir);
        }
    }
    
    public void SetAni(AniType aType)
    {
        Animator currentAnimator = targetAnimators[(int)targetType];

        // 모든 트리거 초기화 (옵션)
        foreach (AniType type in System.Enum.GetValues(typeof(AniType)))
        {
            currentAnimator.ResetTrigger(type.ToString());
        }

        currentAnimator.SetTrigger(aType.ToString());
    }
}

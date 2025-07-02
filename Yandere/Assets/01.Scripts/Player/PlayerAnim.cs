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
    idle = 0,
    move = 1,
    attack1 = 2,
    attack2 = 3 ,
    hurt = 4,
}

public class PlayerAnim : MonoBehaviour
{
    public targetDirectType targetType;
    public Animator[] targetAnimators; //0- forward 1- backward

    public void SetAni(AniType aType)
    {
        //change tatget direction

        if (!targetAnimators[(int)targetType].isActiveAndEnabled)
        {
            targetAnimators[(int)targetType].gameObject.SetActive(true);
            if (targetType == targetDirectType.forward)
                targetAnimators[(int)targetDirectType.backward].gameObject.SetActive(false);
            else
                targetAnimators[(int)targetDirectType.forward].gameObject.SetActive(false);
        }
        targetAnimators[(int)targetType].SetInteger("aniInt", (int)aType);
    }

}

using UnityEngine;

public class PlayRunSoundOnEnter : StateMachineBehaviour
{
    private PlayerMovement playerMovement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerMovement == null)
        {
            playerMovement = animator.GetComponent<PlayerMovement>();
        }

        if (playerMovement != null)
        {
            playerMovement.PlayMoveSound();
        }
    }
}

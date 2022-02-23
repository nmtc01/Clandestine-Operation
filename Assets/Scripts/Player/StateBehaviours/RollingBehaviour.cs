using UnityEngine;

public class RollingBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => Player.GetInstanceControl().SetIsRolling(false);
}
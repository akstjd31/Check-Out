using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]

public class SoundDistance : MonoBehaviour
{
    [SerializeField] private GameObject soundCollider;
    [SerializeField] private float distance;
    private PlayerStateMachine state;
    private SphereCollider distanceCollider;
    

    private void Awake()
    {
        state = this.GetComponent<PlayerStateMachine>();
        distanceCollider = soundCollider.GetComponent<SphereCollider>();

        distanceCollider.radius = 0.5f * distance;
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.TryGetComponent<SirenController>(out var sirenController)
        //    && other.TryGetComponent<SirenModel>(out var sirenModel)
        //    && (state.CurrentState == PlayerState.Run || state.CurrentState == PlayerState.Walk))
        //{
        //    if (sirenModel.monsterState == Monster.MonsterState.WanderingAround)
        //        sirenController.StateChange(Monster.MonsterState.Alert);
        //}

        if (other.TryGetComponent<SirenController>(out var sirenController)
            && other.TryGetComponent<SirenModel>(out var sirenModel)
            )
        {
            Debug.Log($" 현재 사이렌 상태 : {sirenModel.monsterState.ToString()}");
            if (sirenModel.monsterState == Monster.MonsterState.WanderingAround)
                sirenController.StateChange(Monster.MonsterState.Alert);
        }
    }
}

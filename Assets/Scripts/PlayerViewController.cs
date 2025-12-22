using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerViewController : MonoBehaviour
{
    FieldOfView playerFieldOfView;

    private HashSet<MannequinModel> observedMannequins = new HashSet<MannequinModel>();
    private HashSet<EchoModel> observedEchos = new HashSet<EchoModel>();

    private HashSet<MannequinModel> currentObservedMannequin = new HashSet<MannequinModel>();
    private HashSet<EchoModel> currentObservedEcho = new HashSet<EchoModel>();

    private void Awake()
    {
        playerFieldOfView = GetComponent<FieldOfView>();
    }

    private void Start()
    {
        if (playerFieldOfView == null)
        {
            Debug.Log("null");
            return;
        }
        StartCoroutine(playerFieldOfView.FindTargetsWithDelay());
    }

    private void Update()
    {
        currentObservedMannequin.Clear();
        currentObservedEcho.Clear();

        foreach (var target in playerFieldOfView.visibleTargets)
        {
            // 마네킹인지 체크
            if (target.TryGetComponent<MannequinModel>(out var mannequinModel))
            {
                mannequinModel.isObservedFromPlayer = true;

                currentObservedMannequin.Add(mannequinModel);

                if (mannequinModel.monsterState != Monster.MonsterState.Stop && target.TryGetComponent<MannequinController>(out var MannequinController))
                {
                    MannequinController.GetTransform(transform);
                    mannequinModel.ChangeState(Monster.MonsterState.Stop);
                }
            }

            // 에코인지 체크
            else if (target.TryGetComponent<EchoModel>(out var echoModel))
            {
                echoModel.isObservedFromPlayer = true;

                currentObservedEcho.Add(echoModel);
            }
        }

        foreach (var mannequin in observedMannequins)
        {
            if (!currentObservedMannequin.Contains(mannequin))
            {
                mannequin.isObservedFromPlayer = false;
            }
        }

        observedMannequins.Clear();
        observedMannequins.UnionWith(currentObservedMannequin);

        foreach (var echo in observedEchos)
        {
            if (!currentObservedEcho.Contains(echo))
            {
                echo.isObservedFromPlayer = false;
            }
        }

        observedEchos.Clear();
        observedEchos.UnionWith(currentObservedEcho);
    }
}

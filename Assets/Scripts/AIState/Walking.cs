using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walking : BaseState {
    private MovementSM _sm;

    public Walking(MovementSM stateMachine) : base("Walking", stateMachine) {
        _sm = stateMachine;
    }

    public override void Enter() {
        base.Enter();
        _sm.SetCurrentTarget();
        _sm.agent.isStopped = false;
        if (!_sm.isAngry)
            _sm.modelRenderer.material.color = Color.green;
        else
            _sm.modelRenderer.material.color = Color.red;
    }

    public override void UpdateLogic() {
        base.UpdateLogic();
        if (IsAtDestination())
            _sm.ChangeState(_sm.inspectingState);
    }

    private bool IsAtDestination() {
        return _sm.agent.remainingDistance < _sm.distanceToDestinationThreshhold && _sm.agent.remainingDistance != 0;
    }
}

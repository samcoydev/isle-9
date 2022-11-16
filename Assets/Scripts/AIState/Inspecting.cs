using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inspecting : BaseState {

    private MovementSM _sm;
    private IInspectable _inspectable;

    public Inspecting(MovementSM stateMachine) : base("Inspecting", stateMachine) {
        _sm = stateMachine;
    }

    public override void Enter() {
        base.Enter();

        _sm.agent.isStopped = true;
        if (!_sm.isAngry)
            _sm.modelRenderer.material.color = Color.yellow;
        else
            _sm.modelRenderer.material.color = Color.red;

        if (_sm.currentTarget.TryGetComponent<IInspectable>(out var inspectable)) {
            _inspectable = inspectable;
            _inspectable.Inspect(this._sm);
        }
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        if (_inspectable == null) Debug.Log("Nothing to inspect.");

        if (!_sm.isInspecting) {
            _sm.DoneInspecting();
            _sm.ChangeState(_sm.walkingState);
        }
    }
}

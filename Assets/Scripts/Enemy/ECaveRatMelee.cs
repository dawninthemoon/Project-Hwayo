using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class ECaveRatMelee : EnemyBase {
    enum States { Patrol, Chase, ChaseWait, AttackReady, TackleStraight, TackleParabola, TackleStraightWait, TackleParabolaWait, Hit, Dead }
    StateMachine<States> _fsm;
    GroundMovementControl _movementControl;
    Vector2 _direction;

    public override void Initalize() {
        base.Initalize();
        InitalizeAnimator("CAVERAT_", "Patrol");
        
        var controller = GetComponent<GroundController>();
        controller.Initalize();

        var movementSetting = GetComponentInChildren<TBLGroundMovement>();
        _movementControl = new GroundMovementControl(controller, movementSetting);
        _movementControl.Initalize();

        _fsm = GetComponent<StateMachineRunner>().Initialize<States>(this);
        _fsm.ChangeState(States.Patrol);
    }
    public override void Progress() {
        _movementControl.CalculateVelocity(_direction);
		_movementControl.MoveObject(_direction);

        _movementControl.PostCalculateVelocity();

        base.Progress();
    }
    protected override void OnChaseDetected() {

    }
    protected override void OnAttackDetected() {

    }
    #region FSM
    void Patrol_Enter() {
        _animator.ChangeAnimation("Patrol", true);
    }
    #endregion
}

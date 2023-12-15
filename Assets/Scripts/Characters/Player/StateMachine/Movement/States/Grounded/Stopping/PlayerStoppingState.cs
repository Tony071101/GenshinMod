using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = 0f;

        SetBaseCameraRecenteringData();

        base.Enter();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        RotateTowardTargetRotation();

        if(!IsMovingHorizontally()){
            return;
        }

        DecelerateHorizontally();
    }

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.Player._playerInput.PlayerActions.Movement.started += OnMovementStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.Player._playerInput.PlayerActions.Movement.started -= OnMovementStarted;
    }
    #endregion

    #region Input Methods
    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
    #endregion
}

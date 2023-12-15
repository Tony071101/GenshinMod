using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundedState
{
    private PlayerDashData dashData;
    private float startTime;
    private int consecutiveDashesUsed;
    private bool shouldKeepRotating;
    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        dashData = movementData.DashData;
    }

    #region IState Methods
    public override void Enter()
    {
        stateMachine.ReusableData.MovementSpeedModifier = dashData.SpeedModifier;

        base.Enter();
        
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

        stateMachine.ReusableData.RotationData = dashData.RotationData;

        Dash();

        shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

        UpdateConsecutiveDashes();

        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

        SetBaseRotationData();
    }

    public override void OnAnimationTransitionEvent()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero){
            stateMachine.ChangeState(stateMachine.HardStoppingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.SprintingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(!shouldKeepRotating){
            return;
        }

        RotateTowardTargetRotation();
    }
    #endregion

    #region Main Methods
    private void Dash()
    {
        Vector3 dashDirection = stateMachine.Player.transform.forward;

        dashDirection.y = 0f;

        UpdateTargetRotation(dashDirection, false);

        if(stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            UpdateTargetRotation(GetMovementInputDirection());

            dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
        }

        stateMachine.Player._rigidbody.velocity = dashDirection * GetMovementSpeed(false);
    }

    private void UpdateConsecutiveDashes()
    {
        if(!IsConsecutive()){
            consecutiveDashesUsed = 0;
        }

        ++consecutiveDashesUsed;

        if(consecutiveDashesUsed == dashData.ConsecutiveDashesLimitAmount){
            consecutiveDashesUsed = 0;

            stateMachine.Player._playerInput.DisableActionFor(stateMachine.Player._playerInput.PlayerActions.Dash, dashData.DashLimitReachedCooldown);
        }
    }

    private bool IsConsecutive()
    {
        return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.Player._playerInput.PlayerActions.Movement.performed += OnMovementPerformed;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.Player._playerInput.PlayerActions.Movement.performed -= OnMovementPerformed;
    }
    #endregion

    #region Input Methods
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        shouldKeepRotating = true;
    }
    
    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }
    #endregion
}

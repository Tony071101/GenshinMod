using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData slopeData;
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        slopeData = stateMachine.Player.ColliderUtility.SlopeData;
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        UpdateShouldSprintState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Float();
    }
    #endregion

    #region Main Methods
    private void UpdateShouldSprintState()
    {
        if(!stateMachine.ReusableData.ShouldSprint){
            return;
        }

        if(stateMachine.ReusableData.MovementInput != Vector2.zero){
            return;
        }

        stateMachine.ReusableData.ShouldSprint = false;
    }
    
    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCollider = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if(Physics.Raycast(downwardsRayFromCapsuleCollider, out RaycastHit hit, slopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore)){
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCollider.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if(slopeSpeedModifier == 0f){
                return;
            }
            
            float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;

            if(distanceToFloatingPoint == 0f){
                return;
            }

            float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            stateMachine.Player._rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

        stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

        return slopeSpeedModifier;
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.Player._playerInput.PlayerActions.Movement.canceled += OnMovementCanceled;

        stateMachine.Player._playerInput.PlayerActions.Dash.started += OnDashStarted;

        stateMachine.Player._playerInput.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.Player._playerInput.PlayerActions.Movement.canceled -= OnMovementCanceled;

        stateMachine.Player._playerInput.PlayerActions.Dash.started -= OnDashStarted;

        stateMachine.Player._playerInput.PlayerActions.Jump.started -= OnJumpStarted;
    }

    protected virtual void OnMove()
    {
        if(stateMachine.ReusableData.ShouldSprint){
            stateMachine.ChangeState(stateMachine.SprintingState);

            return;
        }

        if(stateMachine.ReusableData.ShouldWalk){
            stateMachine.ChangeState(stateMachine.WalkingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }
    #endregion

    #region Input Methods
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.DashingState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpingState);
    }
    #endregion
}

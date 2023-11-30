using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public Rigidbody _rigidbody { get; private set; }
    public PlayerInput _playerInput { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        movementStateMachine = new PlayerMovementStateMachine();
    }

    private void Start() {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void Update() {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();
    }

    private void FixedUpdate() {
        movementStateMachine.PhysicsUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    public Rigidbody _rigidbody { get; private set; }
    public PlayerInput _playerInput { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        MainCameraTransform = Camera.main.transform;
        movementStateMachine = new PlayerMovementStateMachine(this);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    
    [field: Header("Collisions")]
    [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

    [field: Header("Cameras")]
    [field: SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Rigidbody _rigidbody { get; private set; }
    public Animator _animator { get; private set; }
    public PlayerInput _playerInput { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
        CameraUtility.Initialize();
        AnimationData.Initialize();
        MainCameraTransform = Camera.main.transform;
        movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void OnValidate() {
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }

    private void Start() {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void OnTriggerEnter(Collider collider) {
        movementStateMachine.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider) {
        movementStateMachine.OnTriggerExit(collider);
    }

    private void Update() {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();
    }

    private void FixedUpdate() {
        movementStateMachine.PhysicsUpdate();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
}

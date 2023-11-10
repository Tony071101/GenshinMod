using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject weapon;
    private Animator anim;
    private PlayerInput _playerInput;
    private StarterAssetsInputs _input;
    private GameObject currentWeaponInHand;

    private void Start() {
        anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update() {
        HandleAttack();
    }

    public void InstantiateWeapon(){
        if(currentWeaponInHand != null){
            return;
        }
        currentWeaponInHand = Instantiate(weaponHolder, weaponHolder.transform);
    }

    public void DestroyWeapon(){
        Destroy(currentWeaponInHand);
    }

    public void HandleAttack(){
        if(_input.attack == true){
            anim.SetTrigger("Attack");
        }
        _input.attack = false;
        if(_input.attack == false){
            anim.SetTrigger("finishAttack");
        }
    }
}

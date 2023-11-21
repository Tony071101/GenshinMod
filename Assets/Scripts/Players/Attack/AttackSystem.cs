using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject sheathHolder;
    [SerializeField] private GameObject weapon;
    private Animator anim;
    private PlayerInput _playerInput;
    private StarterAssetsInputs _input;
    private ThirdPersonController thirdPersonController;
    private GameObject currentWeaponInHand;
    private GameObject currentWeaponSheath;
    private float destroyTimer = 10f;
    private void Start() {
        anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update() {
        HandleAttack();
    }

    public void InstantiateWeapon(){
        if(currentWeaponInHand != null){
            return;
        }
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWeaponSheath);
    }

    public void SheathWeapon(){
        if(currentWeaponInHand == null){
            return;
        }
        currentWeaponSheath = Instantiate(currentWeaponInHand, sheathHolder.transform);
        Destroy(currentWeaponInHand);
    }

    public void DestroyWeapon(){
        if(currentWeaponSheath == null){
            return;
        }
        Destroy(currentWeaponSheath, destroyTimer);
    }

    private void HandleAttack(){
        if(_input.attack){
            anim.SetTrigger("Attack");

            _input.move = Vector2.zero;
            _input.jump = false;
        }
        _input.attack = false;
    }

    public void StartDealDamage(){
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage(){
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}

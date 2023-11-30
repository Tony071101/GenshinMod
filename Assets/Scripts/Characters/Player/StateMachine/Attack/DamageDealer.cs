using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private bool canDealDamage;
    private List<GameObject> hasDealDamage;
    [SerializeField] private float weaponLength;
    [SerializeField] private float weaponDamage;

    private void Start() {
        canDealDamage = false;
        hasDealDamage = new List<GameObject>();
    }

    private void Update() {
        DealDamage();
    }

    private void DealDamage(){
        if(canDealDamage){
            RaycastHit hit;

            int layerMask = 1 << 9;
            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask)){
                if(!hasDealDamage.Contains(hit.transform.gameObject)){
                    print("Damage");
                    hasDealDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage(){
        canDealDamage = true;
        hasDealDamage.Clear();
    }

    public void EndDealDamage(){
        canDealDamage = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
        
    }

}

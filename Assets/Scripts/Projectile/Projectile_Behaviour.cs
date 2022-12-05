using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class Projectile_Behaviour : MonoBehaviour
    {
        
        private void Start()
        {
            
            StartCoroutine(projectileLifetime());
        }

        IEnumerator projectileLifetime() //LVA: explained in Projectile_Enem
        {
            yield return new WaitForSeconds(Plr_Manager.inst._slashLifetime - Plr_Manager.inst._slashLifetime * 0.5f);
            gameObject.GetComponent<Rigidbody>().drag = 0.66f;
            yield return new WaitForSeconds(Plr_Manager.inst._slashLifetime - Plr_Manager.inst._slashLifetime * 0.5f);
            Destroy(gameObject);
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag != "Player" && collision.tag != "Unhittable")
            {
                if (collision.tag == "Enemy")
                //LVA: If the player's projectile collides with an enemy, then run the following {}
                {
                    collision.GetComponent<Enemy_AI_2D>().TakeDamage(Plr_Manager.inst._damage);
                }
                else if (collision.tag != "Projectile")
                {
                    Destroy(collision);
                }
                Destroy(gameObject);
            }
        }
    }
}

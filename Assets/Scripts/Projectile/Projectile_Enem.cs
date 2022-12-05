using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class Projectile_Enem : MonoBehaviour
    {
        private void Start()
        {
            
            StartCoroutine(projectileLifetime());
            //LVA: When this script gets activated (which will be when the projectile is instantiated), run this IEnumarator
        }

        IEnumerator projectileLifetime()
        {
            yield return new WaitForSeconds(Plr_Manager.inst._slashLifetime - Plr_Manager.inst._slashLifetime * 0.5f);
            //LVA: after half the lifetime for the projectile has occured, do the next line of code.
            gameObject.GetComponent<Rigidbody>().drag = 0.66f;
            //LVA: the GameObject now slows down by 33%, simulating weight and making the projectile feel more "real"
            yield return new WaitForSeconds(Plr_Manager.inst._slashLifetime - Plr_Manager.inst._slashLifetime * 0.5f);
            //LVA: Once the last half of the projectiles lifetime has come to an end, do the next line of code
            Destroy(gameObject);
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag != "Projectile" && collision.tag != "Unhittable" && collision.tag != "Enemy")
            //LVA: If the projectile collides with a GameObject that is NOT tagged as one of the above, do the next line of code
            //LVA: I could have (&& collision.tag == "Player") with the above if statement as well, however
            //LVA: I want the projectile to be destroyed if it hits any object that is not the above. 
            {
                if (collision.tag == "Player")
                //LVA: if it IS colliding with a player, run what is in the {}
                {
                    Plr_Manager.inst.TakeDamage(1);
                    //LVA: using the Plr_Manager singleton, run the TakeDamage() function and send through the int 1. This will deal 1 damage to the player. 
                }
                Destroy(gameObject);
            }
        }
    }
}

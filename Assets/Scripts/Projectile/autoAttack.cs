using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class autoAttack : MonoBehaviour
    {

        [SerializeField]private GameObject spike;
        //LVA: spike is the Prefab used that gets generated when destroying an enemy
        public float duration = 1f;
        //LVA: Unused variable
        private void OnTriggerEnter(Collider collision)
        {
            
            if (collision.tag == "Enemy" && Plr_Manager.inst._centipedeOn)
            //LVA: if an enemy collides with this scripts attatched GameObject's collider AND centipede mode is on then run the following
            {
                collision.GetComponent<Enemy_AI_2D>().TakeDamage(Plr_Manager.inst._damage);
                //LVA: the script needs to access the Enemy's AI script to deal damage to it.
                //LVA: So this script grabs the collisions <Enemy_AI_2D>() and then runs the .TakeDamage() function in that script, while sending across the amount of damage to do.
                //LVA: The amount of damage will always kill.

                GameObject spiker = Instantiate(spike, collision.transform.position, Quaternion.identity);
                //LVA: this creates a new spike prefab in in the position of the collided enemy.
                spiker.transform.eulerAngles = new Vector3(-90,0,0);
                //LVA: the prefab is rotated incorrecly, so this line corrects the prefabs rotation by setting the X rotation to -90 degrees
                spiker.transform.localScale = new Vector3(0.2f, 0.2f, 0.45f);
                //LVA: To fine tune the size of the new prefab, the size is directly changed to be 80% less on the X and Y, and 55% less on the Z

            }
        }
    }
}

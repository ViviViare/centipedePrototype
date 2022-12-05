using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ViviViare
{
    public class Enemy_AI_2D : MonoBehaviour
    {
        public int _hp;   
        //LVA: HP value.
        public int _damage;   
        //LVA: damage value.
        [SerializeField]private float _speed;
        //LVA: Speed value which is used for the speed of the enemy projectile NOT the enemy movement speed
        [SerializeField]private float _chaseRange = 5f;   
        //LVA: Range from the Enemy that allows the enemy to start chasing the player
        [SerializeField]private float _attackRange = 1f;  
        //LVA: Range from the Enemy that allows the enemy to start attacking the player
        [SerializeField]private GameObject _player;   
        //LVA: Stating what the player Gameobject is
        [SerializeField]private GameObject _target;   
        //LVA: Stating what the target is that projectiles should be fired towards.
        [SerializeField]private GameObject _slashPrefab;  
        //LVA: The object to use when creating the slash projectile
        [SerializeField]private GameObject _center;   
        //LVA: The Center of the enemy.
        [SerializeField]private NavMeshAgent _agent;   
        //LVA: The NavMeshAgent is a component that allows for simulated AI movement.
        [SerializeField]private Animator _anim;   
        //LVA: Animator is the component that controls a GameObjects animation statemachine
        private AnimatorStateInfo currentBaseState;	
        public bool useCurves = true;
        private CapsuleCollider col;
        
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");
        private bool _onCooldown;
        [SerializeField]private float _cooldown;
        private bool _playerInRange;   
        //LVA: bool to check if player is within range.

        private float orgColHight; 
		private Vector3 orgVectColCenter;
        private void Start()
        {
            col = GetComponent<CapsuleCollider> ();
            _player = GameObject.FindGameObjectWithTag("Player");
            _target = _player.transform.GetChild(1).gameObject;  
            //LVA: the number in the brackets of .GetChild is to say "Get the object that is 1 from the top of the GameObjects children" and something to remember, all arrays start at 0, so 1 in this case is the second child of the GameObject
            orgColHight = col.height;
			orgVectColCenter = col.center;
            
        }
        private void Update()
        {
            float distance = Vector3.Distance(transform.position, _player.gameObject.transform.position); 
            //LVA: This is the distance from the Player to the Enemy. 


            if (distance >= _chaseRange)  
            //LVA: If the distance between the enemy and player is equal or greater to the chase range
            {
                _anim.SetFloat ("Speed", 0);  
                //LVA: Set the speed in the animator to 0, causing the idle animation to play.
                if (Plr_Manager.inst._chaser = gameObject)
                {
                    Plr_Manager.inst._chaser = null;
                    Plr_Manager.inst._beingChased = false;
                }
            }
            else if (distance <= _chaseRange && distance >= _attackRange && !Plr_Manager.inst._beingChased || Plr_Manager.inst._chaser == gameObject && distance <= _chaseRange && distance >= _attackRange)
            { //LVA: If the player is within the chase range radius AND the player is outside of the attack radius then do the following code.
                //chasing

                
                if (Plr_Manager.inst._chaser = gameObject)
                {
                    Plr_Manager.inst._chaser = null;
                    Plr_Manager.inst._beingChased = false;
                }
                chasing();
            }
            else if (distance < _chaseRange && distance < _attackRange  && !_onCooldown && Plr_Manager.inst._chaser == gameObject && !Plr_Manager.inst._centipedeOn)
            { //LVA: if the player is within the chase range radius, within the attack range radius, the enemy is not on cooldown and the enemy is the enemy currently chasing the player and the Centipede mode is not on, do the following code.
                //attac
                
                //if (_onCooldown) return;
                attacking();
                
            } 
        }
        private void OnTriggerEnter(Collider collision) 
        //LVA: this function was made because the attack function above does not work.
        {
            
            if (collision.tag == "Player" && !Plr_Manager.inst._centipedeOn)  
            //LVA: if the Enemy collides with the player and Centipede mode is not on, then do the following code.
            {
                Plr_Manager.inst.TakeDamage(1);  
                //LVA: Plr_Manager is a singleton, which means it is accessiable anywhere.
                //LVA: .TakeDamage() is a function in Plr_Manager, this script is accessing it and parsing through the int 1.
                Destroy(gameObject);  
                //LVA: This destroys the Game Object that this script is attatched too (which also destroys that instance of the script)
            }
        }

        private void attacking()
        {
            _anim.SetFloat ("Speed", 0);
            _playerInRange = true;
            _onCooldown = true;
            _agent.SetDestination(transform.position);  
            //LVA: .SetDestination of NavMeshAgent means to "walk" towards the position that is within the brackets
            //LVA: In this instance, I want the enemy to stop moving when attacking, so I set its walk position, to its current position.
            transform.LookAt(_player.transform); 
            //LVA: this causes the Game Object's transform that this script is attatched too to rotate and face towards the center of the transform in the brackets
            fireProjectile();
        }

        private void chasing()
        {
            _anim.SetFloat ("Speed", 1);  
            //LVA: Because the enemy is now chasing, Speed is set to 1, which causes the walk cycle animation to play and loop
            Plr_Manager.inst._chaser = gameObject;  
            //LVA: this sets the _chaser var of Plr_Manager to the game object attatched to this script.
            Plr_Manager.inst._beingChased = true;  
            //LVA: this sets the bool var _beingChased to true, which *should* be used to disallow other enemies from chasing the player (this does not work)
            _playerInRange = false;
            _agent.SetDestination(_player.transform.position); 
            //LVA: As I do want the enemy to start moving, the position used in NavMeshAgent.SetDestination is the player's transform position
            transform.LookAt(_player.transform);
        }


        public void TakeDamage(int damage)
        {
            _hp -= damage;  
            //LVA: the HP var gets removed by the damage var which is parsed through the function (look at the brackets after TakeDamage)
            CheckDeath();
        }


        private void CheckDeath()
        {
            if (_hp <= 0)  
            //LVA: If the Enemy's hp is equal or below 0 then it is dead, so I want to remove that enemy from the scene.
            {   
                Plr_Manager.inst._beingChased = false;
                Plr_Manager.inst.addTooInsanity();  
                //LVA: This game uses a tracker to determin if the player should be in 2D or 3D mode, when the enemy dies it adds to this tracker by calling this function.
                Destroy(gameObject);
            }
        }

        private void fireProjectile()
        {

            GameObject rangedAttack = Instantiate(_slashPrefab, _center.transform.position, Quaternion.identity);
            //LVA: Instantiate means to create
            //LVA: In Instantiation, the first var is the object to create, the second is where to create it and the third is rotation of the object.

            Vector3 direction = (_target.transform.position - _center.transform.position).normalized;
            //LVA: the direction that the projectile should fire itself towards, basing off of the Target and the Enemy's center.

            if (transform.rotation.y > 0)
            //LVA: if the enemy is facing to the right
            {
                rangedAttack.transform.eulerAngles = new Vector3(0,0,-90);
                //LVA: .eulerAngles is used to modify a GameObjects rotation directly
            }
            else
            //LVA: if the enemy is not facing to the right (the left)
            {
                rangedAttack.transform.eulerAngles = new Vector3(0,0,90);
                rangedAttack.transform.localScale = new Vector3(1,1,1);
            }

            
            rangedAttack.GetComponent<Rigidbody>().velocity = direction * _speed*4;
            //LVA: the ranged attack that has just been spawned needs to actually move towards the target or it will stay static
            //LVA: this adds the direction value from above and multiplies it by speed then by 4
            StartCoroutine(combatCooldown());
            //LVA: This coroutine is to allow the cooldown for attacking to be re-enabled.
        }
        IEnumerator combatCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            //LVA: the code after a WaitForSeconds() will NOT run until the amount of seconds have passed, in this case the amount of seconds is the variable _cooldown
            _onCooldown = false;
            //LVA: Once the wait is over, the bool _onCooldown is turned back to false.
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _chaseRange);
            //LVA: This allows for the chaseRange of the enemy to be visualised in the Editor as Red

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            //LVA: This allows for the attackRange of the enemy to be visualised in the Editor as Blue
        }

        
    }

}

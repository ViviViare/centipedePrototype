using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class Plr_Manager : MonoBehaviour
    {

        public static Plr_Manager inst;
        //LVA: This script is a singleton, meaning that only one of it should exist in the game and can be accessed anywhere
        //LVA: as long as it is referenced like this: Plr_Manager.inst
    
        private void Awake()
        {
            inst = this;
            //LVA: before any other code is ran, the variable inst needs to be set to *this* instance of this script.
            //LVA: usually you would also add some code to check if there is already a singleton of this script, and if so, delete the new one
            //LVA: but as I know for sure that another one will not exist, I do not need to add that
        }

        public int _damage;
        public int _hp;
        public int _killCount;
        //LVA: _killCount is used to track how many kills the player has got, and trigger the centipede mode once it reaches a set value
        public bool _centipedeOn;
        //LVA: this bool is set so that other scripts will be able to tell if the centipede mode has been turned on or not

        [SerializeField] private float _speed;
        [SerializeField] private float _cooldown;
        private bool _onCooldown;
        [SerializeField]private GameObject _slashPrefab;
        [SerializeField]private GameObject _target;
        [SerializeField]private GameObject _playerCenter;
        [SerializeField]private GameObject _player;
        [SerializeField]private GameObject _playerBody;
        [SerializeField]private GameObject _playerHead;
        private Camera _mainCam;
        //LVA: Unused
        [SerializeField]private GameObject cam2D;
        [SerializeField]private GameObject cam3DPov;
        public float _slashLifetime;
        public bool _beingChased;
        public GameObject _chaser;
        public SphereCollider autoAttackDistance;
        //LVA: Unused
        

        private void Start()
        {
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J) && !_onCooldown && !_centipedeOn)
            //LVA: Check if the player pressed the key J, the player is not on cooldown, and the player is not in centipede mode
            //LVA: The last check is to make sure the player cannot shoot during centipede mode, since combat is done automatically in that mode.
            {
                fireProjectile();
            }
        }

        private void fireProjectile() //LVA: The same as in Enemy_AI_2D
        {

            GameObject rangedAttack = Instantiate(_slashPrefab, _playerCenter.transform.position, Quaternion.identity);
            Vector3 direction = (_target.transform.position - _playerCenter.transform.position).normalized;

            if (_player.transform.rotation.y > 0) 
            {
                rangedAttack.transform.eulerAngles = new Vector3(0,0,-90);
            }
            else
            {
                rangedAttack.transform.eulerAngles = new Vector3(0,0,90);
                rangedAttack.transform.localScale = new Vector3(1,1,1);
            }

            
            rangedAttack.GetComponent<Rigidbody>().velocity = direction * _speed;
            StartCoroutine(combatCooldown());
        }
        IEnumerator combatCooldown()
        {
            yield return new WaitForSeconds(_cooldown);
            _onCooldown = false;
        }


        public void TakeDamage(int dmg)
        {
            _hp -= dmg;
            if (_hp <= 0)
            {
                Destroy(_player);
                Menus.instance.pause(true);
                //LVA: If the player dies, then the pause menu gets reused, but the death specific pause menu gets used as well.
            }
        }

        public void addTooInsanity()
        {
            _killCount++;
            //LVA: when increasing an int by 1, you can simply say int++ to increase it by 1.
            
            if (_killCount >= 16)
            //LVA: Technically the killcount max is meant to be 8, but for some reason each kill gives the player 2 kills added to the total
            //LVA: The reason is likely because of the way the enemies die (UNTESTED) and it runs that code twice (AGAIN UNTESTED)
            //LVA: To circumnavigate this, I just doubled the amount the player needs to go into Centipede mode
            {
                //Start 3D mode
                _playerBody.SetActive(false);
                //LVA: As the game is now in first person, the player's 3D model should NOT be visble.
                //LVA: In first person POV's, the player model is NEVER reused for that POV. 
                //LVA: Instead, either a new model is used specifically made for that POV (as to save resources / look correct)
                //LVA: or is entirely removed for the same reasons
                _playerHead.SetActive(false);
                //LVA: the head and body are different models in the free asset I was using, so I had to disable both of them seperately.
                cam3DPov.GetComponent<Camera>().enabled = true;
                //LVA: First enable the 3D First person camera
                cam2D.GetComponent<Camera>().enabled = false;
                //LVA: Then disable the current, 2.5D orthographic camera.
                //LVA: I enable/disable them in this order so that there is no camera issues in runtime
                //LVA: When there is not a camera in scene, things can generally go wrong.
                
                _centipedeOn = true;
            }
            else if (_killCount == 3)
            {
                cam2D.GetComponent<cam_2Deffects>().startShaking();
                //LVA: This is meant to start camera shaking effects once the player has hit 3 kills, it does not work.
            }
        }
    }
}

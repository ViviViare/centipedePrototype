using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class cam_follow : MonoBehaviour
    {
        public GameObject player;

        private Vector3 offset;

        void Start () 
        {
            
            offset = transform.position - player.transform.position;
            //LVA: the offset the camera should be at compared to the player is equal to where the camera currently is in the scene, before any gameplay occurs.
        }

        void LateUpdate () 
        {
            transform.position = player.transform.position + offset;
            //LVA: after regular Update() functions have occured, then LateUpdate() runs.
            //LVA: in this, the position of the camera is now equal to the position of the player with the offset included.
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class cam_FirstPersonEffects : MonoBehaviour
    //LVA: Note: Most of this does NOT work as intended.
    {
        public AnimationCurve currentCurve;
        //LVA: this Curve is used to determin the "intensity" of the screen shake
        public float duration = 5f;
        //LVA: the duration is how long the screenshake should last for
        public GameObject offset;
        void Start()
        {
            StartCoroutine(shake());
        }
        void LateUpdate () 
        {
            transform.position = offset.transform.position;
        }
        IEnumerator shake()
        {
            Vector3 startPos = transform.position;
            //LVA: 
            float elaspedTime = 0f; //LVA: Same time of elapsed time system as in the centiSkillCheck script

            while (elaspedTime < duration)
            {
                
                elaspedTime += Time.deltaTime;

                float strength = currentCurve.Evaluate(elaspedTime / duration);
                transform.position = startPos + Random.insideUnitSphere * strength;
                yield return null; 
                //LVA: All IEnumators need to yield return something. So this one returns null

                
            }
            transform.position = startPos;
            //LVA: reset the position of the camera back to its intial position
            startUp();
            //LVA: Because I want the first person screen shake to happen constantly, once the IEnumerator ends, it runs it again.
        }

        private void startUp()
        {
            StartCoroutine(shake());
        }
    }
}

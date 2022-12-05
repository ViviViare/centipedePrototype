using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class cam_2Deffects : MonoBehaviour
    //LVA: IMPORTANT NOTE: This script either does not work, or its effects do not get shown for whatever reason.
    //LVA: As of writing this comment 05/12/22 22:49, I have not solved it or even attempted too.
    //LVA: I will state my intentions for what is happening in this code
    //LVA: but bare in mind, it does not work.
    {

        public bool shakeOn;
        public bool canShake;
        public AnimationCurve curveLevel1;
        public AnimationCurve curveLevel2;
        public AnimationCurve curveLevel3;
        //LVA: I have three different animation curves used to have different "intensities" of screenshake


        public AnimationCurve currentCurve;
        public float duration = 1f;
        void Start()
        {
            StartCoroutine(shake());
        }

        public void startShaking()
        {
            canShake = true;
        }
        IEnumerator shake()
        {
            
            Vector3 startPos = transform.position;
            float elaspedTime = 0f;
            print("shaking");
            while (elaspedTime < duration && canShake)
            {
                elaspedTime += Time.deltaTime;

                if (Plr_Manager.inst._killCount <= 3)
                {
                    //LVA: Use curve1 when the killcount is equal or under to 3
                    currentCurve = curveLevel1;
                }
                else if (Plr_Manager.inst._killCount <= 5)
                {
                    //LVA: Use curve1 when the killcount is equal or under to 5 and not the above curve
                    currentCurve = curveLevel2;
                }
                else if (Plr_Manager.inst._killCount <= 8)
                {
                    //LVA: Use curve1 when the killcount is equal or under to 8 and not any of the above curves
                    //LVA: Since there is no reset to the currentCurve variable outside of this if/else statement, curveLevel3 is never replaced.
                    currentCurve = curveLevel3;
                }

                float strength = currentCurve.Evaluate(elaspedTime / duration);
                transform.position = startPos + Random.insideUnitSphere * strength;
                yield return null;
            }
            transform.position = startPos;
        }

        IEnumerator randomStart()
        {
            while (true)
            {
                float randomNum = 15f;
                if (Plr_Manager.inst._killCount <= 4)
                {
                    randomNum = Random.Range(3f,6f);
                }
                else if (Plr_Manager.inst._killCount <= 6)
                {
                    randomNum = Random.Range(2f,5f);
                }
                else if (Plr_Manager.inst._killCount <= 7)
                {
                    randomNum = Random.Range(1f,4f);
                }
                yield return new WaitForSeconds(randomNum);
                StartCoroutine(shake());  
            }
            //LVA: This IEnumerator is a remenant of a previous piece of code where, two numbers would be randomly chosen and if they align then the screen would shake
            //LVA: The idea is that, based on the killCount, the higher the killCount value, the more likely the numbers would align.
            //LVA: I couldn't get it to work so to make sure it didn't mess with anything, I deleted the other IEnumerator, but this one remained (probably because I forgot to delete it)
     
        }
    }
}

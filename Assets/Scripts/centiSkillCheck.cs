using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ViviViare
{
    public class centiSkillCheck : MonoBehaviour
    {
        private bool onCooldown;

        [SerializeField]private KeyCode[] possibleKeys;
        //LVA: When the variable type has a [] after it, that means it is an array of that type, so in this instance this is an array of KeyCodes
        [SerializeField]private string[] possibleKeysStrings;
        //LVA: Same as the Keycode array. I have two Serialized arrays so that I can match the Text and Keys together.
        [SerializeField]private TextMeshProUGUI letterText;
        //LVA: As this script is going to change a piece of text on the UI, I need to reference the TextMeshProUGUI, which contains the text element.
        [SerializeField]private Slider timerSlider;
        [SerializeField]private Canvas canvas;
        private int chosenLetter;
        public float gameTime;
        private bool stopTimer;
        private bool pressedTheButton;
        [SerializeField]private float elaspedTime;

        void Start()
        {
            resetTimer();
            //LVA: When the game starts I want the timer to be at a default
            newLetter();
            //LVA: When the game starts I want it to already have a letter chosen
            letterText.text = possibleKeysStrings[chosenLetter];
            //LVA: Because the previous function has set a chosen letter already, I am setting the text in letterText to that letter.
        }

        // Update is called once per frame
        void Update()
        {
            
            if (Plr_Manager.inst._centipedeOn)
            //LVA: This script only needs to do anything if the centipede mode is on
            {
                
                canvas.enabled = true;
                dangerZone();               
            }
            else
            {
                //LVA: if the centipede mode is off, then the canvas / UI is disabled
                canvas.enabled = false;
            }
            if (Input.GetKeyDown(possibleKeys[chosenLetter]) && Plr_Manager.inst._centipedeOn)
            {
                //LVA: when the player presses the key that has been chosen, then run the following code:
                print("correct Press!");
                StartCoroutine(catchout());
                resetTimer();
                newLetter();
                
            }
        }
        IEnumerator catchout()
        {
            //LVA: This IEnumator is to have a cooldown period between each button press.
            pressedTheButton = true;
            yield return new WaitForSeconds(0.2f);
            pressedTheButton = false;
        }

        private void newLetter()
        {
            chosenLetter = Random.Range(0, possibleKeys.Length);
            //LVA: The letter that the player needs to press is chosen by randomly choosing an int between 0 and the total amount of possible keys.
            //LVA: If it is 0 then it is the first key, if it is 5 then it is the 6th key, etc. etc.
            letterText.text = possibleKeysStrings[chosenLetter];
            //LVA: The chosenLetter int is then used to display the correct key in the text.
            //LVA: The two arrays have the exact same type of letters in them, but one is purely for pressable keys, one for letters.
            //LVA: Despite the key A and the letter A both being an A, because they are different types I have to seperate them into two different arrays.
        }

        private void resetTimer()
        {
            timerSlider.value = gameTime;
            //LVA: gameTime is the maximum amount of time the player has to press the button
            //LVA: So when the key has been pressed, the timer has to be reset back so that it can accurately display the new time.
            timerSlider.maxValue = gameTime;
            elaspedTime = 0f;
            //LVA: elapsedTime needs to be reset back to 0 so that the timer does not work off of the past key's timer
        }

        private void dangerZone()
        {
            elaspedTime += Time.deltaTime;
            //LVA: elapsedTime is essentially the amount of time it has been.
            //LVA: I cannot just purely use Time.deltaTime on its own, as it is not modifyable.
            if (!pressedTheButton)
            //LVA: if the player has NOT pressed the button then run this code
            {
                float time = gameTime - elaspedTime;
                //LVA: a variable called time is equal to the maximum amount of time inbetween key presses and the elapsed time since the key got last reset

                if (time <= 0 && !stopTimer)
                //LVA: If the elapsed time equals or exceeds 0 and the timer has not already stopped then run the next bit of code
                {
                    Plr_Manager.inst.TakeDamage(999);
                    //LVA: Because I want the player to die instantly when they fail to press the button, instead of directly killing the player
                    //LVA: I deal a high amount of damage that will always exceed the players maximum health
                    stopTimer = true;
                }

                if (!stopTimer)
                {
                    timerSlider.value = time;
                    //LVA: this updates the timeSliders value to equal the amount of time left, this shows the player how much time they have left to press the button.
                }
            }
        }



    }
}

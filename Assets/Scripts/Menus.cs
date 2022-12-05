using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
//LVA: because this script controls the Pause menu and Start menu, it needs to state that it is using Unity's UI functionality.
using UnityEngine.SceneManagement; 
//LVA: This script deals with reloading the level when the player presses Restart, so stating that it is using SceneManagement is nessissary.
using TMPro;  
//LVA: In Unity, whenever you are editing UI text, 99% of the time you will be using TextMeshPro (TMPro), so you have to declare that.
 //LVA: TMPro is **NOT** a part of Unity normally, it is a package you have to manually install via the Package Manager under Window in the editor.

namespace ViviViare  
//LVA: namespaces are essentially a directory of scripts, if a script is not in the same directory as another script, they cannot access each other (NOTE: I am still learning namespaces so this information may not be 100% correct)
{
    public class Menus : MonoBehaviour 
    {
        public static Menus instance; 
        //LVA: Public static means that there can only be one instance of this script, but it can be accessed anywhere without needing to define it.

        [SerializeField]private Canvas pauseMenuUI;
        [SerializeField]private Canvas startMenuUI;


        [SerializeField]private Button startButton;
        
        [SerializeField]private Button exitButton, startExitButton;
        [SerializeField]private Button resumeButton;
        [SerializeField]private Button replayButton;
        [SerializeField]private Camera cam2D;
        [SerializeField]private Camera cam3D;

        private Scene level;

        public bool gameIsPaused = false;
        private bool isDeadStill;

        void Awake() 
        //LVA: Everything in Awake() is ran once and before anything else in the script.
        {
            instance = this;
        }

        void Start()
        {
            exitButton.onClick.AddListener(exit);  
            //LVA: onClick.AddListener(); means that when the button is clicked, the function in the brackets will be ran.
            startExitButton.onClick.AddListener(exit);

            resumeButton.onClick.AddListener(resume);
            replayButton.onClick.AddListener(replay);
            startButton.onClick.AddListener(startactual);

            level = SceneManager.GetActiveScene();  
            //LVA: level is going to be used to Reload the level, so it needs to be defined as the current level (since that is what level needs to be reloaded)
            StartCoroutine(killTheTimeOnStart());  
            //LVA: StartCoroutine is used to run an IEnumarator, in this instance it is the IEnumarator "killTheTimeOnStart()"
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isDeadStill)  //LVA: If the user presses ESC and the player has not died, run the following code.
            {
                if (gameIsPaused)  
                //LVA: when a bool is used like this, it is asking "if (Bool is true)", writing it like this saves time. To do the opposite add a "!" before the varaible.
                {
                    resume();
                }
                else
                {
                    pause(false);
                }
            }
        }

        IEnumerator killTheTimeOnStart()
        {
            yield return new WaitForSeconds(0.1f);  
            //LVA: All IEnumators need to return a value, so having a WaitForSeconds wait for only a 10th of a second, can essentially circumnavigate this.
            startMenuUI.enabled = true;
            pauseMenuUI.enabled = false;
            Time.timeScale = 0f; 
            //LVA: Time.timeScale is essentially the speed that things run in the game. If it is 0 the game is essentially paused. If it as 1, it runs as normal.
        }

        public void pause(bool isDead) 
        {
            pauseMenuUI.enabled = true;  
            //LVA: When the game is paused, enable the pauseMenuUI, which allows the user to press the UI buttons.
            Time.timeScale = 0f;
            gameIsPaused = true;
            if (isDead)
            {
                isDeadStill = true;
                resumeButton.gameObject.SetActive(false);
            }
        }
        private void resume()
        {
            pauseMenuUI.enabled = false;  
            //LVA: When resuming the game, disable the pauseMenuUI, which stops the player being able to access the pause buttons.
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
        private void replay()
        {
            resume(); 
            resumeButton.gameObject.SetActive(true);
            isDeadStill = false;
            SceneManager.LoadScene(level.name);  
            //LVA: level is defined upabove 
        }

        private void exit()
        {
            Application.Quit();  
            //LVA: This will close the application. (NOTE: This function DOES NOT WORK in editor. You can only test if this works by making a build and running it)
        }

        private void startactual()
        {
            cam3D.enabled = false;  
            //LVA: This disables the 3D camera for the First Person POV
            cam2D.enabled = true;  
            //LVA: This enables the 2.5D camera for the Third Person POV
            startMenuUI.enabled = false;  
            //LVA: This disables the start menu UI.
            Time.timeScale = 1f;
        }
    }
}

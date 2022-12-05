using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViviViare
{
    public class Player_Movement2D : MonoBehaviour 
	//LVA: NOTE: This script is modified from the movement script from the unity-chan! package I installed for the player model
	//LVA: It was an immense pain to get it to work.
	//LVA: Some of the comments are in Japanese, I do not speak Japanese, I left them in.
	//LVA: For the stuff immediately following any Japanese comments, I probably do not know what they are, but I could guess.
    {
    	public float animSpeed = 1.5f;	
		public float lookSmoother = 3.0f;
		//LVA: unused
		public bool useCurves = true;

		public float forwardSpeed = 7.0f;
		public float _centiSpeed = 15.0f;
		public float backwardSpeed = 2.0f;
		//LVA: Unused
		public float rotateSpeed = 2.0f;
		//LVA: Unused
		public float jumpPower = 3.0f; 
		//LVA: Unused

		private CapsuleCollider col;
		private Rigidbody rb;
        private CharacterController cont;
		//LVA: Unused, was placed in case I decided to use CharacterControllers for player movement instead of NavAgents, instead I decided on neither.

		private Vector3 velocity;
		[SerializeField]private float _centiLag = 0.5f;
		//LVA: The centipede mode was going to have a delay between each movement, I eventually decided against this after playtesting.
		private bool _centiCantWalk;
		//LVA: Same as _centiLag

		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;
		private AnimatorStateInfo currentBaseState;	
		
		// アニメーター各ステートへの参照
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");

		// 初期化
		void Start ()
		{
			
			anim = GetComponent<Animator> ();
			
			col = GetComponent<CapsuleCollider> ();
			rb = GetComponent<Rigidbody> ();
			
			orgColHight = col.height;
			orgVectColCenter = col.center;
            
		}
        void FixedUpdate ()
		{
			if (!Plr_Manager.inst._centipedeOn)
			//LVA: For non centipede movement (the 2.5D movement) do the following {} code
			{
				float h = Input.GetAxis ("Horizontal");
				float v = Input.GetAxis ("Vertical");
				anim.SetFloat ("Speed", h);	
				anim.SetFloat ("Direction", v); 
				anim.speed = animSpeed;		
				currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
				rb.useGravity = true;
			

				velocity = new Vector3 (0, 0, h);
				//LVA: As the player only moves on the Z axis, I only need to have the velocity that the player will move by be on the Z part of a Vector3
				//LVA: Vector3's are a vector of 3 variables, (X, Y, Z)
				velocity = transform.TransformDirection (velocity);
				
				
				velocity *= forwardSpeed;
				//LVA: Velocity is times by the forward speed variable.
				//LVA: another way of writing this would be:
				//LVA: velocity = velocity * forwardSpeed;
				

				//LVA: when using GetAxis("Horizontal"), the returned value is a float number. Negative numbers mean that the player is moving towards the left
				//LVA: and positive numbers mean the player is moving towards the right

				//LVA: When using GetAxis("Vertical"), then Negative numbers mean the player is moving downwards, and positive numbers mean the player is moving upwards.

				if (h > 0.1f)
				//LVA: If the player is facing to the right
				{
					transform.eulerAngles = new Vector3(0, 90, 0);
					transform.localPosition += velocity * Time.fixedDeltaTime;
					//LVA: as the player is facing to the right, the velocity is as it should be
				}
				else if (h < -0.1f)
				//LVA: if the player is facing to the left
				{
					transform.eulerAngles = new Vector3(0, -90, 0);
					transform.localPosition -= velocity * Time.fixedDeltaTime;
					//LVA: as the player is facing to the left, the velocity used should be reversed (so if it was a velocity of 5, instead -5 should be used)
				}

				if (currentBaseState.fullPathHash == locoState) {
					//カーブでコライダ調整をしている時は、念のためにリセットする
					if (useCurves) {
						resetCollider ();
					}
				}

				// REST中の処理
				// 現在のベースレイヤーがrestStateの時
				else if (currentBaseState.fullPathHash == restState) {
					//cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
					// ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
					if (!anim.IsInTransition (0)) 
					{
						anim.SetBool ("Rest", false);
					}
				}
				void resetCollider ()
				{
					
					col.height = orgColHight;
					col.center = orgVectColCenter;
				}
			}
			else //Centipede mode
			{
				if (Input.GetKey(KeyCode.W) )
				{
					print("moving");
					_centiCantWalk = true;
					float v = Input.GetAxis ("Vertical");
					//LVA: In centipede mode, the W key is used instead of A & D, so the Vertical axis is required.
					velocity = new Vector3 (0, 0, v);
					//LVA: same as the velocity in the 2.5D movement, except using the vertical axis instead of the horizontal
					velocity = transform.TransformDirection (velocity);
					velocity *= _centiSpeed;
					//LVA: different speed is used for this mode, to add to the difference between the two modes.
					transform.localPosition += velocity * Time.fixedDeltaTime;
					
				}
			}
		}

			
    }
}

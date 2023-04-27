using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		public bool cast;
		public bool magicSpace;
		public bool option;
		public bool drop;
		public bool showControls;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnCast(InputValue value)
		{
			CastInput(value.isPressed);
		}
		public void OnMagicSpace(InputValue value)
		{
			MagicSpaceInput(value.isPressed);
		}
		public void OnOption(InputValue value)
		{
			OptionInput(value.isPressed);
		}

		public void OnDrop(InputValue value)
		{
			DropInput(value.isPressed);
		}

		public void OnShowControl(InputValue value)
		{
			ShowControlInput(value.isPressed);
		}


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void CastInput(bool castState)
		{
			cast = castState;
		}

		public void MagicSpaceInput(bool magicSpaceState)
		{
			magicSpace = magicSpaceState;
		}
		public void OptionInput(bool optionState)
		{
			option = optionState;
		}
		public void DropInput(bool dropState)
		{
			drop = dropState;
		}
		public void ShowControlInput(bool showState)
		{
			showControls = showState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			
		}

		private void Start() 
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public void ResetControls()
		{
			move = Vector2.zero;
			look = Vector2.zero;
			jump = false;
			sprint = false;
			cast = false;
			magicSpace = false;
			option = false;
		}
	}
	
}
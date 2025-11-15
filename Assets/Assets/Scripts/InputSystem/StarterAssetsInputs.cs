using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool isDrop;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool isShoot = false;
		public bool isZoom = false;



		[Header("Weapon Settings")]
		public bool isSelectedWeapon1 = false;
		public bool isSelectedWeapon2 = false;
		public bool isSelectedWeapon3 = false;
		public bool isReload 			= false;


#if ENABLE_INPUT_SYSTEM
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

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}

		public void OnZoom(InputValue value)
		{
			ZoomInput(value.isPressed);
		}

		public void OnSelectedWeapon1(InputValue value)
		{
			Weapon1Input(value.isPressed);
		}

		public void OnSelectedWeapon2(InputValue value)
		{
			Weapon2Input(value.isPressed);
		}

		public void OnSelectedWeapon3(InputValue value)
		{
			Weapon3Input(value.isPressed);
		}

		public void OnDrop(InputValue value)
		{
			DropInput(value.isPressed);
		}

		public void OnReload(InputValue value)
        {
			ReloadInput(value.isPressed);
        }
		
#endif



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

		public void ShootInput(bool newShootState)
		{
			isShoot = newShootState;
		}

		public void ZoomInput(bool newZoomState)
		{
			isZoom = !isZoom;
		}

		public void Weapon1Input(bool isSelectedWeapon1)
		{
			this.isSelectedWeapon1 = isSelectedWeapon1;
		}

		public void Weapon2Input(bool isSelectedWeapon2)
		{
			this.isSelectedWeapon2 = isSelectedWeapon2;
		}

		public void Weapon3Input(bool isSelectedWeapon3)
		{
			this.isSelectedWeapon3 = isSelectedWeapon3;
		}

		public void DropInput(bool isDrop)
		{
			this.isDrop = isDrop;
		}
		
		public void ReloadInput(bool isReload)
        {
			this.isReload = isReload;
        }
		

		//!-----------------------------------------------------------
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}
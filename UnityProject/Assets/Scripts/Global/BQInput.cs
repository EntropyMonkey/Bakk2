// BQInput.cs

using UnityEngine;
using System;

/// <summary>
/// manages the different input devices
/// the class is namend BQInput to avoid confusions with the input-class of the unityengine
/// BQ is an abbreviation for BalloonQuest
/// </summary>
public static class BQInput
{
	public class Helper
	{
		#region Pressed
		/// <summary>
		/// Returns whether the control is active now and has not been active last frame
		/// </summary>
		/// <param name="c">the control</param>
		public static bool Pressed( BQInput.Controls c )
		{
			return ( ( ( LastInput >> (int)c ) & 1 ) == 0 &&
					( ( CurrentInput >> (int)c ) & 1 ) == 1 ) &&
					!Locked[(int)c];
		}
		#endregion

		#region Released
		/// <summary>
		/// Returns whether the control has been released
		/// </summary>
		/// <param name="c">the control</param>
		public static bool Released( BQInput.Controls c )
		{
			return ( ( ( LastInput >> (int)c ) & 1 ) == 1 &&
					( ( CurrentInput >> (int)c ) & 1 ) == 0 ) &&
					!Locked[(int)c];
		}
		#endregion

		#region Active
		/// <summary>
		/// Returns whether the control is active now
		/// </summary>
		/// <param name="c">the control</param>
		public static bool Active( BQInput.Controls c )
		{
			return ( ( ( CurrentInput >> (int)c ) & 1 ) > 0 ) &&
				!Locked[(int)c];
		}
		#endregion
	}

	public enum Controls
	{
		Jump = 0,
		Attack,
        Use,
		Left,
		Right,
		Up,
		Down,
		Sprint,
		GirlInteraction,
		ThrowGirl,
		CameraLeft,
		CameraRight,
		CameraUp,
		CameraDown,
	};

	#region Static
	/// <summary>
	/// when all x input flags are set, the decimal number is 255 (2^X - 1) where x is the amount of different controls
	/// </summary>
	public static readonly int maxFlagsSet = (int)Math.Pow(2, Enum.GetValues(typeof(Controls)).Length) - 1;

	/// <summary>
	/// for all inputs locked
	/// </summary>
	public static bool[] Input_AllLocked = new bool[maxFlagsSet];
    /// <summary>
    /// Do not lock any input
    /// </summary>
    public static bool[] Input_NoneLocked = new bool[maxFlagsSet];


    /// <summary>
    /// The array stores for each control whether it is currently locked
    /// per array definition all input is by default locked
    /// </summary>
    private static bool[] locked = new bool[maxFlagsSet];
	private static bool[] lastLocked = new bool[maxFlagsSet];
    public static bool[] Locked
    {
        get
        {
            return locked;
        }
        set
        {
			if ( locked == value ) return;
			lastLocked = locked;
            locked = value;
            // if locking left or right, reset left/right value
            if (locked[(int)Controls.Right] && CurrentLeftRightValue > 0.0f)
                CurrentLeftRightValue = 0.0f;
            if (locked[(int)Controls.Left] && CurrentLeftRightValue < 0.0f)
                CurrentLeftRightValue = 0.0f;

            // reset up/down value
            if (locked[(int)Controls.Up] && CurrentUpDownValue > 0.0f)
                CurrentUpDownValue = 0.0f;
            if (locked[(int)Controls.Down] && CurrentUpDownValue < 0.0f)
                CurrentUpDownValue = 0.0f;

            // reset cameraleft/right
            if (locked[(int)Controls.CameraRight] && CurrentCameraLeftRightValue > 0.0f)
                CurrentCameraLeftRightValue = 0.0f;
            if (locked[(int)Controls.CameraLeft] && CurrentCameraLeftRightValue < 0.0f)
                CurrentCameraLeftRightValue = 0.0f;

            // reset camera up/down
            if (locked[(int)Controls.CameraUp] && CurrentCameraUpDownValue > 0.0f)
                CurrentCameraUpDownValue = 0.0f;
            if (locked[(int)Controls.CameraDown] && CurrentCameraUpDownValue < 0.0f)
                CurrentCameraUpDownValue = 0.0f;
        }
    }
	public static bool[] LastLocked { get { return lastLocked; } }

	#region CurrentInput
	/// <summary>
	/// The current input is calculated in the updateInput method,
	/// it stores whether a button is pressed or not
	/// </summary>
	public static int CurrentInput
	{
		internal set;
		get;
	}
	#endregion

	#region LastInput
	/// <summary>
	/// The current input from the last frame
	/// </summary>
	public static int LastInput
	{
		internal set;
		get;
	}
	#endregion

	#region CurrentLeftRightValue
	/// <summary>
	/// Container for Input.GetAxis("Horizontal"). Maybe we will later have
	/// more than this input method.
	/// </summary>
	public static float CurrentLeftRightValue
	{
		internal set;
		get;
	}
	#endregion

	#region CurrentCameraLeftRightValue
	/// <summary>
	/// Container for Input.GetAxis("RightHorizontal")
	/// </summary>
	public static float CurrentCameraLeftRightValue
	{
		internal set;
		get;
	}
	#endregion

	#region CurrentUpDownValue
	/// <summary>
	/// Container for Input.GetAxis("Vertical"). Maybe we will later have
	/// more than this input method.
	/// </summary>
	public static float CurrentUpDownValue
	{
		internal set;
		get;
	}
	#endregion

	#region CurrentCameraUpDownValue
	/// <summary>
	/// Container for Input.GetAxis("RightVertical")
	/// </summary>
	public static float CurrentCameraUpDownValue
	{
		internal set;
		get;
	}
	#endregion

	#region Jump
	/// <summary>
	/// Jump is pressed at the moment
	/// </summary>
	public static bool Jump
	{
		get
		{
			return Helper.Active(Controls.Jump);
		}
	}

	/// <summary>
	/// Jump has been released this frame
	/// </summary>
	public static bool JumpReleased
	{
		get
		{
			return Helper.Released(Controls.Jump);
		}
	}

	/// <summary>
	/// jump has started to be pressed this frame
	/// </summary>
	public static bool JumpPressed
	{
		get
		{
			return Helper.Pressed(Controls.Jump);
		}
	}
	#endregion

	#region Attack
	/// <summary>
	/// Attack is pressed at the moment
	/// </summary>
	public static bool Attack
	{
		get
		{
            return Helper.Active(Controls.Attack);
		}
	}
	/// <summary>
	/// Attack has been released this frame
	/// </summary>
	public static bool AttackReleased
	{
		get
		{
            return Helper.Released(Controls.Attack);
		}
	}

	/// <summary>
	/// Attack has started to be pressed this frame
	/// </summary>
	public static bool AttackPressed
	{
		get
		{
            return Helper.Pressed(Controls.Attack);
		}
	}
	#endregion

    #region Use
    /// <summary>
    /// Use is pressed at the moment
    /// </summary>
    public static bool Use
    {
        get
        {
            return Helper.Active(Controls.Use);
        }
    }
    /// <summary>
    /// Use has been released this frame
    /// </summary>
    public static bool UseReleased
    {
        get
        {
            return Helper.Released(Controls.Use);
        }
    }

    /// <summary>
    /// Use has started to be pressed this frame
    /// </summary>
    public static bool UsePressed
    {
        get
        {
            return Helper.Pressed(Controls.Use);
        }
    }
    #endregion

	#region GirlInteraction
	/// <summary>
	/// GirlInteraction is currently pressed
	/// </summary>
	public static bool GirlInteraction
	{
		get
		{
            return Helper.Active(Controls.GirlInteraction);
		}
	}

	/// <summary>
	/// GirlInteraction has been released this frame
	/// </summary>
	public static bool GirlInteractionReleased
	{
		get
		{
            return Helper.Released(Controls.GirlInteraction);
		}
	}

	/// <summary>
	/// girlInteraction has started to be pressed this frame
	/// </summary>
	public static bool GirlInteractionPressed
	{
		get
		{
            return Helper.Pressed(Controls.GirlInteraction);
		}
	}
	#endregion

	#region Sprint
	/// <summary>
	/// sprint is currently pressed
	/// </summary>
	public static bool Sprint
	{
		get
		{
            return Helper.Active(Controls.Sprint);
		}
	}

	/// <summary>
	/// Sprint has been released this frame
	/// </summary>
	public static bool SprintReleased
	{
		get
		{
            return Helper.Released(Controls.Sprint);
		}
	}

	/// <summary>
	/// Sprint has started to be pressed this frame
	/// </summary>
	public static bool SprintPressed
	{
		get
		{
            return Helper.Pressed(Controls.Sprint);
		}
	}
	#endregion

	#region Left
	/// <summary>
	/// Left is currently pressed
	/// </summary>
	public static bool Left
	{
		get
		{
            return Helper.Active(Controls.Left);
		}
	}

	/// <summary>
	/// Left has been released this frame
	/// </summary>
	public static bool LeftReleased
	{
		get
		{
            return Helper.Released(Controls.Left);
		}
	}

	/// <summary>
	/// Left has started to be pressed this frame
	/// </summary>
	public static bool LeftPressed
	{
		get
		{
            return Helper.Pressed(Controls.Left);
		}
	}
	#endregion

	#region Right
	/// <summary>
	/// Left is currently pressed
	/// </summary>
	public static bool Right
	{
		get
		{
            return Helper.Active(Controls.Right);
		}
	}

	/// <summary>
	/// Right has been released this frame
	/// </summary>
	public static bool RightReleased
	{
		get
		{
            return Helper.Released(Controls.Right);
		}
	}

	/// <summary>
	/// Right has started to be pressed this frame
	/// </summary>
	public static bool RightPressed
	{
		get
		{
            return Helper.Pressed(Controls.Right);
		}
	}
	#endregion

	#region Up
	/// <summary>
	/// Up is currently pressed
	/// </summary>
	public static bool Up
	{
		get
		{
            return Helper.Active(Controls.Up);
		}
	}

	/// <summary>
	/// Up has been released this frame
	/// </summary>
	public static bool UpReleased
	{
		get
		{
            return Helper.Released(Controls.Up);
		}
	}

	/// <summary>
	/// Up has started to be pressed this frame
	/// </summary>
	public static bool UpPressed
	{
		get
		{
            return Helper.Pressed(Controls.Up);
		}
	}
	#endregion

	#region Down
	/// <summary>
	/// Down is currently pressed
	/// </summary>
	public static bool Down
	{
		get
		{
            return Helper.Active(Controls.Down);
		}
	}

	/// <summary>
	/// Down has been released this frame
	/// </summary>
	public static bool DownReleased
	{
		get
		{
            return Helper.Released(Controls.Down);
		}
	}

	/// <summary>
	/// Down has started to be pressed this frame
	/// </summary>
	public static bool DownPressed
	{
		get
		{
            return Helper.Pressed(Controls.Down);
		}
	}
	#endregion

	#region CameraLeft
	/// <summary>
	/// Left is currently pressed
	/// </summary>
	public static bool CameraLeft
	{
		get
		{
            return Helper.Active(Controls.CameraLeft);
		}
	}

	/// <summary>
	/// Left has been released this frame
	/// </summary>
	public static bool CameraLeftReleased
	{
		get
		{
            return Helper.Released(Controls.CameraLeft);
		}
	}

	/// <summary>
	/// Left has started to be pressed this frame
	/// </summary>
	public static bool CameraLeftPressed
	{
		get
		{
            return Helper.Pressed(Controls.CameraLeft);
		}
	}
	#endregion

	#region CameraRight
	/// <summary>
	/// Left is currently pressed
	/// </summary>
	public static bool CameraRight
	{
		get
		{
            return Helper.Active(Controls.CameraRight);
		}
	}

	/// <summary>
	/// Right has been released this frame
	/// </summary>
	public static bool CameraRightReleased
	{
		get
		{
            return Helper.Released(Controls.CameraRight);
		}
	}

	/// <summary>
	/// Right has started to be pressed this frame
	/// </summary>
	public static bool CameraRightPressed
	{
		get
		{
            return Helper.Pressed(Controls.CameraRight);
		}
	}
	#endregion

	#region CameraUp
	/// <summary>
	/// Up is currently pressed
	/// </summary>
	public static bool CameraUp
	{
		get
		{
            return Helper.Active(Controls.CameraUp);
		}
	}

	/// <summary>
	/// Up has been Helper.Released this frame
	/// </summary>
	public static bool CameraUpReleased
	{
		get
		{
            return Helper.Released(Controls.CameraUp);
		}
	}

	/// <summary>
	/// Up has started to be pressed this frame
	/// </summary>
	public static bool CameraUpPressed
	{
		get
		{
			return Helper.Pressed(Controls.CameraUp);
		}
	}
	#endregion

	#region CameraDown
	/// <summary>
	/// Down is currently pressed
	/// </summary>
	public static bool CameraDown
	{
		get
		{
            return Helper.Active(Controls.CameraDown);
		}
	}

	/// <summary>
	/// Down has been released this frame
	/// </summary>
	public static bool CameraDownReleased
	{
		get
		{
            return Helper.Released(Controls.CameraDown);
		}
	}

	/// <summary>
	/// Down has started to be pressed this frame
	/// </summary>
	public static bool CameraDownPressed
	{
		get
		{
            return Helper.Pressed(Controls.CameraDown);
		}
	}
	#endregion

	#region ThrowGirl
	/// <summary>
	/// Throw Girl is currently pressed
	/// </summary>
	public static bool ThrowGirl
	{
		get
		{
            return Helper.Active(Controls.ThrowGirl);
		}
	}

	/// <summary>
	/// Throw Girl has been released this frame
	/// </summary>
	public static bool ThrowGirlReleased
	{
		get
		{
            return Helper.Released(Controls.ThrowGirl);
		}
	}

	/// <summary>
	/// Throw girl has been started to be pressed this frame
	/// </summary>
	public static bool ThrowGirlPressed
	{
		get
		{
            return Helper.Pressed(Controls.ThrowGirl);
		}
	}
	#endregion

	#endregion

	#region Methods
	internal static void UpdateInput()
	{
		BQInput.LastInput = BQInput.CurrentInput;

		#region Left|Right
		BQInput.CurrentLeftRightValue = UnityEngine.Input.GetAxis("Horizontal");
		if (BQInput.CurrentLeftRightValue > 0)
		{
            // handle locked right input
            if (Locked[(int)Controls.Right])
            {
                CurrentLeftRightValue = 0.0f;
            }
            else
            {
                // if already switched on, dont switch off, if switched off, switch on
                BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Right;
                // switch off left
                BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Left);
            }
		}
		else if (BQInput.CurrentLeftRightValue < 0)
		{
            // handle locked left input
            if (Locked[(int)Controls.Left])
            {
                CurrentLeftRightValue = 0.0f;
            }
            else
            {
                // switch on left
                BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Left;
                // switch off right
                BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Right);
            }
		}
		else
		{
			// set input to zero at right and left
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Right);
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Left);
		}
		#endregion

		#region Up|Down
		BQInput.CurrentUpDownValue = UnityEngine.Input.GetAxis("Vertical");
		if (BQInput.CurrentUpDownValue > 0)
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Up;
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Down);
		}
		else if (BQInput.CurrentUpDownValue < 0)
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Down;
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Up);
		}
		else
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Up);
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Down);
		}
		#endregion

		#region CameraLeft|CameraRight
		BQInput.CurrentCameraLeftRightValue = UnityEngine.Input.GetAxis("RightHorizontal");
		if (BQInput.CurrentCameraLeftRightValue > 0)
		{
			// if already switched on, dont switch off, if switched off, switch on
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.CameraRight;
			// switch off left
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraLeft);
		}
		else if (BQInput.CurrentCameraLeftRightValue < 0)
		{
			// switch on left
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.CameraLeft;
			// switch off right
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraRight);
		}
		else
		{
			// set input to zero at right and left
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraRight);
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraLeft);
		}
		#endregion

		#region CameraUp|CameraDown
		BQInput.CurrentCameraUpDownValue = UnityEngine.Input.GetAxis("RightVertical");
		if (BQInput.CurrentCameraLeftRightValue > 0)
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.CameraUp;
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraDown);
		}
		else if (BQInput.CurrentCameraUpDownValue < 0)
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.CameraDown;
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraUp);
		}
		else
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraUp);
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.CameraDown);
		}
		#endregion

		#region Jump
		if (UnityEngine.Input.GetButton("Jump"))
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Jump;
		}

		if (UnityEngine.Input.GetButtonUp("Jump"))
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, 
				(double)BQInput.Controls.Jump);
		}
		#endregion

		#region GirlInteraction
		if (UnityEngine.Input.GetButtonDown("GirlInteraction") ||
			UnityEngine.Input.GetMouseButtonDown(0))
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.GirlInteraction;
		}

		if (UnityEngine.Input.GetButtonUp("GirlInteraction") ||
			UnityEngine.Input.GetMouseButtonUp(0))
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.GirlInteraction);
		}
		#endregion

		#region Attack
		if (UnityEngine.Input.GetButtonDown("Attack"))
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Attack;
		}

		if (UnityEngine.Input.GetButtonUp("Attack"))
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Attack);
		}
		#endregion

        #region Use
        if (UnityEngine.Input.GetButtonDown("Attack"))
        {
            BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Use;
        }

        if (UnityEngine.Input.GetButtonUp("Attack"))
        {
            BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Use);
        }
        #endregion

		#region Sprint
		if (UnityEngine.Input.GetAxis("DPadVDepth") == -1 ||
			UnityEngine.Input.GetButtonDown("Sprint"))
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.Sprint;
		}
		
		if (UnityEngine.Input.GetAxis("DPadVDepth") != -1 &&
			!UnityEngine.Input.GetButton("Sprint"))
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.Sprint);
		}
		#endregion

		#region ThrowGirl
		if (UnityEngine.Input.GetButtonDown("ThrowGirl"))
		{
			BQInput.CurrentInput |= 1 << (int)BQInput.Controls.ThrowGirl;
		}

		if (UnityEngine.Input.GetButtonUp("ThrowGirl"))
		{
			BQInput.CurrentInput &= BQInput.maxFlagsSet - (int)Math.Pow(2, (double)BQInput.Controls.ThrowGirl);
		}

		#endregion
	}

    #endregion
}

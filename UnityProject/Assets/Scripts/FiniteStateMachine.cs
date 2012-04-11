﻿/*
 * Prints state changes
 * ALL - all state changes (player, enemies, girl)
 * PLAYER - only player
 */
//#define DEBUG_STATES_ALL
//#define DEBUG_STATES_PLAYER

using UnityEngine;
using System;

public class FiniteStateMachine<T>
{
	public FSMState<T> CurrentState
	{
		get;
		private set;
	}

	public FSMState<T> PreviousState
	{
		get;
		private set;
	}

	public FSMState<T> GlobalState
	{
		get;
		private set;
	}

	public T Owner
	{
		get;
		private set;
	}

	public FiniteStateMachine()
	{
		CurrentState = null;
		PreviousState = null;
		GlobalState = null;
	}

	public void Configure( T owner, FSMState<T> initialState, FSMState<T> globalState )
	{
		Owner = owner;

		if( globalState != null )
		{
			GlobalState = globalState;
			GlobalState.Enter( Owner );
		}
		
		ChangeState( initialState );
	}

	public void Update()
	{
		if( GlobalState != null )
			GlobalState.Execute( Owner );
		if( CurrentState != null )
			CurrentState.Execute( Owner );
	}


	public bool ChangeState( FSMState<T> NewState, bool forceChange = false )
	{
        if (NewState == CurrentState && !forceChange) 
            return false;

#if DEBUG_STATES_ALL
        Debug.Log("FSM(" + typeof(T) + "):Change State " + CurrentState + " to " + NewState);
#endif
#if DEBUG_STATES_PLAYER
        if (typeof(T) == typeof(PlayerController))
        {
            Debug.Log("FSM(PlayerController): Change State " + CurrentState + " to " + NewState + "\n" + Environment.StackTrace);
        }
#endif

		PreviousState = CurrentState;
		CurrentState = NewState;
		
		if( PreviousState != null )
			PreviousState.Exit( Owner );
		
		if( CurrentState != null )
		    CurrentState.Enter( Owner );

		return true;
	}

	public bool ChangeGlobalState( FSMState<T> NewGlobalState )
	{
		if ( NewGlobalState != null )
		{
			GlobalState.Exit( Owner );
			NewGlobalState.Enter( Owner );
			GlobalState = NewGlobalState;
			return true;
		}
		else return false;
	}
};
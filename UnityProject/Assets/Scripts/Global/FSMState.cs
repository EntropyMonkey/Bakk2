// FSMState.cs

using UnityEngine;

abstract public class FSMState<T> : ScriptableObject
{
	abstract public void Enter( T entity );
	abstract public void Execute( T entity );
	abstract public void Exit( T entity );
}
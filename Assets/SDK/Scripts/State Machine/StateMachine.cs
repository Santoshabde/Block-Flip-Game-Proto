using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public abstract class StateMachine : MonoBehaviour
    {
        [Header("Only-For Debug Purpose")]
        [SerializeField] private string currentDebugState;

        protected State currentState;

        public State CurrentState => currentState;

        public virtual void SwitchState(State newState)
        {
            currentDebugState = newState.ToString();

            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        protected virtual void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }
    }
}
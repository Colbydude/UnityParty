using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityParty.Helpers
{
    public class StateMachine
    {
        private List<IState> m_states = new List<IState>();
        private IState m_currState;

        public void AddState<T>() where T : IState, new()
        {
            if (GetState<T>() != null)
            {
                throw new ArgumentException("State already exists in state machine", typeof(T).ToString());
            }

            m_states.Add(new T());

            if (m_states.Count == 1)
            {
                ChangeState<T>();
            }
        }

        public void ChangeState<T>() where T : IState
        {
            if (m_currState != null && typeof(T) == m_currState.GetType())
            {
                throw new ArgumentException("Cannot change state to current state", typeof(T).ToString());
            }

            IState nextState = GetState<T>();

            if (nextState == null)
            {
                throw new ArgumentException("State does not exist in state machine", typeof(T).ToString());
            }
            else
            {
                m_currState = nextState;
                m_currState.Start();
            }
        }

        public IState GetCurrentState()
        {
            return m_currState;
        }

        public void Update()
        {
            if (m_currState != null)
            {
                m_currState.Update();
            }
        }

        private IState GetState<T>()
        {
            foreach (IState state in m_states)
            {
                if (state is T)
                {
                    return state;
                }
            }

            return null;
        }
    }
}

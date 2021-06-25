using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Helper
{
    public class StateMachine
    {
        private List<IState> m_states = new List<IState>();
        private IState m_currState;

        public void AddState<T>() where T : IState, new()
        {
            m_states.Add(new T());

            if (m_states.Count == 1)
            {
                ChangeState<T>();
            }
        }

        public void ChangeState<T>() 
        {
            foreach (IState state in m_states)
            {
                if (state.GetType() == typeof(T))
                {
                    m_currState = state;
                    state.Start();
                    break;
                }
            }
        }

        public IState GetCurrentState()
        {
            return m_currState;
        }

        public void Update()
        {
            m_currState.Update();
        }
    }
}
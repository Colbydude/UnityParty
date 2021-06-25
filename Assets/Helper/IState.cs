using System.Collections;
using UnityEngine;

namespace Assets.Helper
{
    public interface IState
    {
        void Start();
        void Update();
    }
}
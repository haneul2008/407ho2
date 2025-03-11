using System;
using UnityEngine;

namespace Work.HN.Code.Save
{
    public abstract class DelegateModerator<T> : MonoBehaviour, IModerator where T : Delegate
    {
        protected T _action;

        public virtual void SetAction(T action)
        {
            _action = action;
        }

        public abstract bool Execute();
    }
}
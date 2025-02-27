using System;
using UnityEngine;

namespace Work.JW.Code.Animators
{
    [CreateAssetMenu(fileName = "AnimParam", menuName = "SO/FSM/Param", order = 0)]
    public class AnimParamSO : ScriptableObject
    {
        public string parameterName;
        public int hashValue;
        
        [TextArea]
        public string description;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(parameterName);
        }
    }
}
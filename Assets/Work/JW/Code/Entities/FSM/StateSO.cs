using UnityEngine;
using Work.JW.Code.Animators;

namespace Work.JW.Code.Entities.FSM
{
    [CreateAssetMenu(fileName = "State", menuName = "SO/FSM/State", order = 0)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public AnimParamSO param;
    }
}
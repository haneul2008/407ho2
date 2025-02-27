﻿using System.Collections.Generic;
using UnityEngine;

namespace Work.JW.Code.Entities.FSM
{
    [CreateAssetMenu(fileName = "StateList", menuName = "SO/FSM/StateList", order = 0)]
    public class StateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}
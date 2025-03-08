using System;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Work.HN.Code.Input;
using Work.JW.Code.Entities.FSM;
using Work.JW.Code.MapLoad;
using Work.JW.Code.MapLoad.UI;
using Work.JW.Code.TriggerSystem;

namespace Work.JW.Code.Entities.Player
{
    public class PlayerNetwork : Player
    {
        protected override void InitEvent()
        {
            if(!IsOwner) return;

            base.InitEvent();
            
            GetCompo<EntityMover>().CanMove = false;
            
            FindAnyObjectByType<MapLoadManager>().OnMapLoaded.AddListener(() => GetCompo<EntityMover>().CanMove = true);
        }
    }
}
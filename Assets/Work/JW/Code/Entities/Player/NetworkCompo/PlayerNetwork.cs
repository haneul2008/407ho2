using System;
using Ami.BroAudio;
using Unity.Netcode;
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
        protected override void InitializeCompo()
        {
            base.InitializeCompo();
            
            GetCompo<EntityMover>().CanMove = false;
            GetCompo<EntityMover>().SetGravityScale(0);
            
            FindAnyObjectByType<MapLoadManager>().OnMapLoaded.AddListener(() => GetCompo<EntityMover>().CanMove = true);
            FindAnyObjectByType<MapLoadManager>().OnMapLoaded.AddListener(() => GetCompo<EntityMover>().SetGravityScale(1.22f));
        }

        protected override void Update()
        {
            if (!IsOwner) return;
            base.Update();
        }

        protected override void FixedUpdate()
        {
            if (!IsOwner) return;
            base.FixedUpdate();
        }

        public override void ChangeState(string newStateName)
        {
            if (!IsOwner) return;
            
            base.ChangeState(newStateName);
        }
    }
}
using UnityEngine;
using Work.JW.Code.MapLoad;

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

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsOwner) gameObject.layer = LayerMask.NameToLayer("Default");
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
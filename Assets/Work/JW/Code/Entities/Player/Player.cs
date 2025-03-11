using Ami.BroAudio;
using UnityEngine;
using Work.HN.Code.Input;
using Work.JW.Code.Entities.FSM;
using Work.JW.Code.MapLoad;
using Work.JW.Code.MapLoad.UI;
using Work.JW.Code.TriggerSystem;

namespace Work.JW.Code.Entities.Player
{
    public class Player : Entity
    {
        [field: SerializeField] public InputReaderSO InputReader { get; private set; }

        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private StateListSO stateList;

        [field: SerializeField] public SoundID JumpSoundID { get; private set; }
        [field: SerializeField] public SoundID DieSoundID { get; private set; }
        [field: SerializeField] public SoundID GetTriggerSoundID { get; private set; }

        private StateMachine _stateMachine;


        protected override void InitializeCompo()
        {
            base.InitializeCompo();

            _stateMachine = new StateMachine(this, stateList);

            InputReader.SetEnable(InputType.MapMaker, false);
            InputReader.SetEnable(InputType.Player, true);

            OnHit.AddListener(() => FindAnyObjectByType<FadeInOut>().Fade(true));
            OnHit.AddListener(() => GetCompo<EntityMover>().StopImmediately(true));
            OnHit.AddListener(() => GetCompo<EntityMover>().CanMove = false);
            OnHit.AddListener(() => FindAnyObjectByType<MapLoadManager>().Clear());
            OnHit.AddListener(() => FindAnyObjectByType<MapLoadManager>().SetMapObjSpawn());
        }

        private void Start()
        {
            ChangeState("IDLE");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            InputReader.ClearPlayerAction();
        }

        protected virtual void Update()
        {
            _stateMachine.StateMachineUpdate();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine.StateMachineFixedUpdate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ITriggerEvent trigger))
            {
                BroAudio.Play(GetTriggerSoundID);
                trigger.TriggerEvent(this);
            }

            if (other.gameObject.layer == 8)
            {
                OnDead();
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                OnDead();
            }
        }

        public void ChangeState(string newStateName)
        {
            if (!IsOwner) return;
            
            _stateMachine.ChangeState(newStateName);
        }

        public override void OnDead()
        {
            BroAudio.Play(DieSoundID);
            base.OnDead();
            ChangeState("IDLE");
        }
    }
}
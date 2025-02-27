using UnityEngine;
using Work.HN.Code.MapMaker;
using Work.HN.Code.MapMaker.Core;

namespace Work.HN.Code.Test
{
    public class TestBlock : MonoBehaviour, ISpawnable
    {
        [field: SerializeField] public int ID { get; private set; }

        private void OnValidate()
        {
            if(ID == 0)
                ID = GetInstanceID();
        }

        public void Spawn()
        {
            print("Spawn Test Block");
        }
    }
}

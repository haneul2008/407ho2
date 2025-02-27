using UnityEngine;
using Work.JW.Code.Entities;

namespace Work.ISC._0._Scripts.Objects
{
    public class PlayerSpawnPos : MonoBehaviour
    {
        private StartPointObjectSO _startPointObj;

        private void Awake()
        {
            _startPointObj = FindAnyObjectByType<StartPointObjectSO>();

            _startPointObj.OnGameStartEvent += SpawnPlayer;
        }

        private void SpawnPlayer(Entity entity)
        {
            entity.transform.position = transform.position;
        }
    }
}
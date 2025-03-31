using System.Collections.Generic;
using UnityEngine;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.ObjectManagement;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Spawn
{
    public class ObjectSpawnPanel : ObjectEditPanel<ObjectSpawner>
    {
        [SerializeField] private ObjectListSO objectList;
        [SerializeField] private ObjectSpawnUI spawnUIPrefab;

        private readonly Dictionary<int, ObjectSpawnUI> _objectUIPairs = new Dictionary<int, ObjectSpawnUI>();
        
        protected override void Awake()
        {   
            base.Awake();
            
            for (int i = 0; i < objectList.objects.Count; i++)
            {
                EditorObject targetObj = objectList.objects[i];

                if (_objectUIPairs.ContainsKey(targetObj.ID))
                {
                    Debug.LogWarning($"{targetObj} ID is already in use");
                    continue;
                }
                
                ObjectSpawnUI objectSpawnUI = Instantiate(spawnUIPrefab, transform);
                objectSpawnUI.Initialize(targetObj);
                _objectUIPairs.Add(targetObj.ID, objectSpawnUI);
            }
        }
    }
}
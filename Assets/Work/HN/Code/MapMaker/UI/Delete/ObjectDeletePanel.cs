using UnityEngine;
using Work.HN.Code.MapMaker.Core;
using Work.HN.Code.MapMaker.ObjectManagement;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.UI.Delete
{
    public class ObjectDeletePanel : ObjectEditPanel<DeleteEditor>
    {
        [SerializeField] private ObjectListSO objectList, triggerList;
        [SerializeField] private DeleteBtn deleteBtn;
        [SerializeField] private Transform contentTrm;

        private void Start()
        {
            SpawnBtn(objectList);
            SpawnBtn(triggerList);
        }

        private void SpawnBtn(ObjectListSO objectListSo)
        {
            foreach (EditorObject editorObject in objectListSo.objects)
            {
                DeleteBtn btn = Instantiate(deleteBtn, contentTrm);
                btn.Initialize(editorObject);
            }
        }
    }
}
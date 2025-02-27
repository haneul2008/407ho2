using UnityEngine;

namespace Work.HN.Code.MapMaker.History
{
    public interface IHistory
    {
        public void Undo();
        public void Redo();
        public void OnDestroy();
    }
}
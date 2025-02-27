using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.Core
{
    public interface ITriggerIdUpdate
    {
        public void OnTriggerIdUpdate(int triggerID, EditorObject targetObject);
    }
}
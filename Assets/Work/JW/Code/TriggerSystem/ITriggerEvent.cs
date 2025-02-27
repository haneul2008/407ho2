using Work.JW.Code.Entities;

namespace Work.JW.Code.TriggerSystem
{
    public interface ITriggerEvent
    {
        public void TriggerEvent(Entity entity);
    }
}
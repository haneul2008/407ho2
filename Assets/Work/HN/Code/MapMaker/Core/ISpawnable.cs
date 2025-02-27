namespace Work.HN.Code.MapMaker.Core
{
    public interface ISpawnable
    {
        public int ID { get; }
        
        public void Spawn();
    }
}
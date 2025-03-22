using Unity.Netcode;

namespace Work.JW.Code.Network
{
    public class SingletonNetworkManager : NetworkManager
    {
        protected override void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }
            
            base.Awake();
        }
    }
}
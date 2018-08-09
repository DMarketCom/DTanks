using UnityEngine;

namespace TankGame.GameServer.ServerStorage
{
    public class ServerStorageContainer: MonoBehaviour
    {
        private enum ServerStorageType
        {
            SciptableObject,
            FirebaseStorage
        }

        [SerializeField]
        private ServerStorageType _storageType;
        [SerializeField]
        private int _storageVersion = 2;

        public ServerSciptableObjectStorage ScriptableObject;

        public IServerStorage Storage {
            get { return GetServerStorage(); }
        }

        private IServerStorage GetServerStorage()
        {
            switch(_storageType)
            {
                case ServerStorageType.SciptableObject:
                    return ScriptableObject;
                case ServerStorageType.FirebaseStorage:
                    return new ServerFirebaseStorage(_storageVersion);
                default:
                    return null;
            }
        }
    }
}

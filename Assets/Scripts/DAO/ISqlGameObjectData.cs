using System.Collections.Generic;
using Spawner;

namespace DefaultNamespace.DAO
{
    public interface ISqlGameObjectData
    {
        void Save(GameObjectData obj);

        List<GameObjectData> Load();
    }
}
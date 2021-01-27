using System.Collections.Generic;
using Spawner;

namespace DefaultNamespace.DAO
{
    public interface ISqlGameObjectData<T>
    {
        void Save(T obj);

        List<T> Load(int saveId);
    }
}
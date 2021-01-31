using System.Collections.Generic;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public interface ISqlGameObjectData<T>
    {
        void Save(T obj);

        List<T> Load(int saveId);
    }
}
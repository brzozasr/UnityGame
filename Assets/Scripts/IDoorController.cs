using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IDoorController
    {
        void OpenDoor(object sender, string chip);
    }
}
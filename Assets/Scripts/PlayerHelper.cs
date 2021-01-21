using System;

namespace DefaultNamespace
{
    public class PlayerHelper
    {
        
    }
    
    public class OnPlatformEnterArgs : EventArgs
    {
        public string ChipName { get; set; }
        public int ChipNumber { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Level.Data.Components
{
    public class CollisionData
    {
        /// <summary>
        /// Assuming every collider has a shape of circle for now
        /// </summary>
        public float Radius { get; set; }
        public bool IsPlayerOwned { get; set; }
    }
}

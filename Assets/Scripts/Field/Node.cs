using Homework.HW3;
using UnityEngine;

namespace Field
{
    public class Node
    {
        public Vector3 Position;
        
        public Node NextNode;
        public bool IsOccupied;
        public OccupationAvailability m_OccupationAvailability;

        public float PathWeight;

        public Node(Vector3 position)
        {
            Position = position;
            m_OccupationAvailability = OccupationAvailability.Undefined;
        }

        public void SetOccupationAvailability(OccupationAvailability status)
        {
            m_OccupationAvailability = status;
        }

        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}
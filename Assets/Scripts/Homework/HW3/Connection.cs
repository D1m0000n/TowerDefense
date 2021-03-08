using UnityEngine;

namespace Homework.HW3
{
    public struct Connection
    {
        public Vector2Int Coordinate;
        public float Weight;

        public Connection(Vector2Int coordinate, float weight)
        {
            Coordinate = coordinate;
            Weight = weight;
        }
    }
}
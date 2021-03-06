﻿using Field;
using UnityEngine;

namespace Homework.HW2
{
    public class Grid
    {
        private Node[,] m_Nodes;
        

        private int m_Width;
        private int m_Height;

        public Grid(int width, int height)
        {
            m_Width = width;
            m_Height = height;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    // m_Nodes[i, j] = new Node();
                }
            }
        }

        public Node GetNode(Vector2Int coonrdinate)
        {
            return GetNode(coonrdinate.x, coonrdinate.y);
        }
        
        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }

            return m_Nodes[i, j];
        }
    }
}
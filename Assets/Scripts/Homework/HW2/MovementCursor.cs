using System;
using UnityEngine;

namespace Homework.HW2
{
    public class MovementCursor : MonoBehaviour
    {
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;

        [SerializeField] private float m_NodeSize;

        [SerializeField] private MovementAgent m_MovementAgent;

        [SerializeField] private GameObject m_Cursor;

        private Camera m_Camera;

        private Vector3 m_Offset;

        private Vector3 m_RightEdge;
        
        private void Awake()
        {
            m_Camera = Camera.main;

            // Default plase size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
            m_RightEdge = transform.position + (new Vector3(width, 0f, height) * 0.5f);
        }

        private void OnValidate()
        {
            m_Camera = Camera.main;

            // Default plase size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
            m_RightEdge = transform.position + (new Vector3(width, 0f, height) * 0.5f);
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    m_Cursor.SetActive(false);
                    return;
                }
                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int y = (int) (difference.z / m_NodeSize);

                m_Cursor.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Right Button");
                    m_Cursor.transform.position = 
                        new Vector3(x * m_NodeSize + m_NodeSize * 0.5f, 0f, y * m_NodeSize + m_NodeSize * 0.5f) + m_Offset;
                    Debug.Log(m_Cursor.transform.position + " " + hit.transform.position);
                    m_MovementAgent.SetTarget(m_Cursor.transform.position);
                }
            }
            else
            {
                m_Cursor.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 0.1f);

            var rightBound = new Vector3(m_Offset.x, m_Offset.y, m_RightEdge.z);
            var leftBound = m_Offset;

            for (int i = 0; i <= m_GridWidth; ++i)
            {
                Gizmos.DrawLine(leftBound, rightBound);
                leftBound.x += m_NodeSize;
                rightBound.x += m_NodeSize;
            }
            
            leftBound = m_Offset;
            rightBound = new Vector3(m_RightEdge.x, m_Offset.y, m_Offset.z);
            for (int i = 0; i <= m_GridHeight; ++i)
            {
                Gizmos.DrawLine(leftBound, rightBound);
                leftBound.z += m_NodeSize;
                rightBound.z += m_NodeSize;
            }
        }
    }
}
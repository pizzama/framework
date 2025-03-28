using System;
using UnityEngine;

namespace SFramework.Game.Map
{
    public class SMGrid
    {
        public const int sortingOrderDefault = 5000;
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private int[,] gridArray;

        public SMGrid(int width, int height, float cellSize, Vector3 originPosition)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new int[width, height];

            bool showDebug = true;
            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = CreateWorldText(
                            gridArray[x, y].ToString(),
                            null,
                            GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f,
                            30,
                            Color.white,
                            TextAnchor.MiddleCenter
                        );
                        Debug.DrawLine(
                            GetWorldPosition(x, y),
                            GetWorldPosition(x, y + 1),
                            Color.white,
                            100f
                        );
                        Debug.DrawLine(
                            GetWorldPosition(x, y),
                            GetWorldPosition(x + 1, y),
                            Color.white,
                            100f
                        );
                    }
                }
                Debug.DrawLine(
                    GetWorldPosition(0, height),
                    GetWorldPosition(width, height),
                    Color.white,
                    100f
                );
                Debug.DrawLine(
                    GetWorldPosition(width, 0),
                    GetWorldPosition(width, height),
                    Color.white,
                    100f
                );

                OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[
                        eventArgs.x,
                        eventArgs.y
                    ]
                        .ToString();
                };
            }
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public float GetCellSize()
        {
            return cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetValue(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                if (OnGridValueChanged != null)
                    OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x,
                y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, value);
        }

        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return 0;
            }
        }

        public int GetValue(Vector3 worldPosition)
        {
            int x,
                y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(
            string text,
            Transform parent = null,
            Vector3 localPosition = default(Vector3),
            int fontSize = 40,
            Color? color = null,
            TextAnchor textAnchor = TextAnchor.UpperLeft,
            TextAlignment textAlignment = TextAlignment.Left,
            int sortingOrder = sortingOrderDefault
        )
        {
            if (color == null)
                color = Color.white;
            return CreateWorldText(
                parent,
                text,
                localPosition,
                fontSize,
                (Color)color,
                textAnchor,
                textAlignment,
                sortingOrder
            );
        }

        // Create Text in the World
        public static TextMesh CreateWorldText(
            Transform parent,
            string text,
            Vector3 localPosition,
            int fontSize,
            Color color,
            TextAnchor textAnchor,
            TextAlignment textAlignment,
            int sortingOrder
        )
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }
}

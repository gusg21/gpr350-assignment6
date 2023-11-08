using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SpatialPartition
{
    public class SPGridGeneric<TItem>
    {
        private Dictionary<Vector2Int, List<TItem>> _boxes = new();
        public Vector2 BoxSize { get; }

        public SPGridGeneric(Vector2 boxSize) => BoxSize = boxSize;
        public SPGridGeneric(float boxWidth, float boxHeight) => BoxSize = new(boxWidth, boxHeight);

        public void UpdateBoxes(IEnumerable<TItem> items, Func<TItem, Vector2> positionFunc)
        {
            // Empty old boxes
            _boxes.Clear();

            foreach (var item in items)
            {
                // Get position
                var itemPosition = positionFunc(item);
                var boxPosition = GetBoxCoords(itemPosition);

                // Add new box if needed
                if (!_boxes.ContainsKey(boxPosition)) _boxes[boxPosition] = new List<TItem>();

                // Add to box
                _boxes[boxPosition].Add(item);
            }
        }

        public List<TItem> GetBox(Vector2Int boxPosition)
        {
            _boxes.TryGetValue(boxPosition, out var value);

            return value ?? new();
        }

        public List<TItem> GetBox(Vector2 worldPosition)
        {
            _boxes.TryGetValue(GetBoxCoords(worldPosition), out var value);

            return value ?? new();
        }

        public Vector2Int GetBoxCoords(Vector2 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x / BoxSize.x),
                Mathf.FloorToInt(worldPosition.y / BoxSize.y)
            );
        }

        public List<TItem> GetNeighbors(Vector2 position)
        {
            Vector2Int[] deltas =
            {
                // Cardinals
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,

                // Corners
                Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right,
                Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right
            };

            List<TItem> neighbors = new();
            
            // Add center
            Vector2Int centerBoxCoords = GetBoxCoords(position);
            neighbors.AddRange(GetBox(centerBoxCoords));

            foreach (var delta in deltas)
                neighbors.AddRange(GetBox(centerBoxCoords + delta));

            return neighbors;
        }

        public void DrawGizmos()
        {
            // Draw any boxes with contents
            foreach (var boxDef in _boxes.Keys)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube((boxDef * BoxSize) + BoxSize / 2f, BoxSize);
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube((boxDef * BoxSize) + BoxSize / 2f, BoxSize * 0.9f);
            }
        }
    }
}
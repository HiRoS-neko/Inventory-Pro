﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Layout/Dynamic Layout Group")]
    public class DynamicLayoutGroup : LayoutGroup
    {
        public enum Corner
        {
            UpperLeft = 0,
            UpperRight = 1,
            LowerLeft = 2,
            LowerRight = 3
        }

        [SerializeField]
        protected Corner m_StartCorner = Corner.UpperLeft;

        [SerializeField]
        protected Vector2 m_CellSize = new(100, 100);

        [SerializeField]
        protected Vector2 m_Spacing = Vector2.zero;

        [SerializeField]
        protected int m_ColumnsCount = 5;

        private ItemCollectionSlotUIBase[] _slots = new ItemCollectionSlotUIBase[0];

        private int cellCountX;
        private int cellCountY;

        protected DynamicLayoutGroup()
        {
        }

        public Corner startCorner
        {
            get => m_StartCorner;
            set => SetProperty(ref m_StartCorner, value);
        }

        public int startAxis => 0;

        public Vector2 cellSize
        {
            get => m_CellSize;
            set => SetProperty(ref m_CellSize, value);
        }

        public Vector2 spacing
        {
            get => m_Spacing;
            set => SetProperty(ref m_Spacing, value);
        }

        public int columnsCount
        {
            get => m_ColumnsCount;
            set => SetProperty(ref m_ColumnsCount, Mathf.Max(1, value));
        }

//        protected override void Awake()
//        {
//            base.Awake();
//        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            var minColumns = 0;
            var preferredColumns = 0;
            minColumns = preferredColumns = m_ColumnsCount;

            SetLayoutInputForAxis(
                padding.horizontal + (cellSize.x + spacing.x) * minColumns - spacing.x,
                padding.horizontal + (cellSize.x + spacing.x) * preferredColumns - spacing.x,
                -1, 0);
        }

        public override void CalculateLayoutInputVertical()
        {
            var width = rectTransform.rect.size.x;
            cellCountX = Mathf.Max(1,
                Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));
            var minRows = Mathf.CeilToInt(rectChildren.Count / (float)m_ColumnsCount - 0.001f);

            var minSpace = padding.vertical + (cellSize.y + spacing.y) * minRows - spacing.y;
            SetLayoutInputForAxis(minSpace, minSpace, -1, 1);
        }

        public override void SetLayoutHorizontal()
        {
//            SetCellsAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        public void SetLayoutVertical(bool forceRebuild)
        {
            SetCellsAlongAxis(1, forceRebuild);
        }

        public void ForceRebuildNow()
        {
//            SetLayoutVertical(true);
            SetDirty();
        }

        private void CacheWrappers()
        {
            var l = new List<ItemCollectionSlotUIBase>(rectChildren.Count);
            foreach (var child in rectChildren)
            {
                var c = child.GetComponent<ItemCollectionSlotUIBase>();
                Assert.IsNotNull(c,
                    "Object without wrapper component in collection container. This isn't allowed. Make sure all objects inside the collection container have a slot component.");
                l.Add(c);
            }

            _slots = l.ToArray();
        }

        private void SetCellsAlongAxis(int axis, bool forceRebuild = false)
        {
            // Normally a Layout Controller should only set horizontal values when invoked for the horizontal axis
            // and only vertical values when invoked for the vertical axis.
            // However, in this case we set both the horizontal and vertical position when invoked for the vertical axis.
            // Since we only set the horizontal position and not the size, it shouldn't affect children's layout,
            // and thus shouldn't break the rule that all horizontal layout must be calculated before all vertical layout.

            var shouldRebuild = false;
            if (_slots.Length != rectChildren.Count)
                CacheWrappers();
            else
                for (var i = 0; i < rectChildren.Count; i++)
                {
                    var c = rectChildren[i].GetComponent<ItemCollectionSlotUIBase>();
                    if (c != null)
                        if (_slots[i].item != c.item)
                        {
                            shouldRebuild = true;
                            break;
                        }
                }

            if (shouldRebuild) CacheWrappers();
            //            else if(forceRebuild == false)
//            {
//                Debug.Log("Nothing changed, don't rebuild.");
//                return;
//            }


            if (axis == 0)
            {
                // Only set the sizes when invoked for horizontal axis, not the positions.
                for (var i = 0; i < rectChildren.Count; i++)
                {
                    var rect = rectChildren[i];

                    m_Tracker.Add(this, rect,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.SizeDelta);

                    rect.anchorMin = Vector2.up;
                    rect.anchorMax = Vector2.up;
                    rect.sizeDelta = cellSize;
                }

                return;
            }

//            float width = rectTransform.rect.size.x;
            var height = rectTransform.rect.size.y;

            cellCountX = columnsCount;
            cellCountY = 1;

//            if (cellSize.x + spacing.x <= 0)
//                cellCountX = int.MaxValue;
//            else
//                cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));

            if (cellSize.y + spacing.y <= 0)
                cellCountY = int.MaxValue;
            else
                cellCountY = Mathf.Max(1,
                    Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));

            var cornerX = (int)startCorner % 2;
            var cornerY = (int)startCorner / 2;

            var cellsPerMainAxis = cellCountX;
            var actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildren.Count);
            var actualCellCountY =
                Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));


            var requiredSpace = new Vector2(
                actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y
            );
            var startOffset = new Vector2(
                GetStartOffset(0, requiredSpace.x),
                GetStartOffset(1, requiredSpace.y)
            );


            for (var i = 0; i < rectChildren.Count; i++)
            {
                var positionX = i % cellsPerMainAxis;
                var positionY = i / cellsPerMainAxis;

                if (cornerX == 1)
                    positionX = actualCellCountX - 1 - positionX;
                if (cornerY == 1)
                    positionY = actualCellCountY - 1 - positionY;


                var child = rectChildren[i];
                uint layoutCols = 1;
                uint layoutRows = 1;
                if (_slots[i].item != null)
                {
                    layoutCols = _slots[i].item.layoutSizeCols;
                    layoutRows = _slots[i].item.layoutSizeRows;
                }

                SetChildAlongAxis(child, 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX,
                    cellSize[0] * layoutCols + spacing[0] * (layoutCols - 1));
                SetChildAlongAxis(child, 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY,
                    cellSize[1] * layoutRows + spacing[1] * (layoutRows - 1));
            }

            CheckBlocks();
        }

        private void CheckBlocks()
        {
            var actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildren.Count);

            // An object in the same column is larger than 1, maybe pushes this one away?
            for (var i = 0; i < rectChildren.Count; i++)
            {
                var item = _slots[i].item;
                if (item == null)
                    continue;

                for (var j = 0; j < item.layoutSizeCols; j++)
                {
                    if (j > 0)
                    {
                    }

                    for (var k = 0; k < item.layoutSizeRows; k++)
                        if (k > 0 || (j > 0 && k == 0))
                        {
                            var indexAdd = j; // 1 to right in grid, index + 1
                            indexAdd += k * actualCellCountX; // Add 1 row worth of indexes (1 down in grid)

                            if (i + indexAdd >= rectChildren.Count) continue;

                            // Can't disable -> Just set size to 0
                            SetChildAlongAxis(rectChildren[i + indexAdd], 0, 0, 0);
                            SetChildAlongAxis(rectChildren[i + indexAdd], 1, 0, 0);
                        }
                }
            }
        }
    }
}
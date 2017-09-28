using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	[Serializable]
	public enum ScalableGridType{
		Both, Width, Height
	}
	
	public class ScalableGridView : MobBehaviour
	{
		public int row, column;
		public RectTransform parent;
		public ScalableGridType scalableType;

		RectTransform _rect;
		GridLayoutGroup _grid;

		void Start(){
			_rect = GetComponent<RectTransform> ();
			_grid = GetComponent<GridLayoutGroup> ();
		}

		void Update(){
			Scale ();
		}

		public void ClearAll(){
			foreach (var item in GetItems()) {
				DestroyImmediate (item.gameObject);
			}
			(_rect ?? (_rect = GetComponent<RectTransform>())).SetHeight(0f);
		}

		public GridItem[] GetItems(){
			return GetComponentsInChildren<GridItem> (false);
		}

		public void Scale(){
			if (GetItems ().Length == 0)
				return;
			var spacing = _grid.spacing;
			switch (scalableType) {
			default:
			case ScalableGridType.Both:
				_grid.cellSize = new Vector2 (parent.rect.width / column - spacing.x, parent.rect.height / row - spacing.y);
				break;
			case ScalableGridType.Width:
				_grid.cellSize.Set (parent.rect.width / column, 0f);
//				_grid.cellSize = new Vector2 (parent.rect.width / column - spacing.x, );
				break;
			case ScalableGridType.Height:
				_grid.cellSize.Set (0f, parent.rect.height / row - spacing.y);
//				_grid.cellSize = new Vector2 (_grid.cellSize.x - spacing.x, parent.rect.height / row - spacing.y);
				break;
			}
			_rect.SetHeight(_grid.preferredHeight);
		}
	}
}


using UnityEngine;
using UnityEditor;

namespace Edit
{
	public class EditorGUISplitView
	{
		public enum Direction { Horizontal, Vertical }

		public Vector2 scrollPosition;
		private Direction splitDirection;
		private float splitNormalizedPosition;
		private bool resize;
		private Rect availableRect;

		private bool enableResize = true;
		private bool showSplitLine = true;
		private Vector2 sizeRange = new Vector2(-1, -1);
		private float size = -1;

		public EditorGUISplitView(Direction splitDirection, bool enableResize = true, float minSize = -1, float maxSize = -1, bool showSplitLine = true)
		{
			splitNormalizedPosition = 0.5f;
			this.splitDirection = splitDirection;
			this.enableResize = enableResize;
			sizeRange = new Vector2(minSize, maxSize);
			this.showSplitLine = showSplitLine;
		}

		public EditorGUISplitView(Direction splitDirection, float fixedSize = -1, bool showSplitLine = true)
		{
			splitNormalizedPosition = 0.5f;
			this.splitDirection = splitDirection;
			enableResize = false;
			if (fixedSize != -1) SetSize(fixedSize);
			this.showSplitLine = showSplitLine;
		}

		public void SetSize(float pixel)
		{
			size = pixel;
		}

		public void SetSplitRatio(float ratio)
		{
			splitNormalizedPosition = ratio;
			if(!enableResize)
			{
				if (splitDirection == Direction.Horizontal)
				{
					size = availableRect.width * splitNormalizedPosition;
					SizeDetect();
				}
				else
				{
					size = availableRect.height * splitNormalizedPosition;
					SizeDetect();
				}
			}
		}

		public void BeginSplitView()
		{
			Rect tempRect;

			if (splitDirection == Direction.Horizontal)
				tempRect = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			else
				tempRect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

			if (tempRect.width > 0.0f)
			{
				availableRect = tempRect;
			}
			if (splitDirection == Direction.Horizontal)
			{
				if(enableResize)
				{
					size = availableRect.width * splitNormalizedPosition;
					SizeDetect();
				}
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(size));
			}
			else
			{
				if (enableResize)
                {
					size = availableRect.height * splitNormalizedPosition;
					SizeDetect();
                }
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(size));
			}
		}

		private void SizeDetect()
        {
			if (sizeRange.x != -1 && size < sizeRange.x) size = sizeRange.x;
			else if (sizeRange.y != -1 && size > sizeRange.y) size = sizeRange.y;
		}

		public void Split()
		{
			GUILayout.EndScrollView();
			ResizeSplitFirstView();
		}

		public void EndSplitView()
		{

			if (splitDirection == Direction.Horizontal)
				EditorGUILayout.EndHorizontal();
			else
				EditorGUILayout.EndVertical();
		}

		private void ResizeSplitFirstView()
		{
			if (!showSplitLine) return;

			Rect resizeHandleRect;

			if (splitDirection == Direction.Horizontal)
			{
				if (enableResize)
				{
					size = availableRect.width * splitNormalizedPosition;
					SizeDetect();
				}
				resizeHandleRect = new Rect(GUILayoutUtility.GetLastRect().x + size, availableRect.y, 2f, availableRect.height);
			}
			else
			{
				if (enableResize)
				{
					size = availableRect.height * splitNormalizedPosition;
					SizeDetect();
				}
				resizeHandleRect = new Rect(availableRect.x, GUILayoutUtility.GetLastRect().y + size, availableRect.width, 2f);
			}

			GUI.DrawTexture(resizeHandleRect, EditorGUIUtility.whiteTexture);

			if (enableResize)
            {
				if (splitDirection == Direction.Horizontal)
					EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);
				else
					EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeVertical);

				if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
				{
					resize = true;
				}
				if (resize)
				{
					if (splitDirection == Direction.Horizontal)
						splitNormalizedPosition = Event.current.mousePosition.x / availableRect.width;
					else
						splitNormalizedPosition = Event.current.mousePosition.y / availableRect.height;
				}
				if (Event.current.type == EventType.MouseUp)
					resize = false;
			}
		}
	}
}
using D3D = SlimDX.Direct3D9;
using System.Collections.Generic;
using System.Windows;

namespace Irelia.Render
{
    public sealed class VectorShape : DisposableObject, ISprite
    {
        private class Line
        {
            public Vector2 Start { get; set; }
            public Vector2 End { get; set; }
            public float Size { get; set; }
            public Color Color { get; set; }
        };

        private readonly D3D.Line d3dLine;
        private readonly IList<Line> lines = new List<Line>();

        public VectorShape(Device device)
        {
            this.d3dLine = new D3D.Line(device.RawDevice);
        }

        public void AddLine(Vector2 start, Vector2 end, float size, Color color)
        {
            Line line = new Line()
            {
                Start = start,
                End = end,
                Size = size,
                Color = color
            };
            
            this.lines.Add(line);
        }

        public void AddPoint(Vector2 point, float size, Color color)
        {
            Line line = new Line()
            {
                Start = point,
                End = new Vector2(point.x + size, point.y),
                Size = size,
                Color = color
            };

            this.lines.Add(line);
        }

        public void AddRectangle(Vector2 leftTop, Vector2 rightBottom, float size, Color color)
        {
            Vector2 rightTop = new Vector2(rightBottom.x, leftTop.y);
            Vector2 leftBottom = new Vector2(leftTop.x, rightBottom.y);

            AddLine(leftTop, rightTop, size, color);
            AddLine(rightTop, rightBottom, size, color);
            AddLine(rightBottom, leftBottom, size, color);
            AddLine(leftBottom, leftTop, size, color);
        }

        public void AddFilledRectangle(Vector2 leftTop, Vector2 rightBottom, Color color)
        {
            Vector2 pt1 = leftTop;
            Vector2 pt2 = new Vector2(rightBottom.x, pt1.y);
            AddLine(pt1, pt2, rightBottom.y - leftTop.y, color);
        }

        bool ISprite.Render(SpriteRenderer renderer)
        {
            foreach (var line in this.lines)
            {
                this.d3dLine.Width = line.Size;

                if (this.d3dLine.Begin().IsFailure)
                    return false;

                var vertexList = new SlimDX.Vector2[2] { line.Start.ToD3DVector2(), line.End.ToD3DVector2() };
                if (this.d3dLine.Draw(vertexList, line.Color.ToD3DColor4()).IsFailure)
                {
                    this.d3dLine.End();
                    return false;
                }

                if (this.d3dLine.End().IsFailure)
                    return false;
            }

            return true;
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                if (this.d3dLine != null)
                    this.d3dLine.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }
        #endregion
    }
}

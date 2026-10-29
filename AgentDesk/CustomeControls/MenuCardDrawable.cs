using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDesk.CustomeControls;

public class MenuCardDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        float w = rect.Width;
        float h = rect.Height;

        canvas.StrokeColor = Color.FromArgb("#35B8DC");
        canvas.StrokeSize = 1.3f;

        var path = new PathF();

        // Top edge with a gap at the top-left
        path.MoveTo(22, 1);
        path.LineTo(w - 9, 1);
        path.QuadTo(w - 1, 1, w - 1, 9);

        // Right edge
        path.LineTo(w - 1, h - 9);
        path.QuadTo(w - 1, h - 1, w - 9, h - 1);

        // Bottom edge
        path.LineTo(9, h - 1);
        path.QuadTo(1, h - 1, 1, h - 9);

        // Left edge with a gap near the top
        path.LineTo(1, 25);

        canvas.DrawPath(path);
    }
}

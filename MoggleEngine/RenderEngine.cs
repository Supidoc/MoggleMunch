using System.Numerics;
using Spectre.Console;

namespace MoggleEngine;

/// <summary>
/// Engine responsible for rasterizing pixels and simple shapes into a Spectre.Console <see cref="Canvas"/>.
/// Provides a singleton instance and camera controls used by game levels and objects to draw to the viewport.
/// </summary>
public class RenderEngine
{
    /// <summary>
    /// Singleton instance of the render engine.
    /// </summary>
    public static RenderEngine Instance = new();

    private Vector2 cameraSize;
    private Color?[,] pixels = new Color?[0, 0];
    private int pixelWidth;

    private RenderEngine()
    {
    }
    
    public int distanceFromEdgeLeft  { get; set; } = 0;
    public int distanceFromEdgeRight { get; set; } = 0;
    public int distanceFromEdgeTop   { get; set; } = 0;
    public int distanceFromEdgeBottom{ get; set; } = 0;

    /// <summary>
    /// Position of the camera (bottom-left) in world coordinates.
    /// </summary>
    public Vector2 CameraPos { get; set; } = new(0, 0);

    /// <summary>
    /// Height of the viewport in pixels.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Width of the viewport in pixels.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The Spectre.Console <see cref="Canvas"/> that is produced by this render engine when rendering.
    /// </summary>
    public Canvas Canvas { get; private set; } = new(1, 1);

    /// <summary>
    /// Initializes the render engine with the given viewport size and pixel scaling.
    /// </summary>
    public void Init(int height, int width, int pixelWidth, Vector2 cameraSize)
    {
        this.Height = height;
        this.Width = width;
        this.pixelWidth = pixelWidth;
        this.cameraSize = cameraSize;

        this.Canvas = new Canvas(width, height);
        this.pixels = new Color?[width, height];
    }

    /// <summary>
    /// Attempts to set a pixel at the specified world position to the provided color if it is inside the camera view.
    /// </summary>
    /// <param name="pos">World position to draw the pixel at.</param>
    /// <param name="color">Spectre.Console color to use for the pixel.</param>
    public void DrawPixel(Vector2 pos, Color color)
    {
        bool inView = true;
        inView = inView && this.CameraPos.X <= pos.X;
        inView = inView && this.CameraPos.Y <= pos.Y;
        inView = inView && (this.CameraPos + this.cameraSize).X >= pos.X;
        inView = inView && (this.CameraPos + this.cameraSize).Y >= pos.Y;
        if (inView)
        {
            Vector2 viewPos = pos - this.CameraPos;
            SetPixel((int)Math.Floor(viewPos.X), (int)Math.Floor(viewPos.Y), color);
        }
    }


    private void SetPixel(int x, int y, Color color)
    {
        if (x >= 0 && x < this.pixels.GetLength(0) && y >= 0 && y < this.pixels.GetLength(1)) this.pixels[x, y] = color;
    }


    /// <summary>
    /// Draws a straight line between two world-space points using a simple raster algorithm.
    /// </summary>
    /// <param name="pos0">Start position.</param>
    /// <param name="pos1">End position.</param>
    /// <param name="color">Color used to draw the line.</param>
    public void DrawLine(Vector2 pos0, Vector2 pos1, Color color)
    {
        float x0 = pos0.X;
        float x1 = pos1.X;
        float y0 = pos0.Y;
        float y1 = pos1.Y;
        float dx = x1 - x0;
        float dy = y1 - y0;


        if (dx > dy)
        {
            float d = 2 * dy - dx;

            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            float y = y0;

            for (float x = x0; x < x1; x++)
            {
                DrawPixel(new Vector2(x, y), color);
                if (d > 0)
                {
                    y += yi;
                    d -= 2 * dx;
                }

                d += 2 * dy;
            }
        }
        else
        {
            float d = 2 * dx - dy;

            int xi = 1;
            if (dy < 0)
            {
                xi = -1;
                dx = -dx;
            }

            float x = x0;
            for (float y = y0; y < y1; y++)
            {
                DrawPixel(new Vector2(x, y), color);
                if (d > 0)
                {
                    x += xi;
                    d -= 2 * (dx - dy);
                }

                d += 2 * dx;
            }
        }
    }


    /// <summary>
    /// Draws a circle outline using the midpoint circle algorithm at the given world-space center.
    /// </summary>
    /// <param name="pos">Center of the circle.</param>
    /// <param name="outerRadius">Outer radius in world units.</param>
    /// <param name="color">Color used for the circle pixels.</param>
    /// <param name="innerRadius">Optional inner radius for ring shapes (not currently used by the algorithm here).</param>
    /// <remarks>Copied from www.geeksforgeeks.org/dsa/mid-point-circle-drawing-algorithm/</remarks>
    public void DrawCircle(Vector2 pos, float outerRadius, Color color, float innerRadius = 0)
    {
        int xCentre = (int)Math.Round(pos.X);
        int yCentre = (int)Math.Round(pos.Y);
        int r = (int)Math.Round(outerRadius);

        int x = r;
        int y = 0;

        // Printing the initial point on the
        // axes after translation
        DrawPixel(new Vector2(x + xCentre, yCentre), color);

        // When radius is zero only a single
        // point will be printed
        if (r > 0)
        {
            DrawPixel(new Vector2(-x + xCentre, -y + yCentre), color);
            DrawPixel(new Vector2(y + xCentre, x + yCentre), color);
            DrawPixel(new Vector2(-y + xCentre, -x + yCentre), color);
        }

        // Initialising the value of P
        int P = 1 - r;
        while (x > y)
        {
            y++;

            // Mid-point is inside or on the perimeter
            if (P <= 0)
            {
                P = P + 2 * y + 1;
            }

            // Mid-point is outside the perimeter
            else
            {
                x--;
                P = P + 2 * y - 2 * x + 1;
            }

            // All the perimeter points have already 
            // been printed
            if (x < y)
                break;

            // Printing the generated point and its 
            // reflection in the other octants after
            // translation
            DrawPixel(new Vector2(x + xCentre, y + yCentre), color);
            DrawPixel(new Vector2(-x + xCentre, y + yCentre), color);
            DrawPixel(new Vector2(x + xCentre, -y + yCentre), color);
            DrawPixel(new Vector2(-x + xCentre, -y + yCentre), color);

            // If the generated point is on the 
            // line x = y then the perimeter points
            // have already been printed
            if (x != y)
            {
                DrawPixel(new Vector2(y + xCentre, x + yCentre), color);
                DrawPixel(new Vector2(-y + xCentre, x + yCentre), color);
                DrawPixel(new Vector2(y + xCentre, -x + yCentre), color);
                DrawPixel(new Vector2(-y + xCentre, -x + yCentre), color);
            }
        }
    }


    /// <summary>
    /// Transfers the internal pixel buffer to the <see cref="Canvas"/> instance and resets the buffer for the next frame.
    /// </summary>
    public void RenderCanvas()
    {
        // Compute desired canvas width from console first
        int w = Console.WindowWidth;
        int h = Console.WindowHeight;
        int widthOffset = this.distanceFromEdgeLeft + this.distanceFromEdgeRight;
        int heightOffset = this.distanceFromEdgeTop + this.distanceFromEdgeBottom;
        int desiredWidth = Math.Max(1, (w - widthOffset) / this.pixelWidth);
        int desiredHeight = Math.Max(1, (h - heightOffset));

        // If width changed, resize buffer before rendering (preserve existing pixels where possible)
        if (desiredWidth != this.Width || desiredHeight != this.Height)
        {
            var newPixels = new Color?[desiredWidth, desiredHeight];
            int copyW = Math.Min(desiredWidth, this.pixels.GetLength(0));
            int copyH = Math.Min(desiredHeight, this.pixels.GetLength(1));
            for (int i = 0; i < copyW; i++)
            for (int j = 0; j < copyH; j++)
                newPixels[i, j] = this.pixels[i, j];

            this.pixels = newPixels;
            this.Width = desiredWidth;
            this.Height = desiredHeight;
            this.cameraSize = new Vector2(this.Width, this.Height);
        }

        // Build canvas using the stable current dimensions
        this.Canvas = new Canvas(this.Width, this.Height);
        this.Canvas.PixelWidth = this.pixelWidth;

        // Copy pixel buffer to canvas (render with 0,0 at bottom-left)
        for (int i = 0; i < this.pixels.GetLength(0); i++)
        {
            for (int j = 0; j < this.pixels.GetLength(1); j++)
            {
                if (this.pixels[i, j].HasValue)
                    this.Canvas.SetPixel(i, this.Height - j - 1, this.pixels[i, j]!.Value);
            }
        }

        // Clear buffer for next frame without reallocating (prevents tearing/flicker)
        for (int i = 0; i < this.pixels.GetLength(0); i++)
        for (int j = 0; j < this.pixels.GetLength(1); j++)
            this.pixels[i, j] = null;
    }

}
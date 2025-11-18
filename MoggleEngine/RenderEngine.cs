using System.Numerics;
using Spectre.Console;

namespace MoggleEngine;

public class RenderEngine
{
    public static RenderEngine Instance = new RenderEngine();
    
    private Vector2 cameraSize;
    private int pixelWidth;
    private Color?[,] pixels = new Color?[0,0];

    private RenderEngine()
    {
    }
    
    public void Init(int height, int width, int pixelWidth, Vector2 cameraSize)
    {
        this.Height = height;
        this.Width = width;
        this.pixelWidth = pixelWidth;
        this.cameraSize = cameraSize;

        this.Canvas = new Canvas(width*pixelWidth, height);
        this.pixels = new Color?[width, height];
    }

    public Vector2 CameraPos { get; set; } = new(0, 0);

    public int Height { get; set; }

    public int Width { get; set; }

    public Canvas Canvas { get; private set; } = new Canvas(1, 1);

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
        if (x >= 0 && x < this.pixels.GetLength(0) && y >= 0 && y < this.pixels.GetLength(1))
        {
            this.pixels[x, y] = color;
        }
    }


    public void DrawCircle(Vector2 pos, float radius, Color color, int innerRadius = 0)
    {
        float posX = pos.X ;
        float posY = pos.Y;

        float minPosX = posX - radius + 0.5f;
        float maxPosX = posX + radius + 0.5f;

        float minPosY = posY - radius+0.5f;
        float maxPosY = posY + radius+0.5f;

        for (float i = minPosX; i <= maxPosX; i++)
        for (float j = minPosY; j <= maxPosY; j++)
            if (Math.Pow(i - posX, 2) + Math.Pow(j - posY, 2) <= Math.Pow(radius + 0.001, 2) &&
                Math.Pow(i - posX, 2) + Math.Pow(j - posY, 2) >= Math.Pow(innerRadius + 0.001, 2))
                DrawPixel(new Vector2(i, j), color);
    }

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

    public void PreRender()
    {
        this.pixels = new Color?[this.Width, this.Height];
    }

    public void RenderCanvas()
    {
        this.Canvas = new Canvas(this.Width, this.Height);
        this.Canvas.PixelWidth = this.pixelWidth;
        for (int i = 0; i < this.pixels.GetLength(0); i++)
        for (int j = 0; j < this.pixels.GetLength(1) ; j++)
            if (this.pixels[i, j].HasValue) 
                this.Canvas.SetPixel(i, this.Height- j -1, this.pixels[i, j]!.Value);
    }
}
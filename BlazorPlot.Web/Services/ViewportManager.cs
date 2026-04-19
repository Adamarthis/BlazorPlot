namespace BlazorPlot.Web.Services
{
    public class ViewportManager
    {
        public double CanvasWidth { get; set; } = 800;
        public double CanvasHeight { get; set; } = 600;

        public double CenterX { get; set; } = 0;
        public double CenterY { get; set; } = 0;

        public double UnitsPerPixel { get; set; } = 0.05;

        public (double px, double py) MathToScreen(double mathX, double mathY)
        {
            double dx = mathX - CenterX;
            double dy = mathY - CenterY;

            double px = (CanvasWidth / 2) + (dx / UnitsPerPixel);
            double py = (CanvasHeight / 2) - (dy / UnitsPerPixel);

            return (px, py);
        }

        public (double mathX, double mathY) ScreenToMath(double px, double py)
        {
            double dx = px - (CanvasWidth / 2);
            double dy = (CanvasWidth / 2) - py;

            double mathX = CenterX + (dx * UnitsPerPixel);
            double mathY = CenterY + (dy * UnitsPerPixel);
            
            return (mathX, mathY);
        }

        public (double XMin, double XMax, double YMin, double YMax) GetVisibleMathBounds()
        {
            var topLeft = ScreenToMath(0, 0);
            var bottomRight = ScreenToMath(CanvasWidth, CanvasHeight);

            return (topLeft.mathX, bottomRight.mathX, bottomRight.mathY, topLeft.mathY);
        }
    }
}

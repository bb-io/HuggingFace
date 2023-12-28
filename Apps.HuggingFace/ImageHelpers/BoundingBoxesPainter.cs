using System.Drawing;
using System.Drawing.Imaging;
using Apps.HuggingFace.Dtos;

namespace Apps.HuggingFace.ImageHelpers;

public static class BoundingBoxesPainter
{
    public static byte[] DrawBoundingBoxes(this byte[] imageBytes, string contentType, 
        IEnumerable<DetectedObjectDto> detectedObjects, bool drawLabels)
    {
        var image = new Bitmap(new MemoryStream(imageBytes));
        
        using (var graphics = Graphics.FromImage(image))
        {
            foreach (var detectedObject in detectedObjects)
            {
                var boxThickness = (int)Math.Ceiling((image.Height + image.Width) * 0.0015);
                var randomColor = GetRandomColor();

                using var pen = new Pen(randomColor, boxThickness);
                
                var rectangle = new Rectangle(detectedObject.Box.Xmin, detectedObject.Box.Ymin, 
                    detectedObject.Box.Xmax - detectedObject.Box.Xmin, detectedObject.Box.Ymax - detectedObject.Box.Ymin);
                graphics.DrawRectangle(pen, rectangle);

                if (drawLabels)
                {
                    var fontSize = (image.Height + image.Width) * 0.0015f;
                    var font = new Font(SystemFonts.DialogFont.FontFamily, fontSize);
                    var label = $"{detectedObject.Label} ({detectedObject.Score:F2})";
                    graphics.DrawString(label, font, new SolidBrush(randomColor), rectangle.Location);
                }
            }
        }
        
        using (var memoryStream = new MemoryStream())
        {
            var imageFormat = GetImageFormatFromMimeType(contentType);
            image.Save(memoryStream, imageFormat);
            return memoryStream.ToArray();
        }
    }
    
    private static Color GetRandomColor()
    {
        var random = new Random();
        var colors = new List<Color>
        {
            Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple, 
            Color.Yellow, Color.Cyan, Color.Magenta, Color.Pink, Color.Chartreuse
        };

        return colors[random.Next(colors.Count)];
    }

    private static ImageFormat GetImageFormatFromMimeType(string mimeType)
    {
        switch (mimeType.ToLower())
        {
            case "image/jpeg":
            case "image/jpg":
                return ImageFormat.Jpeg;
            case "image/png":
                return ImageFormat.Png;
            case "image/gif":
                return ImageFormat.Gif;
            case "image/bmp":
                return ImageFormat.Bmp;
            case "image/tiff":
            case "image/tif":
                return ImageFormat.Tiff;
            case "image/x-icon":
                return ImageFormat.Icon;
            case "image/webp":
                return ImageFormat.Webp;
            default:
                throw new NotSupportedException("Unsupported image MIME type.");
        }
    }
}
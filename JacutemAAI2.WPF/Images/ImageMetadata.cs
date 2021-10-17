namespace JacutemAAI2.WPF.Images
{
    public class ImageMetadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string BppString { get; set; }
        public int ColorCount { get; set; }

        public ImageMetadata(int width, int height, string bppString, int colorCount)
        {
            Width = width;
            Height = height;
            BppString = bppString;
            ColorCount = colorCount;
        }
    }
}

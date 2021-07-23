using System.Drawing;

namespace ImageLibGbaDS.Paleta
{
    public interface IPaleta
    {
        bool TemAlpha { get; set; }
        Color[] Cores { get; set; }
    }
}

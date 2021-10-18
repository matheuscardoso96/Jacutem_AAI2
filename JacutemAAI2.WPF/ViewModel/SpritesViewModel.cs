using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JacutemAAI2.WPF.Images.ImagesMapPath;

namespace JacutemAAI2.WPF.ViewModel
{
    public class SpritesViewModel : ImageViewModelBase
    {
        public SpritesViewModel():base(new SpritesPaths())
        {

        }
    }
}

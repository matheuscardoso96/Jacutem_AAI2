using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jacutem_AAI2.Custom
{


    public class PictureBoxCustom : PictureBox
    {
        private Color _borderColor;
        private float _borderWidth;
        [Browsable(true)]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; this.Invalidate(); }
        }
        [Browsable(true)]
        public float BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; this.Invalidate(); }
        }

        public PictureBoxCustom(bool temBorda)
        {
            if (temBorda)
            {
                _borderColor = Color.Red;
                _borderWidth = 1;
            }
            

        }

        public void AtivaBorda()
        {
            _borderColor = Color.Red;
            _borderWidth = 0.1f;
        }

        public void Desativa()
        {
            _borderColor = Color.Transparent;
            _borderWidth = 0.1f;
        }

       protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
           pe.Graphics.DrawRectangle(new Pen(BorderColor, BorderWidth), 0, 0, this.Size.Width -1 , this.Size.Height - 1);
            pe.Dispose();
        }

       

    }
}

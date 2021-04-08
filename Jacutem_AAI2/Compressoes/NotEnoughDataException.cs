using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacutem_AAI2.Compressoes
{
    public class NotEnoughDataException : IOException
    {
        private long currentOutSize;
        private long totalOutSize;
        /// <summary>
        /// Gets the actual number of written bytes.
        /// </summary>
        public long WrittenLength { get { return this.currentOutSize; } }
        /// <summary>
        /// Gets the number of bytes that was supposed to be written.
        /// </summary>
        public long DesiredLength { get { return this.totalOutSize; } }

        /// <summary>
        /// Creates a new NotEnoughDataException.
        /// </summary>
        /// <param name="currentOutSize">The actual number of written bytes.</param>
        /// <param name="totalOutSize">The desired number of written bytes.</param>
        public NotEnoughDataException(long currentOutSize, long totalOutSize)
            : base("Not enough data availble; 0x" + currentOutSize.ToString("X")
                + " of " + (totalOutSize < 0 ? "???" : ("0x" + totalOutSize.ToString("X")))
                + " bytes written.")
        {
            this.currentOutSize = currentOutSize;
            this.totalOutSize = totalOutSize;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacutem_AAI2.Compressoes
{
    public class StreamTooShortException : EndOfStreamException
    {
        /// <summary>
        /// Creates a new exception that indicates that the stream was shorter than the given input length.
        /// </summary>
        public StreamTooShortException()
            : base("The end of the stream was reached "
                 + "before the given amout of data was read.")
        { }
    }
}

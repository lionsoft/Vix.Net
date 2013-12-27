using System;

namespace Vestris.VMWareLib
{
    /// <summary>
    /// A VMWare exception. Every VMWare operational failure is translated into 
    /// a <see ref="Vestris.VMWareLib.VMWareException" />.
    /// </summary>
    public class VMWareException : Exception
    {
        /// <summary>
        /// The original VMWare error code.
        /// </summary>
        public ulong ErrorCode { get; private set; }

        /// <summary>
        /// A VMWare exception with default error text in English-US.
        /// </summary>
        /// <param name="code">VMWare VixCOM.Constants error code</param>
        public VMWareException(ulong code) : this(code, VMWareInterop.Instance.GetErrorText(code, "en-US"))
        {
        }

        /// <summary>
        /// A VMWare exception.
        /// </summary>
        /// <param name="code">VMWare VixCOM.Constants error code</param>
        /// <param name="message">error description</param>
        public VMWareException(ulong code, string message) : base(message)
        {
            ErrorCode = code;
        }
    }
}

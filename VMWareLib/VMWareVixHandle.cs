using VixCOM;

namespace Vestris.VMWareLib
{
    /// <summary>
    /// A wrapper for a VixCOM handle. 
    /// </summary>
    /// <remarks>
    /// Most VixCOM objects returned from VixCOM API functions implement IVixHandle.
    /// </remarks>
    public class VMWareVixHandle<T>
    {
        /// <summary>
        /// Raw VixCOM handle of implemented type.
        /// </summary>
        protected T Handle { get; set; }
        
        /// <summary>
        /// Pointer to the IVixHandle interface.
        /// </summary>
        protected IVixHandle VixHandle { get { return (IVixHandle) Handle; } }

        /// <summary>
        /// A constructor for a null Vix handle.
        /// </summary>
        public VMWareVixHandle()
        {

        }

        /// <summary>
        /// A constructor for an existing Vix handle.
        /// </summary>
        /// <param name="handle">handle value</param>
        public VMWareVixHandle(T handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Get an array of properties.
        /// </summary>
        /// <param name="properties">properties to fetch</param>
        /// <returns>An array of property values.</returns>
        public object[] GetProperties(object[] properties)
        {
            object result = null;
            VMWareInterop.Check(VixHandle.GetProperties(properties, ref result));
            return (object[]) result;
        }

        /// <summary>
        /// Return the value of a single property.
        /// </summary>
        /// <param name="propertyId">property id</param>
        /// <typeparam name="TProperty">property value type</typeparam>
        /// <returns>The value of a single property of type R.</returns>
        public TProperty GetProperty<TProperty>(int propertyId)
        {
            object[] properties = { propertyId };
            return (TProperty) GetProperties(properties)[0];
        }
    }
}

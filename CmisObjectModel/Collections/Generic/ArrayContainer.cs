
namespace CmisObjectModel.Collections.Generic
{
    /// <summary>
   /// Encapsulates an array and allows access to it via a DynamicProperty
   /// </summary>
   /// <remarks></remarks>
    public class ArrayContainer<T> : Common.Generic.DynamicProperty<T[]>
    {

        public ArrayContainer(string propertyName, params T[] values) : base(propertyName)
        {
            _values = values;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        protected T[] _values;
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override T[] Value
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }
        public T[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

    }
}
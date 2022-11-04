using System.Data;
using System.Linq;
using sxs = System.Xml.Serialization;
using ccg = CmisObjectModel.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Core.Choices
{
    [sxs.XmlRoot("choice", Namespace = Constants.Namespaces.cmis)]
    public partial class cmisChoice
    {

        protected cmisChoice(string displayName)
        {
            _displayName = displayName;
        }

        public cmisChoice[] Choices
        {
            get
            {
                return ChoicesCore;
            }
            set
            {
                ChoicesCore = value;
            }
        }
        protected static cmisChoice[] ChoicesCore { get; set; }

        private ccg.ArrayMapper<cmisChoice, cmisChoice> initial_choicesAsReadOnly() => new ccg.ArrayMapper<cmisChoice, cmisChoice>(this, "Choices", () => ChoicesCore, "DisplayName", choice => choice.DisplayName);

        private ccg.ArrayMapper<cmisChoice, cmisChoice> _choicesAsReadOnly;
        /// <summary>
      /// Access to choices via index or DisplayName
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccg.ArrayMapper<cmisChoice, cmisChoice> ChoicesAsReadOnly
        {
            get
            {
                return _choicesAsReadOnly;
            }
        }

        /// <summary>
      /// Create new child
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public abstract cmisChoice CreateChild();

        public object[] Values
        {
            get
            {
                return ValuesCore;
            }
            set
            {
                ValuesCore = value;
            }
        }
        protected abstract object[] ValuesCore { get; set; }

    }

    namespace Generic
    {
        /// <summary>
      /// Generic Version of cmisChoice
      /// </summary>
      /// <typeparam name="TProperty"></typeparam>
      /// <typeparam name="TDerived"></typeparam>
      /// <remarks>Baseclass for all typesafe cmisChoice-classes</remarks>
        [Attributes.JavaScriptConverter(typeof(JSON.Core.Choices.Generic.cmisChoiceConverter<bool, cmisChoiceBoolean>), "{\"TProperty\":\"TProperty\",\"TDerived\":\"TChoice\"}")]
        public abstract class cmisChoice<TProperty, TDerived> : cmisChoice where TDerived : cmisChoice<TProperty, TDerived>, new()
        {

            protected cmisChoice() : base()
            {

                _choicesAsReadOnly = new ccg.ArrayMapper<cmisChoice, TDerived>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
            }
            /// <summary>
         /// this constructor is only used if derived classes from this class needs an InitClass()-call
         /// </summary>
         /// <param name="initClassSupported"></param>
         /// <remarks></remarks>
            protected cmisChoice(bool? initClassSupported) : base(initClassSupported)
            {
                _choicesAsReadOnly = new ccg.ArrayMapper<cmisChoice, TDerived>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
            }
            protected cmisChoice(string displayName, params TDerived[] choices) : base(displayName)
            {
                _choicesAsReadOnly = new ccg.ArrayMapper<cmisChoice, TDerived>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
            }
            protected cmisChoice(string displayName, TProperty[] values, params TDerived[] choices) : base(displayName)
            {
                _choicesAsReadOnly = new ccg.ArrayMapper<cmisChoice, TDerived>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
            }

            protected TDerived[] _choices;
            public virtual new TDerived[] Choices
            {
                get
                {
                    return _choices;
                }
                set
                {
                    if (!ReferenceEquals(value, _choices))
                    {
                        var oldValue = _choices;
                        _choices = value;
                        OnPropertyChanged("Choices", value, oldValue);
                    }
                }
            } // Choices
            protected virtual cmisChoice[] ChoicesCore
            {
                get
                {
                    if (_choices is null)
                    {
                        return null;
                    }
                    else
                    {
                        return (from choice in _choices
                                select choice).Cast<cmisChoice>().ToArray();
                    }
                }
                set
                {
                    if (value is null)
                    {
                        Choices = null;
                    }
                    else
                    {
                        Choices = (from choice in value
                                   where choice is TDerived
                                   select ((TDerived)choice)).ToArray();
                    }
                }
            }

            private ccg.ArrayMapper<cmisChoice, TDerived> _choicesAsReadOnly;
            /// <summary>
         /// Access to choices via index or DisplayName
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            public new ccg.ArrayMapper<cmisChoice, TDerived> ChoicesAsReadOnly
            {
                get
                {
                    return _choicesAsReadOnly;
                }
            }

            public override cmisChoice CreateChild()
            {
                return new TDerived();
            }

            protected TProperty[] _values;
            public virtual new TProperty[] Values
            {
                get
                {
                    return _values;
                }
                set
                {
                    if (!ReferenceEquals(value, _values))
                    {
                        var oldValue = _values;
                        _values = value;
                        OnPropertyChanged("Values", value, oldValue);
                    }
                }
            } // Values
            protected override object[] ValuesCore
            {
                get
                {
                    if (_values is null)
                    {
                        return null;
                    }
                    else
                    {
                        return (from item in _values
                                select item).Cast<TProperty[]>().ToArray();
                    }
                }
                set
                {
                    if (value is null)
                    {
                        Values = null;
                    }
                    else
                    {
                        Values = (from item in value
                                  where item is TProperty
                                  select (Conversions.ToGenericParameter<TProperty>(item))).ToArray();
                    }
                }
            }

        }
    }
}
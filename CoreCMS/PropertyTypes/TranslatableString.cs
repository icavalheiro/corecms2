using System.Globalization;

namespace CoreCMS
{
    /// <summary>
    /// Intends to be an easy dropin for translatable strings.
    /// </summary>
    public class TranslatableString : TranslatableProperty<string>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TranslatableString() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="s">Value to be used to initialyze it with.</param>
        public TranslatableString(string s) : base(s) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="culture">Which is the default culture.</param>
        /// <param name="value">Value to be used to initialyze it with.</param>
        public TranslatableString(CultureInfo culture, string value) : base(culture, value) { }

        #region Operators
        /// <summary>
        /// Defines an implicit operator that is expected to be used if the application does not have
        /// multilanguage support.
        /// </summary>
        /// <param name="ts">The other object</param>
        public static implicit operator string(TranslatableString ts)
        {
            return ts.GetDefaultValue();
        }

        /// <summary>
        /// Defines an implicit operator that is expected to be used if the application does not have
        /// multilanguage support.
        /// </summary>
        /// <param name="s">The other object</param>
        public static implicit operator TranslatableString(string s)
        {
            return new TranslatableString(s);
        }
        #endregion
    }
}

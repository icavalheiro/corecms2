using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CoreCMS
{
    /// <summary>
    /// This class may be used in the place of a property
    /// it's intention is to hold multiple values depending on the 
    /// culture that is needed, so it can have multiple translations.
    /// </summary>
    /// <typeparam name="T">Type of which it will be, try using "string" or "int" or anything that you may want to have different values depending on Culture</typeparam>
    [Serializable]
    public class TranslatableProperty<T>
    {
        public static CultureInfo DefaultCulture = new CultureInfo("en-US");
        public Dictionary<CultureInfo, T> Values { get; private set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        public TranslatableProperty()
        {
            Values = new Dictionary<CultureInfo, T>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value to be used to initialyze it with.</param>
        public TranslatableProperty(T value)
        {
            Values = new Dictionary<CultureInfo, T>();
            SetTranslationFor(DefaultCulture, value);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="culture">Which is the default culture.</param>
        /// <param name="value">Value to be used to initialyze it with.</param>
        public TranslatableProperty(CultureInfo culture, T value)
        {
            Values = new Dictionary<CultureInfo, T>();
            SetTranslationFor(culture, value);
        }

        /// <summary>
        /// Set a new value for the given culture.
        /// </summary>
        /// <param name="culture">Culture to have its value set.</param>
        /// <param name="translation">Value to be set.</param>
        public void SetTranslationFor(CultureInfo culture, T translation)
        {
            if (ContainsTranslationFor(culture))
            {
                Values[culture] = translation;
            }
            else
            {
                Values.Add(culture, translation);
            }
        }

        /// <summary>
        /// Get the default value of that instance based on it's default culture.
        /// </summary>
        /// <returns>The default value.</returns>
        public T GetDefaultValue()
        {
            if (ContainsTranslationFor(DefaultCulture))
            {
                return GetTranslationFor(DefaultCulture);
            }

            return Values.Values.FirstOrDefault();
        }

        /// <summary>
        /// Check if this object has a translation for the given culture.
        /// </summary>
        /// <param name="culture">Culture to check for the translation.</param>
        /// <returns>True if this object instance has a translation.</returns>
        public bool ContainsTranslationFor(CultureInfo culture)
        {
            return Values.ContainsKey(culture);
        }

        /// <summary>
        /// Check fi this object has a translation for the given culture.
        /// </summary>
        /// <param name="culture">Culture to check for the translation.</param>
        /// <returns>True if this object instance has a translation.</returns>
        public T GetTranslationFor(CultureInfo culture)
        {
            if (!Values.ContainsKey(culture))
            {
                return GetDefaultValue();
            }

            return Values[culture];
        }

        /// <summary>
        /// Check fi this object has a translation for the given culture.
        /// </summary>
        /// <param name="culture">Culture to check for the translation.</param>
        /// <returns>True if this object instance has a translation.</returns>
        public T GetTranslationFor(string culture)
        {
            return GetTranslationFor(new CultureInfo(culture));
        }


    }
}


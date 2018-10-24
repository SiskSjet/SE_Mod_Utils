using System.Collections.Generic;
using System.Linq;
using VRage;

namespace Sisk.Utils.Localization {
    public class Lang {
        private static readonly Dictionary<MyLanguagesEnum, IDictionary<string, string>> Map = new Dictionary<MyLanguagesEnum, IDictionary<string, string>> {
            { MyLanguagesEnum.English, English }
        };

        private static IDictionary<string, string> English => new Dictionary<string, string>();

        /// <summary>
        ///     Add a new language dictionary.
        /// </summary>
        /// <param name="language">The language of the dictionary.</param>
        /// <param name="dictionary">The dictionary containing the translations.</param>
        public static void Add(MyLanguagesEnum language, IDictionary<string, string> dictionary) {
            if (!Map.ContainsKey(language)) {
                Map.Add(language, dictionary);
            } else {
                Map[language] = dictionary;
            }
        }

        /// <summary>
        ///     Check if dictionary for given language exists.
        /// </summary>
        /// <param name="language">The language key.</param>
        /// <returns>Return true if dictionary exists and is not empty.</returns>
        public static bool Contains(MyLanguagesEnum language) {
            return Map.ContainsKey(language) && Map[language].Any();
        }

        /// <summary>
        ///     Return the dictionary for given language.
        /// </summary>
        /// <param name="language">The language key.</param>
        /// <returns>Return a dictionary with translation for given language.</returns>
        public static IDictionary<string, string> Get(MyLanguagesEnum language) {
            return Map[language];
        }
    }
}
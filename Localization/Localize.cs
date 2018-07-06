using System;
using System.Collections.Generic;
using VRage;
using VRage.Utils;

namespace Sisk.Utils.Localization {
    /// <summary>
    /// A static class used to localize strings.
    /// </summary>
    public static class Localize {
        private static readonly Dictionary<string, LocalizationItem> Localization = new Dictionary<string, LocalizationItem>();

        // ReSharper disable InconsistentNaming
        /// <summary>
        ///     Creates a new localization entry.
        /// </summary>
        /// <param name="id">The id for this localization.</param>
        /// <param name="English">The english translation.</param>
        /// <param name="Czech">The czech translation.</param>
        /// <param name="Slovak">The slovak translation.</param>
        /// <param name="German">The german translation.</param>
        /// <param name="Russian">The russian translation.</param>
        /// <param name="Spanish_Spain">The spanish (Spain) translation.</param>
        /// <param name="French">The french translation.</param>
        /// <param name="Italian">The italian translation.</param>
        /// <param name="Danish">The danish translation.</param>
        /// <param name="Dutch">The dutch translation.</param>
        /// <param name="Icelandic">The icelandic translation.</param>
        /// <param name="Polish">The polish translation.</param>
        /// <param name="Finnish">The finnish translation.</param>
        /// <param name="Hungarian">The hungarian translation.</param>
        /// <param name="Portuguese_Brazil">The portuguese translation.</param>
        /// <param name="Estonian">The estonian translation.</param>
        /// <param name="Norwegian">The norwegian translation.</param>
        /// <param name="Spanish_HispanicAmerica">The spanish (HispanicAmerica) translation.</param>
        /// <param name="Swedish">The swedish translation.</param>
        /// <param name="Catalan">The catalan translation.</param>
        /// <param name="Croatian">The croatian translation.</param>
        /// <param name="Romanian">The romanian translation.</param>
        /// <param name="Ukrainian">The ukrainian translation.</param>
        /// <param name="Turkish">The turkish translation.</param>
        /// <param name="Latvian">The latvian translation.</param>
        /// <param name="ChineseChina">The chinese translation.</param>
        public static void Create(string id, string English, string Czech = null, string Slovak = null, string German = null, string Russian = null, string Spanish_Spain = null, string French = null, string Italian = null, string Danish = null, string Dutch = null, string Icelandic = null, string Polish = null, string Finnish = null, string Hungarian = null, string Portuguese_Brazil = null, string Estonian = null, string Norwegian = null, string Spanish_HispanicAmerica = null, string Swedish = null, string Catalan = null, string Croatian = null, string Romanian = null, string Ukrainian = null, string Turkish = null, string Latvian = null, string ChineseChina = null) {
            if (string.IsNullOrEmpty(id)) {
                throw new ArgumentException("Null or whitespace are not allowed.", nameof(id));
            }

            if (string.IsNullOrEmpty(English)) {
                throw new ArgumentException("Null or whitespace are not allowed.", nameof(English));
            }

            var translations = new Dictionary<MyLanguagesEnum, string> {
                {MyLanguagesEnum.English, English},
                {MyLanguagesEnum.Czech, Czech},
                {MyLanguagesEnum.Slovak, Slovak},
                {MyLanguagesEnum.German, German},
                {MyLanguagesEnum.Russian, Russian},
                {MyLanguagesEnum.Spanish_Spain, Spanish_Spain},
                {MyLanguagesEnum.French, French},
                {MyLanguagesEnum.Italian, Italian},
                {MyLanguagesEnum.Danish, Danish},
                {MyLanguagesEnum.Dutch, Dutch},
                {MyLanguagesEnum.Icelandic, Icelandic},
                {MyLanguagesEnum.Polish, Polish},
                {MyLanguagesEnum.Finnish, Finnish},
                {MyLanguagesEnum.Hungarian, Hungarian},
                {MyLanguagesEnum.Portuguese_Brazil, Portuguese_Brazil},
                {MyLanguagesEnum.Estonian, Estonian},
                {MyLanguagesEnum.Norwegian, Norwegian},
                {MyLanguagesEnum.Spanish_HispanicAmerica, Spanish_HispanicAmerica},
                {MyLanguagesEnum.Swedish, Swedish},
                {MyLanguagesEnum.Catalan, Catalan},
                {MyLanguagesEnum.Croatian, Croatian},
                {MyLanguagesEnum.Romanian, Romanian},
                {MyLanguagesEnum.Ukrainian, Ukrainian},
                {MyLanguagesEnum.Turkish, Turkish},
                {MyLanguagesEnum.Latvian, Latvian},
                {MyLanguagesEnum.ChineseChina, ChineseChina}
            };
            var localizationItem = new LocalizationItem(translations);
            Localization.Add(id, localizationItem);
        }
        // ReSharper restore InconsistentNaming

        /// <summary>
        ///     Get the localized <see cref="MyStringId" /> for given <paramref name="stringId" />.
        /// </summary>
        /// <param name="stringId">The stringId that identifies the localized <see cref="MyStringId" />.</param>
        /// <returns>Returns the localized <see cref="MyStringId" />.</returns>
        public static MyStringId Get(string stringId) {
            return Localization.ContainsKey(stringId)
                ? MyStringId.GetOrCompute(Localization[stringId])
                : MyStringId.Get(stringId);
        }

        /// <summary>
        ///     Gets the localized string for given <paramref name="stringId" /> and format the result if <paramref name="args" />
        ///     are set.
        /// </summary>
        /// <param name="stringId">The stringId that identifies the localized string</param>
        /// <param name="args">The arguments used by <see cref="string.Format(string,object)" />.</param>
        /// <returns>Returns the localized and formated string.</returns>
        public static string GetString(string stringId, params object[] args) {
            var result = Localization.ContainsKey(stringId)
                ? MyStringId.GetOrCompute(Localization[stringId])
                : MyStringId.Get(stringId);

            return args.Length == 0 ? result.GetString() : result.GetStringFormat(args);
        }
    }
}
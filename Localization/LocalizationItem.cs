using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage;

namespace Sisk.Utils.Localization {
    /// <summary>
    ///     A class that holds translations.
    /// </summary>
    internal class LocalizationItem {
        private readonly Dictionary<MyLanguagesEnum, string> _dictionary = new Dictionary<MyLanguagesEnum, string>();

        public LocalizationItem(IReadOnlyDictionary<MyLanguagesEnum, string> translations) {
            foreach (var pair in translations) {
                var language = pair.Key;
                var translation = pair.Value;
                if (!string.IsNullOrWhiteSpace(translation)) {
                    _dictionary.Add(language, translation);
                }
            }
        }

        private string Default => _dictionary[MyLanguagesEnum.English];

        public string this[MyLanguagesEnum index] => _dictionary[index];

        public static implicit operator string(LocalizationItem item) {
            return item.ToString();
        }

        public override string ToString() {
            var language = MyAPIGateway.Session.Config.Language;
            if (_dictionary.ContainsKey(language)) {
                return _dictionary[language];
            }

            return Default;
        }
    }
}
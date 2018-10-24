using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Utils;

// ReSharper disable InlineOutVariableDeclaration

namespace Sisk.Utils.Localization {
    public static class Texts {
        public const string LOCALIZATION_TAG = "LOC:";
        public const string LOCALIZATION_TAG_GENERAL = "LOCG:";

        private static readonly Dictionary<MyStringId, StringBuilder> StringBuilders = new Dictionary<MyStringId, StringBuilder>(MyStringId.Comparer);
        private static readonly Dictionary<MyStringId, string> Strings = new Dictionary<MyStringId, string>(MyStringId.Comparer);

        public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, object arg0) {
            return stringBuilder.AppendFormat(GetString(textEnum), arg0);
        }

        public static void Clear() {
            Strings.Clear();
            StringBuilders.Clear();
            Strings[new MyStringId()] = "";
            StringBuilders[new MyStringId()] = new StringBuilder();
        }

        public static bool Exists(MyStringId id) {
            return Strings.ContainsKey(id);
        }

        public static StringBuilder Get(MyStringId id) {
            StringBuilder sb;
            if (!StringBuilders.TryGetValue(id, out sb)) {
                sb = MyTexts.Get(id);
            }

            return sb;
        }

        public static string GetString(MyStringId id) {
            string str;
            if (!Strings.TryGetValue(id, out str)) {
                str = MyTexts.GetString(id);
            }

            return str;
        }

        public static string GetString(string keyString) {
            return GetString(MyStringId.GetOrCompute(keyString));
        }

        public static bool IsTagged(string text, int position, string tag) {
            return !tag.Where((t, index) => text[position + index] != (int) t).Any();
        }

        public static void LoadSupportedLanguages(HashSet<MyLanguagesEnum> outSupportedLanguages) {
            foreach (var value in Enum.GetValues(typeof(MyLanguagesEnum)).Cast<MyLanguagesEnum>()) {
                if (Lang.Contains(value)) {
                    outSupportedLanguages.Add(value);
                }
            }
        }

        public static void LoadTexts(MyLanguagesEnum language = MyLanguagesEnum.English) {
            var dict = Lang.Get(language);
            if (dict != null && dict.Any()) {
                PatchTexts(dict);
            }
        }

        public static void PatchTexts(IDictionary<string, string> resources) {
            foreach (var entry in resources) {
                var key = entry.Key;
                var str = entry.Value;
                if (key != null && str != null) {
                    var id = MyStringId.GetOrCompute(key);
                    Strings[id] = str;
                    StringBuilders[id] = new StringBuilder(str);
                }
            }
        }

        public static string SubstituteTexts(string text) {
            if (!text.StartsWith("{") || !text.EndsWith("}")) {
                return text;
            }

            if (IsTagged(text, 1, LOCALIZATION_TAG)) {
                var startIndex = LOCALIZATION_TAG.Length + 1;
                var stringBuilder = Get(MyStringId.GetOrCompute(text.Substring(startIndex, text.Length - startIndex - 1)));
                if (stringBuilder != null) {
                    return stringBuilder.ToString();
                }
            } else if (IsTagged(text, 1, LOCALIZATION_TAG_GENERAL)) {
                var startIndex = LOCALIZATION_TAG_GENERAL.Length + 1;
                var stringBuilder = Get(MyStringId.GetOrCompute(text.Substring(startIndex, text.Length - startIndex - 1)));
                if (stringBuilder != null) {
                    return stringBuilder.ToString();
                }
            }

            return text;
        }

        public static string SubstituteTextsDirect(string text) {
            if (text[0] != '{' || text[text.Length - 1] != '}' || !IsTagged(text, 1, LOCALIZATION_TAG)) {
                return text;
            }

            var startIndex = LOCALIZATION_TAG.Length + 1;
            var stringBuilder = Get(MyStringId.GetOrCompute(text.Substring(startIndex, text.Length - startIndex - 1)));
            if (stringBuilder != null) {
                return stringBuilder.ToString();
            }

            return text;
        }

        public static string TrySubstitute(string input) {
            var orCompute = MyStringId.GetOrCompute(input);
            StringBuilder sb;
            return !StringBuilders.TryGetValue(orCompute, out sb) ? input : sb.ToString();
        }
    }
}
using VRage;
using VRage.Utils;

namespace Sisk.Utils.Localization {
    public static class Extension {
        /// <summary>
        ///     Gets a string from <see cref="MyStringId" />.
        /// </summary>
        /// <param name="stringId">The <see cref="MyStringId" /> used to get the string.</param>
        /// <returns>Returns the string from <see cref="MyStringId" />.</returns>
        public static string GetString(this MyStringId stringId) {
            return MyTexts.GetString(stringId);
        }

        /// <summary>
        ///     Gets a formated string from <see cref="MyStringId" />.
        /// </summary>
        /// <param name="stringId">The <see cref="MyStringId" /> used to get the string.</param>
        /// <param name="args">The arguments used by <see cref="string.Format(string,object)" />.</param>
        /// <returns>Returns the formated string from <see cref="MyStringId" />.</returns>
        public static string GetStringFormat(this MyStringId stringId, params object[] args) {
            return string.Format(MyTexts.GetString(stringId), args);
        }
    }
}
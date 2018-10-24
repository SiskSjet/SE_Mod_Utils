using VRage.Utils;

namespace Sisk.Utils.Localization.Extensions {
    public static class MyStringIdExtensions {
        /// <summary>
        ///     Get the localized <see cref="MyStringId" /> for given <paramref name="stringId" />.
        /// </summary>
        /// <param name="stringId">The stringId that identifies the localized <see cref="MyStringId" />.</param>
        /// <returns>Returns the localized <see cref="string" />.</returns>
        public static string GetString(this MyStringId stringId) {
            return Texts.GetString(stringId);
        }

        /// <summary>
        ///     Gets the localized string for given <paramref name="stringId" /> and format the result if <paramref name="args" />
        ///     are set.
        /// </summary>
        /// <param name="stringId">The stringId that identifies the localized <see cref="MyStringId" />.</param>
        /// <param name="args">The arguments used by <see cref="string.Format(string,object)" />.</param>
        /// <returns>Returns the localized and formatted string.</returns>
        public static string GetString(this MyStringId stringId, params object[] args) {
            return string.Format(Texts.GetString(stringId), args);
        }
    }
}
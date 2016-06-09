using System.Text;

namespace MultiHash
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterEncodedString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterEncodedString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        public CharacterEncodedString(string value, Encoding encoding)
        {
            EncodedString = encoding.GetBytes(value);
        }

        /// <summary>
        /// Gets the encoded string.
        /// </summary>
        /// <value>
        /// The encoded string.
        /// </value>
        public byte[] EncodedString { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CharacterEncodedString"/> to <see>
        ///         <cref>System.Byte[]</cref>
        ///     </see>
        ///     .
        /// </summary>
        /// <param name="characterEncodedString">The character encoded string.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte[](CharacterEncodedString characterEncodedString)
        {
            return characterEncodedString.EncodedString;
        }
    }
}
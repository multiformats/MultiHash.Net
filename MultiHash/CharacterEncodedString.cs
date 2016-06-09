using System.Text;

namespace MultiHash
{
    public class CharacterEncodedString
    {
        public CharacterEncodedString(string value, Encoding encoding)
        {
            EncodedString = encoding.GetBytes(value);
        }

        public byte[] EncodedString { get; }

        public static implicit operator byte[](CharacterEncodedString characterEncodedString)
        {
            return characterEncodedString.EncodedString;
        }
    }
}
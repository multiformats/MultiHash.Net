using System;
using System.Linq;

namespace MultiHash
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultiHash
    {
        /// <summary>
        /// Encodes the hash according to the multihash protocol
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        public static byte[] MultiHashEncode(this Hash hash)
        {
            var codeByteSequence = BitConverter.GetBytes(hash.Algorithm.Code);
            byte[] digest = hash;
            var lengthByteSequence = BitConverter.GetBytes(digest.Length);
            var multiHashResult = codeByteSequence.Concat(lengthByteSequence).Concat(digest).ToArray();
            return multiHashResult;
        }

        /// <summary>
        /// Decodes the specified multihash encoded digest.
        /// </summary>
        /// <param name="multiHashEncodedDigest">The multihash encoded digest.</param>
        /// <returns></returns>
        public static Hash Decode(byte[] multiHashEncodedDigest)
        {
            var bytes =
                multiHashEncodedDigest.Skip(8)
                    .Take(BitConverter.ToInt32(multiHashEncodedDigest.Skip(4).Take(4).ToArray(), 0));
            HashAlgorithmCode hashAlgorithmCode = multiHashEncodedDigest[0];
            HashAlgorithm hashAlgorithm = hashAlgorithmCode;
            return new Hash(bytes.ToArray(), hashAlgorithm);
        }
    }
}
using System;
using System.Linq;

namespace MultiHash
{
    public static class MultiHash
    {
        public static byte[] MultiHashEncode(this Hash hash)
        {
            var codeByteSequence = BitConverter.GetBytes(hash.Algorithm.Code);
            byte[] digest = hash;
            var lengthByteSequence = BitConverter.GetBytes(digest.Length);
            var multiHashResult = codeByteSequence.Concat(lengthByteSequence).Concat(digest).ToArray();
            return multiHashResult;
        }

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
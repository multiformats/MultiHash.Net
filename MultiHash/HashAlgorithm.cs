using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MultiHash
{
    public delegate byte[] HashFunction(byte[] bytesToHash);

    public class HashAlgorithmCode
    {
        public static readonly HashAlgorithmCode Sha1 = new HashAlgorithmCode(0x11);
        public static readonly HashAlgorithmCode Sha2_256 = new HashAlgorithmCode(0x12);
        public static readonly HashAlgorithmCode Sha2_512 = new HashAlgorithmCode(0x13);
        public static readonly HashAlgorithmCode Sha3_512 = new HashAlgorithmCode(0x14);
        public static readonly HashAlgorithmCode Sha3_384 = new HashAlgorithmCode(0x15);
        public static readonly HashAlgorithmCode Sha3_256 = new HashAlgorithmCode(0x16);
        public static readonly HashAlgorithmCode Sha3_224 = new HashAlgorithmCode(0x17);
        public static readonly HashAlgorithmCode Shake_128 = new HashAlgorithmCode(0x18);
        public static readonly HashAlgorithmCode Shake_256 = new HashAlgorithmCode(0x19);
        public static readonly HashAlgorithmCode Blake2B = new HashAlgorithmCode(0x40);
        public static readonly HashAlgorithmCode Blake2S = new HashAlgorithmCode(0x41);

        private HashAlgorithmCode(int code)
        {
            Code = code;
        }

        public int Code { get; }

        public static implicit operator HashAlgorithmCode(int code)
        {
            return new HashAlgorithmCode(code);
        }

        public static implicit operator int(HashAlgorithmCode code)
        {
            return code.Code;
        }
    }

    // 0x11 sha1
    // 0x12 sha2-256
    // 0x13 sha2-512
    // 0x14 sha3-512
    // 0x15 sha3-384
    // 0x16 sha3-256
    // 0x17 sha3-224
    // 0x18 shake-128
    // 0x19 shake-256
    // 0x40 blake2b
    // 0x41 blake2s

    public sealed class HashAlgorithm
    {
        private static readonly Dictionary<int, Tuple<string, HashFunction>> MapCodeAndAlgorithm = new Dictionary
            <int, Tuple<string, HashFunction>>
        {
            {0x11, new Tuple<string, HashFunction>("sha1", SHA1.Create().ComputeHash)},
            {0x12, new Tuple<string, HashFunction>("sha2-256", SHA256.Create().ComputeHash)},
            {0x13, new Tuple<string, HashFunction>("sha2-512", SHA512.Create().ComputeHash)},
            {0x14, new Tuple<string, HashFunction>("sha3-512", HashAlgorithmIsNotSupported("sha3-512"))},
            {0x15, new Tuple<string, HashFunction>("sha3-384", HashAlgorithmIsNotSupported("sha3-384"))},
            {0x16, new Tuple<string, HashFunction>("sha3-256", HashAlgorithmIsNotSupported("sha3-256"))},
            {0x17, new Tuple<string, HashFunction>("sha3-224", HashAlgorithmIsNotSupported("sha3-224"))},
            {0x18, new Tuple<string, HashFunction>("shake-128", HashAlgorithmIsNotSupported("shake-128"))},
            {0x19, new Tuple<string, HashFunction>("shake-256", HashAlgorithmIsNotSupported("shake-256"))},
            {0x40, new Tuple<string, HashFunction>("blake2b", HashAlgorithmIsNotSupported("blake2b"))},
            {0x41, new Tuple<string, HashFunction>("blake2s", HashAlgorithmIsNotSupported("blake2s"))}
        };

        public static readonly HashAlgorithm Sha1 = new HashAlgorithm(HashAlgorithmCode.Sha1);
        public static readonly HashAlgorithm Sha2_256 = new HashAlgorithm(HashAlgorithmCode.Sha2_256);
        public static readonly HashAlgorithm Sha2_512 = new HashAlgorithm(HashAlgorithmCode.Sha2_512);
        public static readonly HashAlgorithm Sha3_512 = new HashAlgorithm(HashAlgorithmCode.Sha3_512);
        public static readonly HashAlgorithm Sha3_384 = new HashAlgorithm(HashAlgorithmCode.Sha3_384);
        public static readonly HashAlgorithm Sha3_256 = new HashAlgorithm(HashAlgorithmCode.Sha3_256);
        public static readonly HashAlgorithm Sha3_224 = new HashAlgorithm(HashAlgorithmCode.Sha3_224);
        public static readonly HashAlgorithm Shake_128 = new HashAlgorithm(HashAlgorithmCode.Shake_128);
        public static readonly HashAlgorithm Shake_256 = new HashAlgorithm(HashAlgorithmCode.Shake_256);
        public static readonly HashAlgorithm Blake2B = new HashAlgorithm(HashAlgorithmCode.Blake2B);
        public static readonly HashAlgorithm Blake2S = new HashAlgorithm(HashAlgorithmCode.Blake2S);
        private readonly HashFunction _hashFunc;

        private HashAlgorithm(HashAlgorithmCode code)
        {
            var entry = MapCodeAndAlgorithm[code];
            _hashFunc = entry.Item2;
            Code = code;
            Name = entry.Item1;
        }

        public HashAlgorithmCode Code { get; private set; }
        public string Name { get; private set; }

        private static HashFunction HashAlgorithmIsNotSupported(string algorithmName)
        {
            return bytes =>
            {
                throw new NotSupportedException(
                    $"The {algorithmName} hash algorithm is not supported in the .NET Framework");
            };
        }

        public static implicit operator HashAlgorithm(HashAlgorithmCode code)
        {
            return new HashAlgorithm(code);
        }

        public Hash Compute(byte[] bytes)
        {
            return new Hash(_hashFunc(bytes), this);
        }
    }
}
using System;
using System.Linq;

namespace MultiHash
{
    public class Hash : IEquatable<Hash>
    {
        private readonly byte[] _hash;

        public Hash(byte[] bytes, HashAlgorithm hashAlgorithm)
        {
            _hash = bytes;
            Algorithm = hashAlgorithm;
        }

        public HashAlgorithm Algorithm { get; }

        public bool Equals(Hash other)
        {
            return _hash.SequenceEqual(other._hash);
        }

        public static implicit operator byte[](Hash hash)
        {
            return hash._hash;
        }
    }
}
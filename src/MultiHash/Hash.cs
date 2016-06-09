using System;
using System.Linq;

namespace MultiHash
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Hash" />
    public class Hash : IEquatable<Hash>
    {
        private readonly byte[] _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        public Hash(byte[] bytes, HashAlgorithm hashAlgorithm)
        {
            _hash = bytes;
            Algorithm = hashAlgorithm;
        }

        /// <summary>
        /// Gets the algorithm.
        /// </summary>
        /// <value>
        /// The algorithm.
        /// </value>
        public HashAlgorithm Algorithm { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Hash other)
        {
            return _hash.SequenceEqual(other._hash);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Hash"/> to <see>
        ///         <cref>System.Byte[]</cref>
        ///     </see>
        ///     .
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte[](Hash hash)
        {
            return hash._hash;
        }
    }
}
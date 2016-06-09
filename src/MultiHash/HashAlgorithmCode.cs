namespace MultiHash
{
    /// <summary>
    /// 
    /// </summary>
    public class HashAlgorithmCode
    {
        /// <summary>
        /// The sha1 hashing algorithm
        /// </summary>
        public static readonly HashAlgorithmCode Sha1 = new HashAlgorithmCode(0x11);
        /// <summary>
        /// The sha2_256 hashing algorithm
        /// </summary>
        public static readonly HashAlgorithmCode Sha2_256 = new HashAlgorithmCode(0x12);
        /// <summary>
        /// The sha2_512 hashing algorithm
        /// </summary>
        public static readonly HashAlgorithmCode Sha2_512 = new HashAlgorithmCode(0x13);
        /// <summary>
        /// The sha3_512 hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Sha3_512 = new HashAlgorithmCode(0x14);
        /// <summary>
        /// The sha3_384  hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Sha3_384 = new HashAlgorithmCode(0x15);
        /// <summary>
        /// The sha3_256 hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Sha3_256 = new HashAlgorithmCode(0x16);
        /// <summary>
        /// The sha3_224 hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Sha3_224 = new HashAlgorithmCode(0x17);
        /// <summary>
        /// The shake_128 hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Shake_128 = new HashAlgorithmCode(0x18);
        /// <summary>
        /// The shake_256 hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Shake_256 = new HashAlgorithmCode(0x19);
        /// <summary>
        /// The blake2 b hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Blake2B = new HashAlgorithmCode(0x40);
        /// <summary>
        /// The blake2 s  hashing algorithm. WARNING : .NET does not support SHA3! 
        /// Choosing this algorithm will throw and is only present because the multihash protocol
        /// requires support
        /// </summary>
        public static readonly HashAlgorithmCode Blake2S = new HashAlgorithmCode(0x41);

        private HashAlgorithmCode(int code)
        {
            Code = code;
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int Code { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="HashAlgorithmCode"/>.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator HashAlgorithmCode(int code)
        {
            return new HashAlgorithmCode(code);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="HashAlgorithmCode"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(HashAlgorithmCode code)
        {
            return code.Code;
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using FsCheck;
using Xunit;

namespace multihash.net.properties
{
    public class Generators
    {
        public static readonly Gen<HashFunction> HashFunctionGenerator =
            Arb.From<HashFunction>().Generator
                .Where(
                    function =>
                        new[] {0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x40, 0x41}.Contains(function.Code));

        public static Arbitrary<HashFunction> ArbitraryHashFunction => Arb.From(HashFunctionGenerator);
    }

    public class MultiHashProperties
    {
        public MultiHashProperties()
        {
            Arb.Register<Generators>();
        }

        [Fact]
        public void Can_successfully_decode_a_multihash_after_encoding_a_digest()
        {
            Prop.ForAll(Generators.ArbitraryHashFunction, Arb.From<NonEmptyString>(), (hashFunction, nonEmptyString) =>
            {
                var digest = nonEmptyString.Get;
                var characterEncodedDigest = Encoding.UTF8.GetBytes(digest);
                var encodedDigest = MultiHash.Encode(hashFunction, characterEncodedDigest);
                var decodedDigest = MultiHash.Decode(encodedDigest);

                Assert.Equal(decodedDigest, digest);

            }).VerboseCheckThrowOnFailure();
        }
    }

    public class MultiHash
    {
        public static byte[] Encode(HashFunction hashFunction, byte[] digest)
        {
            var codeByteSequence = BitConverter.GetBytes(hashFunction.Code);
            var lengthByteSequence = BitConverter.GetBytes(digest.Length);
            var multiHashResult = codeByteSequence.Concat(lengthByteSequence).Concat(digest).ToArray();
            return multiHashResult;
        }

        public static string Decode(byte[] encodedDigest)
        {
            throw new System.NotImplementedException();
        }
    }
}

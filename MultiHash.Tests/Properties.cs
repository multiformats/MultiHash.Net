using System.Text;
using FsCheck;
using Xunit;

namespace MultiHash.Tests
{
    public class Generators
    {
        public static Gen<HashAlgorithm> HashFunctionGenerator = Gen.Elements(HashAlgorithm.Sha1, HashAlgorithm.Sha2_256,
            HashAlgorithm.Sha2_512);

        public static Arbitrary<HashAlgorithm> ArbitraryHashFunction => Arb.From(HashFunctionGenerator);
    }

    public class Properties
    {
        public Properties()
        {
            Arb.Register<Generators>();
        }

        [Fact]
        public void Can_successfully_decode_a_multihash_after_encoding_a_digest()
        {
            Prop.ForAll(Generators.ArbitraryHashFunction, Arb.From<NonEmptyString>(), (hashAlgorithm, nonEmptyString) =>
            {
                var digest = nonEmptyString.Get;
                byte[] characterEncodedDigest = new CharacterEncodedString(digest, Encoding.ASCII);
                var hash = hashAlgorithm.Compute(characterEncodedDigest);
                var encodedDigest = hash.MultiHashEncode();
                var decodedDigest = MultiHash.Decode(encodedDigest);

                Assert.Equal(hash, decodedDigest);
            }).VerboseCheckThrowOnFailure();
        }
    }
}
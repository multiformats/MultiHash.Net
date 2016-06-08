namespace multihash.net.properties
{
    public class HashFunction
    {
        public HashFunction(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; private set; }
        public string Name { get; private set; }
        
    }
}
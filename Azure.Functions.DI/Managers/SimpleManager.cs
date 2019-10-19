using Azure.Functions.DI.Interfaces;

namespace Azure.Functions.DI.Managers
{
    public class SimpleManager : ISimpleManager
    {
        public string ToUpperCase(string s)
        {
            return s.ToUpper();
        }
    }
}

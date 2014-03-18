using System.Collections.Generic;
using System.Linq;

namespace Ripple.Core.Core.Enums
{
    public class LedgerEntryType
    {
        private static readonly IDictionary<string, LedgerEntryType> ByName = new Dictionary<string, LedgerEntryType>();
        private static readonly IDictionary<int, LedgerEntryType> ByCode = new Dictionary<int, LedgerEntryType>();
        private readonly int _ord;

        static LedgerEntryType()
        {
            ByName.Add("Invalid", Invalid);
            ByName.Add("AccountRoot", AccountRoot);
            ByName.Add("DirectoryNode", DirectoryNode);
            ByName.Add("GeneratorMap", GeneratorMap);
            ByName.Add("RippleState", RippleState);
            ByName.Add("Nickname", Nickname);
            ByName.Add("Offer", Offer);
            ByName.Add("Contract", Contract);
            ByName.Add("LedgerHashes", LedgerHashes);
            ByName.Add("EnabledFeatures", EnabledFeatures);
            ByName.Add("FeeSettings", FeeSettings);

            foreach (var value in Values)
            {
                ByCode.Add(value._ord, value);
            }
        }

        private LedgerEntryType(int i)
        {
            _ord = i;
        }

        public static readonly LedgerEntryType Invalid = new LedgerEntryType(-1);
        public static readonly LedgerEntryType AccountRoot = new LedgerEntryType('a');
        public static readonly LedgerEntryType DirectoryNode = new LedgerEntryType('d');
        public static readonly LedgerEntryType GeneratorMap = new LedgerEntryType('g');
        public static readonly LedgerEntryType RippleState = new LedgerEntryType('r');
        public static readonly LedgerEntryType Nickname = new LedgerEntryType('n');
        public static readonly LedgerEntryType Offer = new LedgerEntryType('o');
        public static readonly LedgerEntryType Contract = new LedgerEntryType('c');
        public static readonly LedgerEntryType LedgerHashes = new LedgerEntryType('h');
        public static readonly LedgerEntryType EnabledFeatures = new LedgerEntryType('f');
        public static readonly LedgerEntryType FeeSettings = new LedgerEntryType('s');

        public static IEnumerable<LedgerEntryType> Values
        {
            get
            {
                yield return Invalid;
                yield return AccountRoot;
                yield return DirectoryNode;
                yield return GeneratorMap;
                yield return RippleState;
                yield return Nickname;
                yield return Offer;
                yield return Contract;
                yield return LedgerHashes;
                yield return EnabledFeatures;
                yield return FeeSettings;
            }
        }

        public int AsInteger
        {
            get { return _ord; }
        }

        public static LedgerEntryType FromNumber(int i)
        {
            return ByCode[i];
        }

        public static LedgerEntryType FromString(string s)
        {
            return ByName[s];
        }

        public string GetName()
        {
            return ByName.Single(q => q.Value == this).Key;
        }
    }
}

using System;
using System.Collections.Generic;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Crypto.Ecdsa;
using Ripple.Core.Encodings.Common;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Coretypes
{
    public class AccountId : ISerializedType, IComparable<AccountId>
    {
        protected IKeyPair keyPair;
        protected byte[] AddressBytes;

        public static TypedFields.AccountIdField Account = new AccountIdField(Field.Account);
        public static TypedFields.AccountIdField Owner = new AccountIdField(Field.Owner);
        public static TypedFields.AccountIdField Destination = new AccountIdField(Field.Destination);
        public static TypedFields.AccountIdField Issuer = new AccountIdField(Field.Issuer);
        public static TypedFields.AccountIdField Target = new AccountIdField(Field.Target);
        public static TypedFields.AccountIdField RegularKey = new AccountIdField(Field.RegularKey);
        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static IDictionary<string, AccountId> Accounts = new Dictionary<string, AccountId>();
        public static AccountId One = FromInteger(1);
        public static AccountId Zero = FromInteger(0);
        public string Address;

        static AccountId()
        {
            Accounts.Add("root", AccountForPass("masterpassphrase")); 
        }

        protected AccountId()
        {
        }

        public static AccountId FromSeed(string masterSeed)
        {
            var a = new AccountId();
            PopulateFieldsFromKeyPair(a, KeyPairFromSeed(masterSeed));
            return a;
        }

        public static AccountId FromSeed(byte[] masterSeed)
        {
            var a = new AccountId();
            PopulateFieldsFromKeyPair(a, KeyPairFromSeed(masterSeed));
            return a;
        }

        public static AccountId FromInteger(int n)
        {
            var a = new AccountId {AddressBytes = new Hash160(new UInt32((long)n).ToByteArray()).Bytes};
            a.Address = Config.Config.B58IdentiferCodecs.EncodeAddress(a.Bytes);
            return a;
        }

        public static AccountId FromAddress(string address)
        {
            var a = new AccountId
            {
                keyPair = null,
                AddressBytes = Config.Config.B58IdentiferCodecs.DecodeAddress(address),
                Address = address
            };
            return a;
        }

        static public AccountId FromAddress(byte[] bytes)
        {
            var a = new AccountId
            {
                keyPair = null,
                AddressBytes = bytes,
                Address = Config.Config.B58IdentiferCodecs.EncodeAddress(bytes)
            };
            return a;
        }

        public static AccountId AccountForPassPhrase(string value)
        {

            if (!Accounts.ContainsKey(value))
            {
                Accounts.Add(value, AccountForPass(value));
            }

            return Accounts[value];
        }

        public static AccountId FromString(string value)
        {
            // TODO No valid addresses should ever fail below condition
            if (value.StartsWith("r") && value.Length >= 26)
            {
                return FromAddress(value);
            }
            
            if (value.Length == 160 / 4)
            {
                return FromAddress(B16.Decode(value));
            }

            // This is potentially dangerous but fromString in
            // generic sense is used by Amount for parsing strings
            return AccountForPassPhrase(value);
        }

        public static IKeyPair KeyPairFromSeed(string masterSeed)
        {
            return KeyPairFromSeed(Config.Config.B58IdentiferCodecs.DecodeFamilySeed(masterSeed));
        }

        public static IKeyPair KeyPairFromSeed(byte[] masterSeed)
        {
            return Seed.CreateKeyPair(masterSeed);
        }


        public byte[] Bytes
        {
            get { return AddressBytes; }
        }

        public IKeyPair KeyPair
        {
            get { return keyPair; }
        }

        public Issue Issue(String code)
        {
            return new Issue(Currency.FromString(code), this);
        }

        public int CompareTo(AccountId other)
        {
            return String.Compare(Address, other.Address, StringComparison.Ordinal);
        }

        public object ToJson()
        {
            return ToString();
        }

        public byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public void ToBytesSink(IBytesSink to)
        {
            to.Add(Bytes);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var id = obj as AccountId;
            if (id != null)
            {
                return Address.Equals(id.Address);
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return Address;
        }

        private static void PopulateFieldsFromKeyPair(AccountId a, IKeyPair kp)
        {
            a.keyPair = kp;
            a.AddressBytes = Utils.Utils.SHA256_RIPEMD160(kp.Pub().ToByteArray());
            a.Address = Config.Config.B58IdentiferCodecs.EncodeAddress(a.Bytes);
        }

        private static AccountId AccountForPass(string value)
        {
            return FromSeed(Seed.PassPhraseToSeedBytes(value));
        }

        public class AccountIdField : TypedFields.AccountIdField
        {
            private readonly Field _f;

            public AccountIdField(Field f)
            {
                _f = f;
            }
            public override Field GetField()
            {
                return _f;
            }
        }

        public class OutTranslator : OutTypeTranslator<AccountId>
        {
            public override AccountId FromParser(BinaryParser parser, int? hint)
            {
                if (hint == null)
                {
                    hint = 20;
                }

                return FromAddress(parser.Read((int)hint));
            }

            public override AccountId FromString(string s)
            {
                return AccountId.FromString(s);
            }
        }

        public class InTranslator : InTypeTranslator<AccountId>
        {
            public override string ToString(AccountId obj)
            {
                return obj.ToString();
            }
        }
    }
}

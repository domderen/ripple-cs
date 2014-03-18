namespace Ripple.Core.Core.Fields
{
    public class TypedFields
    {
        public abstract class UInt8Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class Vector256Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class VariableLengthField : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class UInt64Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class UInt32Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class UInt16Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class PathSetField : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class StObjectField : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class Hash256Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class Hash160Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class Hash128Field : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class StArrayField : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class AmountField : IHasField
        {
            public abstract Field GetField();
        }

        public abstract class AccountIdField : IHasField
        {
            public abstract Field GetField();
        }
    }
}

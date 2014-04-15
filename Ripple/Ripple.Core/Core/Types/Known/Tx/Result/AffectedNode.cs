using System.Linq;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Types.Known.Tx.Result
{
    // TODO: Fix up this nonsense.
    public class AffectedNode : StObject
    {
        private readonly Field _field;
        private readonly StObject _nested;

        public AffectedNode(StObject source)
        {
            fields = source.Fields;
            _field = Field;
            _nested = GetNested;
        }

        public bool WasPreviousNode
        {
            get { return IsDeletedNode || IsModifiedNode; }
        }

        public bool IsCreatedNode
        {
            get { return _field == Field.CreatedNode; }
        }

        public bool IsDeletedNode
        {
            get { return _field == Field.DeletedNode; }
        }

        public bool IsModifiedNode
        {
            get { return _field == Field.ModifiedNode; }
        }

        public Hash256 LedgerIndex
        {
            get { return _nested[Hash256.LedgerIndex]; }
        }

        public LedgerEntryType LedgerEntryType
        {
            get { return LedgerEntryType(_nested); }
        }

        public Field Field
        {
            get { return fields.First().Key; }
        }

        private StObject GetNested
        {
            get { return (StObject) base[Field]; }
        }

        public static bool IsAffectedNode(StObject source)
        {
            return (source.Count == 1 && source.Has(DeletedNode) || source.Has(CreatedNode) || source.Has(ModifiedNode));
        }

        public StObject NodeAsPrevious()
        {
            return RebuildFromMeta(true);
        }

        public StObject NodeAsFinal()
        {
            return RebuildFromMeta(false);
        }

        public StObject RebuildFromMeta(bool layerPrevious)
        {
            var mixed = new StObject();
            bool created = IsCreatedNode;

            var wrapperField = created
                ? Field.CreatedNode
                : IsDeletedNode
                    ? Field.DeletedNode
                    : Field.ModifiedNode;

            var wrapped = (StObject)base[wrapperField];

            var finalFields = created
                ? Field.NewFields
                : Field.FinalFields;

            var finals = (StObject) wrapped[finalFields];
            foreach (var field in finals)
            {
                mixed.Add(field, finals[field]);
            }

            // DirectoryNode LedgerEntryType won't have `PreviousFields`
            if (layerPrevious && wrapped.Has(Field.PreviousFields))
            {
                var previous = wrapped[PreviousFields];
                var changed = new StObject();
                mixed.Add(Field.FinalFields, changed);

                foreach (var field in previous)
                {
                    mixed.Add(field, previous[field]);
                    changed.Add(field, finals[field]);
                }
            }

            foreach (var field in wrapped)
            {
                if (field == Field.NewFields || field == Field.PreviousFields || field == Field.FinalFields)
                {
                    continue;
                }

                mixed.Add(field, wrapped[field]);
            }
            return Formatted(mixed);
        }

    }
}

namespace Ripple.Core.Core.Types.Shamap
{
    using Ripple.Core.Core.Coretypes.Hash;

    /// <summary>
    /// This is a toy implementation for illustrative purposes.
    /// </summary>
    public class NodeStore
    {
        private IKeyValueBackend backend;

        public NodeStore(IKeyValueBackend backend)
        {
            this.backend = backend;
        }

        /// <summary>
        /// The complement to `set` api, which together form a simple public interface.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public byte[] Get(Hash256 hash)
        {
            return this.backend.Get(hash);
        }

        /// <summary>
        /// The complement to `get` api, which together form a simple public interface.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Hash256 Set(byte[] content)
        {
            return this.StoreContent(content);
        }

        /// <summary>
        /// All data stored is keyed by the hash of it's contents.
        /// Ripple uses the first 256 bits of a sha512 as it's 33 percent
        /// faster than using sha256.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>
        /// `key` used to store the content.
        /// </returns>
        private Hash256 StoreContent(byte[] content)
        {
            var hasher = new Hash256.HalfSha512();
            hasher.Update(content);
            Hash256 key = hasher.Finish();
            this.StoreHashKeyedContent(key, content);
            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hash">
        /// As ripple uses the `hash` of the contents as the
        /// NodeStore key, `hash` is pervasively used in lieu of
        /// the term `key`.
        /// </param>
        /// <param name="content"></param>
        private void StoreHashKeyedContent(Hash256 hash, byte[] content)
        {
            // Note: The real nodestore actually prepends some metadata, which doesn't
            // contribute to the hash.
            this.backend.Put(hash, content); // metadata + content
        }

        /// <summary>
        /// In ripple, all data is stored in a simple binary key/value database.
        /// The keys are 256 bit binary strings and the values are binary strings of
        /// arbitrary length.
        /// </summary>
        public interface IKeyValueBackend
        {
            void Put(Hash256 key, byte[] content);

            byte[] Get(Hash256 key);
        }
    }
}

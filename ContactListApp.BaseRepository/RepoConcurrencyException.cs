using System;

namespace ContactListApp.BaseRepository {
    public class RepoConcurrencyException<TEntity> : Exception {
        public RepoConcurrencyException(TEntity entity, Exception ex) :
            base("Concurrency error", ex) {
            Entity = entity;
        }

        public TEntity Entity { get; private set; }

        public TEntity DbEntity { get; set; }

        public byte[] Contact_Version { get; set; } = null;
    }
}
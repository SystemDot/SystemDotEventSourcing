namespace SystemDot.EventSourcing.Handlers
{
    using System.Collections;
    using System.Collections.Generic;

    public class SagaLoaderCollection<TEvent> : IEnumerable<ISagaLoader<TEvent>>
    {
        readonly List<ISagaLoader<TEvent>> inner;

        public SagaLoaderCollection()
        {
            inner = new List<ISagaLoader<TEvent>>();
        }

        public IEnumerator<ISagaLoader<TEvent>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void RegisterSaga(ISagaLoader<TEvent> toRegister)
        {
            inner.Add(toRegister);
        }
    }
}
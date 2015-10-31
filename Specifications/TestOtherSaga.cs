namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.EventSourcing.Sagas;

    class TestOtherSaga : Saga, IStartSaga<TestStartSagaEvent>
    {
        public void When(TestStartSagaEvent command)
        {
            Then(new TestOtherSagaGeneratedCommand());
        }
    }
}
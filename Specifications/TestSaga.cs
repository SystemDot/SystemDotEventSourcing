namespace SystemDot.EventSourcing.Specifications
{
    using SystemDot.EventSourcing.Sagas;

    class TestSaga : Saga, 
        IStartSaga<TestStartSagaEvent>, 
        IContinueSaga<TestSagaEvent>
    {
        string state;

        public void When(TestStartSagaEvent command)
        {
            Then<TestSagaGeneratedCommand>(e => e.Id = command.Id);
            Then(new TestSecondSagaGeneratedCommand());
        }

        public void When(TestSagaEvent command)
        {
            Then<TestSagaGeneratedCommandWithState>(e => e.State = state);
        }

        void ApplyEvent(TestStartSagaEvent @event)
        {
            state = @event.State;
        }
    }
}
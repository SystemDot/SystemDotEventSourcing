using BoDi;
using Microsoft.Owin.Testing;
using TechTalk.SpecFlow;

namespace SystemDot.EventSourcing.Synchronisation.Testing
{
    [Binding]
    public class TestClientSynchronisationBehaviour
    {
        readonly IObjectContainer specFlowContainer;
        TestServer currentServer;

        TestClientSynchronisationBehaviour(IObjectContainer specFlowContainer)
        {
            this.specFlowContainer = specFlowContainer;
        }

        [BeforeScenario("TestClientSynchronisation")]
        public void OnBeforeScenario()
        {
           currentServer = TestServer.Create<Startup>();
           Register(currentServer);
        }
        
        [AfterScenario("TestClientSynchronisation")]
        public void OnAfterScenario()
        {
            currentServer.Dispose();
        }

        void Register<T>(T toRegister) where T : class
        {
            specFlowContainer.RegisterInstanceAs<T>(toRegister);
        }
    }
}
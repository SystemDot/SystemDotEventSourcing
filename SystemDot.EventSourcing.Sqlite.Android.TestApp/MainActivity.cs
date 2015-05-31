using Android.App;
using Android.Widget;
using Android.OS;

namespace SystemDot.EventSourcing.Sqlite.Android.TestApp
{
    using System;
    using System.Threading.Tasks;
    using SystemDot.Bootstrapping;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Environment;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.Sessions;
    using SystemDot.EventSourcing.Sqlite.Android.Bootstrapping;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;

    [Activity(Label = "SystemDot.EventSourcing.Sqlite.Android.TestApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        readonly IocContainer container = new IocContainer();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            SetupEventSourcing().Wait();
            FindViewById<Button>(Resource.Id.DuplicateCommitButton).Click += OnDuplicateCommitButtonClick;
        }

        void OnDuplicateCommitButtonClick(object sender, EventArgs eventArgs)
        {
            Guid commitId = Guid.NewGuid();
            var factory = container.Resolve<IEventSessionFactory>();

            IEventSession session = factory.Create();
            session.StoreEvent(new SourcedEvent { Body = new TestEvent() }, new EventStreamId("Test", "Test"));
            session.Commit(commitId);
            session.Commit(commitId);
        }

        async Task SetupEventSourcing()
        {
            await Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().WithSimpleMessaging()
                .UseEventSourcing().PersistToSqlite()
                .InitialiseAsync();
        }
    }
}


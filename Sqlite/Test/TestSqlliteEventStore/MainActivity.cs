using Android.App;
using Android.Widget;
using Android.OS;

namespace SqlliteEventStore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using SystemDot.Bootstrapping;
    using SystemDot.Core;
    using SystemDot.Core.Collections;
    using SystemDot.Domain;
    using SystemDot.Domain.Bootstrapping;
    using SystemDot.Environment;
    using SystemDot.EventSourcing;
    using SystemDot.EventSourcing.Aggregation;
    using SystemDot.EventSourcing.Bootstrapping;
    using SystemDot.EventSourcing.InMemory.Bootstrapping;
    using SystemDot.EventSourcing.Sqlite.Android;
    using SystemDot.EventSourcing.Sqlite.Android.Bootstrapping;
    using SystemDot.EventSourcing.Streams;
    using SystemDot.Ioc;
    using SystemDot.ThreadMarshalling;
    using Java.Util;

    [Activity(Label = "SqlliteEventStore", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            MainActivityLocator.Set(this);

            var container = new IocContainer();

            Setup(container).Wait();

            container.Resolve<TestAggregateThing>().Do();

            SqlLitePersistenceEngine engine = new SqlLitePersistenceEngine(new JsonSerializer());
            SqlLiteEventStore store = new SqlLiteEventStore(engine);
            //TestRootId testRootId = new TestRootId();
            
            //IEventStream stream = store.OpenStream(testRootId.ToEventStreamId());
            //stream.Add(new SourcedEvent
            //{
            //    Body = new TestEvent
            //    {
            //        TestValue = "Hello",
            //        TestStructure = new TestStructure
            //        {
            //            TestStructureValue = 1
            //        }
            //    }
            //});

            //stream.CommitChanges(Guid.NewGuid());

            store.GetCommits().ForEach(c => c.Events.ForEach(OutputEvent));
            base.OnCreate(bundle);
        }

        static void OutputEvent(SourcedEvent e)
        {
            Trace.TraceInformation(e.Body.ToString());
        }

        static async Task Setup(IocContainer container)
        {
            await Bootstrap.Application()
                .ResolveReferencesWith(container)
                .UseEnvironment()
                .UseDomain().DispatchEventsOnUiThread().WithSimpleMessaging()
                .UseEventSourcing().PersistToSqlite()
                .InitialiseAsync();
        }
    }

    public class TestAggregateThing
    {
        readonly IDomainRepository repository;

        public TestAggregateThing(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public void Do()
        {
            repository.Save(new TestAggregate());
        }
    }

    public class TestAggregate : AggregateRoot
    {
        public TestAggregate() : base (new TestRootId())
        {
            AddEvent(new TestEvent
            {
                TestValue = "Hello",
                TestStructure = new TestStructure
                {
                    TestStructureValue = 1
                }
            });
        }
    }

    public class TestRootId : MultiSiteId
    {
        public TestRootId() : base(Guid.NewGuid().ToString(), "SiteId")
        {
        }
    }

    public class TestStructure
    {
        public int TestStructureValue { get; set; }
        
        public override string ToString()
        {
            return "TestStructureValue: " + TestStructureValue;
        }
    }

    public class TestEvent
    {
        public string TestValue { get; set; }
        public TestStructure TestStructure { get; set; }

        public override string ToString()
        {
            return "TestValue: " + TestValue + "TestStructure: " + TestStructure;
        }
    }
}


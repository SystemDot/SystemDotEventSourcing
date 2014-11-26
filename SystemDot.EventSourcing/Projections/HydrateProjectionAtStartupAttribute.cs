namespace SystemDot.EventSourcing.Projections
{
    using System;
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HydrateProjectionAtStartupAttribute : Attribute
    {
    }
}
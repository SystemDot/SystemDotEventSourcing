namespace SystemDot.EventSourcing.Commits
{
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendDelimeted(this StringBuilder builder, object value)
        {
            return builder.Append(value).Append("|");
        }
    }
}
namespace SystemDot.EventSourcing.Sqlite.Android
{
    using System.IO;

    public interface ISerialize
    {
        void Serialize<T>(Stream output, T graph);
        T Deserialize<T>(Stream input);
    }
}
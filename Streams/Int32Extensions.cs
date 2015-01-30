using System;

namespace SystemDot.EventSourcing.Streams
{
    public static class Int32Extensions
    {
        public static int ShiftBitsUp(this int toShift, int by)
        {
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(toShift), 0);
            uint wrapped = number >> (32 - by);
            return BitConverter.ToInt32(BitConverter.GetBytes((number << by) | wrapped), 0);
        }
    }
}
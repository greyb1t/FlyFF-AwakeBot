using System;

namespace Awabot.Core.Exceptions
{
    class AwakeningParseException : Exception
    {
        public AwakeningParseException(string message) : base(message) { }
    }
}
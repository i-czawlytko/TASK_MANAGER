using System;


namespace TaskManager.Infrastructure
{
    class ChangeStatusException : Exception
    {
        public ChangeStatusException(string message)
            : base(message)
        { }
    }
}

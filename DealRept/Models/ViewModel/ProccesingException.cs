using System;

namespace DealRept.Models.ViewModel
{
    public class ProccesingException:Exception
    {
        public ProccesingException()
        {
        }

        public ProccesingException(string message)
            : base(message)
        {
        }

        public ProccesingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

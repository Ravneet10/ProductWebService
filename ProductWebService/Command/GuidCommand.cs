using System;

namespace ProductWebService.Command
{
    public abstract class GuidCommand : Command
    {
        public Guid Id
        {
            get => Guid.Parse(CommandId);
            set => CommandId = value.ToString();
        }

        protected GuidCommand() : this(Guid.NewGuid())
        {
        }

        protected GuidCommand(Guid id)
        {
            Id = id;
        }
    }
}

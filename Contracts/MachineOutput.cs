namespace Contracts
{
    public class MachineOutput
    {
        public string Topic { get; set; }

        public string Ref { get; set; }

        public MachinePayload Payload { get; set; }

        public string Event { get; set; }
    }

    public class MachinePayload
    {
        public Guid Machine_id { get; set; }

        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public MachineStatus Status { get; set; }
    }

    public enum MachineStatus
    { 
        idle = 0,
        running = 5,
        finished = 10,
        errored = 15
    }
}
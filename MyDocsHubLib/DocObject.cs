using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDocsHubLib
{
    public class DocObject : BaseObject
    {
        public DocObject(DateOnly ReceivedDate, string Receiver, string Type, string Sender, string Subject, bool Outgoing = false)
        {
            this.ReceivedDate = ReceivedDate;
            this.Receiver = Receiver;
            this.Type = Type;
            this.Sender = Sender;
            this.Subject = Subject;
            this.Outgoing = Outgoing;
        }

        internal DocObject()
        {
        }

        public int FileSize { get; set; }

        public string FullPath { get; set; } = string.Empty;

        public bool Outgoing { get; set; }

        public DateOnly ReceivedDate { get; set; }

        public DateTime ReceivedDateAndTime { get => ReceivedDate.ToDateTime(new(12, 0, 0)); set => ReceivedDate = DateOnly.FromDateTime(value); }

        public string Receiver { get; set; } = string.Empty;

        public string Sender { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}
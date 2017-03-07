using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sselOnLine.AppCode
{
    public class MessageOptions
    {
        public bool DisableReply { get; set; }
        public bool Exclusive { get; set; }
        public bool AcknowledgeRequired { get; set; }
        public bool BlockAccess { get; set; }
        public int AccessCutoff { get; set; }
    }
}

using LNF.Models.Data;
using LNF.Repository;
using LNF.Repository.Data;
using LNF.Repository.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sselOnLine.AppCode
{
    public static class MessengerUtility
    {
        public static IList<Resource> GetTools()
        {
            return null;
        }

        public static IList<string> GetAreas()
        {
            return null;
        }

        public static IList<Client> GetManagers()
        {
            return null;
        }

        public static IList<Client> GetRecipientsByPriv(ClientPrivilege priv)
        {
            return null;
        }

        public static IList<Client> GetRecipientsByCommunity(int flag)
        {
            return null;
        }

        public static MessengerMessage CreateMessage(Client client, string subject, string body, MessengerMessage parent = null, MessageOptions options = null)
        {
            MessengerMessage mm = new MessengerMessage();
            mm.Client = client;
            mm.Subject = subject;
            mm.Body = body;
            mm.ParentID = (parent != null) ? parent.MessageID : 0;
            mm.Created = DateTime.Now;
            mm.Status = "Draft";
            if (options != null)
            {
                mm.DisableReply = options.DisableReply;
                mm.Exclusive = options.Exclusive;
                mm.AcknowledgeRequired = options.AcknowledgeRequired;
                mm.BlockAccess = options.BlockAccess;
                mm.AccessCutoff = options.AccessCutoff;
            }
            else
            {
                mm.DisableReply = false;
                mm.Exclusive = false;
                mm.AcknowledgeRequired = false;
                mm.BlockAccess = false;
                mm.AccessCutoff = 0;
            }
            DA.Current.SaveOrUpdate(mm);
            return mm;
        }

        public static void SendMessage(MessengerMessage message, IEnumerable<Client> recipients)
        {
            message.Sent = DateTime.Now;
            message.Status = "Sent";
            DA.Current.SaveOrUpdate(message);
            foreach (Client c in recipients)
            {
                MessengerRecipient mr = new MessengerRecipient();
                mr.Client = c;
                mr.Message = message;
                mr.Folder = "Inbox";
                mr.Received = DateTime.Now;
                mr.Acknowledged = null;
                mr.AccessCount = 0;
                DA.Current.SaveOrUpdate(mr);
            }
        }

        public static IList<MessengerRecipient> GetMessages(Client client, string folder)
        {
            return DA.Current.Query<MessengerRecipient>().Where(x => x.Client == client && x.Folder == folder && x.Acknowledged == null).ToList();
        }
    }
}

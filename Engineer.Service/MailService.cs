using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Engineer.Service
{
    public class MailService
    {
        private XmlDocument doc;
        private XmlNamespaceManager nsmgr;

        private static volatile MailService _instance;
        private static object syncRoot = new Object();



        public static MailService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {

                            _instance = new MailService();

                        }
                    }
                }

                return _instance;
            }
        }

        public void LoadFile(string xmlFilePath)
        {
            // Create an XML reader for this file.
            if (doc == null)
            {
                doc = new XmlDocument();
                doc.Load(xmlFilePath);

                if (nsmgr == null)
                {
                    //Create an XmlNamespaceManager for resolving namespaces.
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                }
            }
        }

        public DTOMessage GetMessage(string messageName)
        {
            DTOMessage message = null;
            // Compile a standard XPath expression
            if (doc == null || nsmgr == null)
            {
                return null;
            }

            XmlNode msgNode;

            msgNode = doc.DocumentElement.SelectSingleNode("/email-list/email[@name='" + messageName + "']");

            if (msgNode != null && msgNode.ChildNodes.Count == 2)
            {
                message = new DTOMessage(msgNode.ChildNodes[0].InnerText, msgNode.ChildNodes[1].InnerText);
                if (msgNode.Attributes["cc"] != null)
                {
                    string ccList = msgNode.Attributes["cc"].InnerText;
                    message.CcList = ccList.Split(new char[] { ',', ';' });
                }
            }
            return message;
        }
        /// <summary>

        /// Transmit an email message with attachments

        /// </summary>

        /// <param name="sendTo">Recipient Email Address</param>

        /// <param name="sendFrom">Sender Email Address</param>

        /// <param name="sendSubject">Subject Line Describing Message</param>

        /// <param name="bodyMessage">The Email Message Body</param>

        /// <param name="attachments">A string array pointing to the location  of each attachment</param>

        public static void SendMessageWithAttachment(string sendTo, string[] ccTO, string sendSubject, string bodyMessage, ArrayList attachments)
        {
            string sendFrom = (ConfigurationSettings.AppSettings["MailFromAddress"] != null) ? ConfigurationSettings.AppSettings["MailFromAddress"].ToString() : "";
            SendMessageWithAttachment(sendFrom, sendTo, ccTO, sendSubject, bodyMessage, attachments);
        }

        public static void SendMessageWithAttachment(string sendFrom, string sendTo, string[] ccTO, string sendSubject, string bodyMessage, ArrayList attachments)
        {
            MailMessage message = new MailMessage();

            if (sendFrom != "") message.From = new MailAddress(sendFrom);
            message.To.Add(new MailAddress(sendTo));
            if (ccTO != null)
            {
                foreach (string cc in ccTO)
                {
                    message.CC.Add(new MailAddress(cc));
                }
            }

            message.Subject = sendSubject;
            bodyMessage = "<FONT face=Calibri size=3>" + bodyMessage;
            bodyMessage += "</FONT>";
            message.Body = bodyMessage;
            message.IsBodyHtml = true;

            if (attachments != null)
            {
                foreach (string attach in attachments)
                {
                    Attachment attached = new Attachment(attach, MediaTypeNames.Application.Octet);
                    message.Attachments.Add(attached);

                }
            }

            SmtpClient client = new SmtpClient();

            // Add credentials
            var section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            client.UseDefaultCredentials = section.Network.DefaultCredentials;
            //client.Host = section.Network.Host;
            //client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);
            //client.EnableSsl = section.Network.EnableSsl;
            //client.Port = section.Network.Port;

            // send message
            client.Send(message);


        }
        public static String formatMsg(String message, params String[] messageArgs)
        {

            StringBuilder formatedMsg = new StringBuilder(message);
            String result;
            if(messageArgs != null)
            {
                for (int i = 0; i < messageArgs.Length; i++)
                {
                    result = Regex.Replace(formatedMsg.ToString(), @"\{" + i + @"\}", messageArgs[i]);
                    formatedMsg = new StringBuilder(result);
                }
            }
            return formatedMsg.ToString();


        }
    }
}

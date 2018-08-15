using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.Utils
{
	/// <summary>
	/// email functions
	/// </summary>
	public class EmailUtils
	{	
		/// <summary>
		/// Get the SMTP host
		/// </summary>
		public static string SmtpHost
		{
			get
			{
				SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
				string retVal = "";
				if (smtpSection != null && smtpSection.Network != null && smtpSection.Network.Host != null)
				{
					retVal = smtpSection.Network.Host;
				}
				return retVal;
			}
		}

		/// <summary>
		/// Get EMT email address
		/// </summary>
		public static string SenderEmailAddress
		{
			get
			{
				SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
				string retVal = "";
				if (smtpSection != null && smtpSection.From != null)
				{
					retVal = smtpSection.From;
				}
				return retVal;
			}
		}
		/// <summary>
		/// send error email
		/// </summary>
		/// <param name="ex"> the execption object</param>
		public static void SendErrorEmail(Exception ex)
		{
			// Need this try block because this method is usually called from exception handlers.
			// We don't want it to generate another exception here.
			try
			{
				string errName = "Error: ";
				string msgKey = ex.GetMessageFirstLineOnly();
				string subject = errName + msgKey;
				string body = errName + "\r\n" + ex.GetMessageVerbose(true, true, true);
				SendMail(ConfigurationManager.AppSettings.Get("SupportEmail"), subject, body, true);
			}
			catch (Exception)
			{
				// Suppress the exception. If it failed in here, we want it to continue.
			}
		}

		/// <summary>
		/// send email
		/// </summary>
		/// <param name="to">To email address string</param>
		/// <param name="subject">Email subject</param>
		/// <param name="body">Email body</param>
		/// <param name="appendServerInfo">Append the server info in email or not</param>
		/// <returns></returns>
		public static bool SendMail(string to, string subject, string body, bool appendServerInfo)
		{
			string errMsg = "";
			return SendMail(null, null, to, "", "", subject, body, appendServerInfo, false, null, out errMsg);
		}

		/// <summary>
		/// Send Email powerful function
		/// </summary>
		/// <param name="host">The host that sents the email</param>
		/// <param name="from">From Email Address string, the email address need to be wrap in angle brackets</param>
		/// <param name="to">To email address string</param>
		/// <param name="cc">CC Email Address string</param>
		/// <param name="bcc">BCC Email Address string</param>
		/// <param name="subject">Email Subject</param>
		/// <param name="body">Email Body</param>
		/// <param name="appendServerInfo">Append the server info in email or not</param>
		/// <param name="isBodyHtml">Is the email body html</param>
		/// <param name="attachments">email attachments</param>
		/// <param name="errorMessage">output error message</param>
		/// <param name="hideSentBy">Hide the sent by string</param>
		/// <returns></returns>
		public static bool SendMail(string host, string from, string to, string cc, string bcc, string subject, string body, bool appendServerInfo, bool isBodyHtml, List<Attachment> attachments, out string errorMessage, bool hideSentBy = false)
		{
			// cache the from address so that we can add footer to display where this email is sent from in the footer
			string origFrom = null;
			if (!String.IsNullOrEmpty(from) && from != SenderEmailAddress)
			{
				// extract the original sent address and put into origFrom for later display 
				origFrom = Regex.Match(from, "<(.*?)>").Groups[1].ToString();
				from = null;
			}
			
			// check some requiered params ( host, from, to or bcc)
			errorMessage = "";
			if (string.IsNullOrEmpty(host)) host = SmtpHost;
			if (string.IsNullOrEmpty(from)) from = SenderEmailAddress;

			List<string> missingParams = new List<string>();
			if (string.IsNullOrEmpty(host)) missingParams.Add("host");
			if (string.IsNullOrEmpty(from)) missingParams.Add("sender address");
			if (string.IsNullOrEmpty(to) && string.IsNullOrEmpty(bcc)) missingParams.Add("recipient address");
			 
			// display error message and return if missing any reqiured params
			if (missingParams.Count > 0)
			{
				errorMessage = string.Join(", ", missingParams.ToArray());
				int pos = errorMessage.LastIndexOf(", ");
				if (pos >= 0) errorMessage = errorMessage.Substring(0, pos + 1) + " and " + errorMessage.Substring(pos + 2, errorMessage.Length - pos - 2);
				errorMessage = "Missing " + errorMessage;
				return false;
			}

			try
			{
				SmtpClient smtpClient = new SmtpClient(host);

				StringBuilder sb = new StringBuilder(body);
				// append server information if needed
				if (appendServerInfo)
				{
					sb.AppendLine();
					sb.AppendLine("Web server:");
					sb.AppendLine(System.Environment.MachineName);
					sb.AppendLine();
				}

				MailMessage mm = new MailMessage();
				if (!string.IsNullOrEmpty(to))
				{
					mm.To.Add(to);
				}
				if (!string.IsNullOrEmpty(cc))
				{
					mm.CC.Add(cc);
				}
				if (!string.IsNullOrEmpty(bcc))
				{
					mm.Bcc.Add(bcc);
				}
				mm.From = new MailAddress(from);
				mm.Subject = subject;
				if (isBodyHtml)
				{
					mm.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
				}

				// append the email footer
				if (!String.IsNullOrEmpty(origFrom) && origFrom != SenderEmailAddress)
				{
					mm.ReplyToList.Add(origFrom);
					if (!hideSentBy)
					{
						sb.AppendLine(isBodyHtml ? String.Format("<p>Sent by {0}.</p>", origFrom) : String.Format("Sent by {0}.", origFrom));
					}
				}
				else
				{
					string bodyFooter = "*** This email message was sent from a notification-only address. Please do not reply to this email. ***";
					sb.AppendLine(isBodyHtml ? String.Format("<em style=\"color: #666\">{0}</em><hr />", bodyFooter) : String.Format("{0}", bodyFooter));
				}

				mm.Body = sb.ToString();
				mm.IsBodyHtml = isBodyHtml;

				// add attachments
				if (attachments != null)
				{
					for (int i = 0; i < attachments.Count; i++)
					{
						mm.Attachments.Add(attachments[i]);
					}
				}

				//send email
				smtpClient.Send(mm);
				return true;
			}
			catch (Exception ex) // catch and report any error
			{
				errorMessage = ex.Message;
				return false;
			}
		}
	}
}

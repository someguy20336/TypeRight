using System;
using System.Collections.Specialized;
using System.Text;

namespace TypeRight
{
	public static class CollectionExtensions
	{
        public static string ToQueryString(this NameValueCollection nvc)
        {
            if (nvc == null)
			{
				return string.Empty;
			}

			StringBuilder sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
				{
					continue;
				}

				string[] values = nvc.GetValues(key);
                if (values == null)
				{
					continue;
				}

				foreach (string value in values)
                {
                    sb.Append(sb.Length == 0 ? "?" : "&");
                    //sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));  // TODO... necessary?  Doesn't work in all cases (i.e. writing typescript)
                    sb.AppendFormat("{0}={1}", key, value);
                }
            }

            return sb.ToString();
        }
    }
}

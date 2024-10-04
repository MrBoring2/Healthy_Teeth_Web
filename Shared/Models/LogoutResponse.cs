using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
	public class LogoutResponse
	{
		/// <summary>
		/// Result
		/// </summary>
		public bool Success { get; set; }
		/// <summary>
		/// Error message
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}

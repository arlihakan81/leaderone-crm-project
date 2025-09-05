using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Leaderone.Application.Requests
{
	public class RegisterRequest : LoginRequest
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match. Please try again.")]
		public string ConfirmPassword { get; set; }
	}
}

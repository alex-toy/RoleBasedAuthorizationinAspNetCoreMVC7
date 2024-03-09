using System.ComponentModel.DataAnnotations;

namespace SecureAPI.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

    }
}

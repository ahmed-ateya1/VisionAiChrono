using VisionAiChrono.Application.Dtos.AuthenticationDtos;

namespace VisionAiChrono.Application.ServiceContract
{
    public interface IAuthenticationServices
    {

        Task<AuthenticationResponse> RegisterClientAsync(RegisterDTO clientRegisterDTO);

        Task<UserDto?> GetUserByIdAsync(Guid userID);
       
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);

        
        Task<AuthenticationResponse> RefreshTokenAsync(string token);

     
        Task<bool> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);

       
        Task<bool> RevokeTokenAsync(string token);

        
        Task<string> AddRoleToUserAsync(AddRoleDto model);

        
        Task<bool> DeleteAccountAsync(Guid userId);
    }
}

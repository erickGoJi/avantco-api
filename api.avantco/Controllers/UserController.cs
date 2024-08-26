
using api.avantco.ActionFilter;
using api.avantco.Model.ApiResponse;
using api.avantco.Model.Users;
using AutoMapper;
using biz.avantco.Entities;
using biz.avantco.Repository.IUsers;
using biz.avantco.Services.ILogger;
using Microsoft.AspNetCore.Mvc;

namespace api.avantco.Controllers;
public class UserController : ControllerBase
{
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;
    private readonly IUsers users_;

    public UserController(ILoggerManager logger, IMapper mapper, IUsers users_)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.users_ = users_;
    }

    //************************************************************//
    //INICIAMOS METODO POST PARA GUARDAR LA INFORMACION DE CLIENTE//
    [HttpPost("addUser")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]

    public ActionResult<ApiResponse<UsersGetDTO>> Create([FromBody] UsersDTO item)
    {
        var response = new ApiResponse<UsersGetDTO>();

        try
        {
            if (users_.Exists(c => c.Email == item.Email))
            {
                response.Success = false;
                response.Message = $"Email: { item.Email } Already Exists";
                return BadRequest(response);
            }

            var password_ = item.Password;
            item.Password = users_.HashPassword(password_);


            Users user = users_.Add(mapper.Map<Users>(item));
            response.Result = mapper.Map<UsersGetDTO>(user);
            //METODO PARA ENVIAR CORREO ELECTRONICO//
            var _user = mapper.Map<UsersDTO>(users_.Find(c => c.Email == item.Email));
            StreamReader reader = new StreamReader(Path.GetFullPath("TemplateEmail/Email.html"));
            string body = string.Empty;
            body = reader.ReadToEnd();
            body = body.Replace("{user}", _user.Name);
            body = body.Replace("{username}", $"{_user.Email}");
            users_.SendMail(_user.Email, body, "Avantco");

        
        
        }
        catch (Exception ex)
        {
            response.Result = null;
            response.Success = false;
            response.Message = ex.ToString();
            logger.LogError($"Something went wrong: { ex.ToString() }");
            return StatusCode(500, response);
        }

        return StatusCode(201, response);

    }
    //METODO LOGIN PARA INICIO DE SESION//
    [HttpPost("Login", Name = "Login")]
    public async Task<ActionResult<ApiResponse<UsersDTO>>> Login(string email, string password)
    {
        var response = new ApiResponse<UsersDTO>();

        try
        { 
            var _user = mapper.Map<UsersDTO>(users_.Find(c => c.Email == email));
            if (_user != null)
            {
                if (users_.VerifyPassword(_user.Password, password))
                {
                    var userData = mapper.Map<UsersDTO>(_user);
                    UsersDTO usersDto = new UsersDTO()
                    {
                        Id = userData.Id,
                        Name = userData.Name,
                        Lastname = userData.Lastname,
                        Email = userData.Email,
                        Password = userData.Password,
                        RecoveryPassword = userData.RecoveryPassword,
                        Active = userData.Active
                     };
                    response.Result = usersDto;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario y/o contraseña incorrecta";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Usuario y/o contraseña incorrecta";

            }

        }
        catch (Exception ex)
        {
            response.Result = null;
            response.Success = false;
            response.Message = "Internal server error";
            logger.LogError($"Something went wrong: { ex.ToString() }");
            return StatusCode(500, response);
        }

        return Ok(response);
    }

    [HttpPut("Recovery_password", Name = "Recovery_password")]
    public async Task<ActionResult<ApiResponse<UsersDTO>>> Recovery_password(string email)
    {
        var response = new ApiResponse<UsersDTO>();

        try
        {
            var _user = mapper.Map<Users>(users_.Find(c => c.Email == email));

            if (_user != null)
            {

                var guid = Guid.NewGuid().ToString().Substring(0, 7);
                var password = users_.HashPassword("$" + guid);

                _user.Password = password;

                users_.Update(mapper.Map<Users>(_user), _user.Id);

                StreamReader reader = new StreamReader(Path.GetFullPath("TemplateEmail/Email.html"));
                string body = string.Empty;
                body = reader.ReadToEnd();
                body = body.Replace("{user}", _user.Name + " " + _user.Lastname );
                body = body.Replace("{username}", _user.Email + " " + guid);
                //body = body.Replace("{pass}", "$" + guid);

                users_.SendMail(_user.Email, body, "Recovery password");

                response.Result = mapper.Map<UsersDTO>(_user);
                response.Success = true;
                response.Message = "success";
            }
            else
            {
                response.Success = false;
                response.Message = "Hubo un error";

            }

        }
        catch (Exception ex)
        {
            response.Result = null;
            response.Success = false;
            response.Message = ex.ToString();
            logger.LogError($"Something went wrong: { ex.ToString() }");
            return StatusCode(500, response);
        }

        return Ok(response);
    }

}
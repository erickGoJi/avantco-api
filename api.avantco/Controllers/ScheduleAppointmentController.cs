
using api.avantco.ActionFilter;
using api.avantco.Model.ApiResponse;
using api.avantco.Model.scheduleAppointment;
using AutoMapper;
using biz.avantco.Entities;
using biz.avantco.Repository.ScheduleAppointment;
using biz.avantco.Services.ILogger;
using dal.avantco.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace api.avantco.Controllers;
public class ScheduleAppointmentController : ControllerBase
{
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;
    private readonly IScheduleAppointmentrRepository scheduleRepository;

    public ScheduleAppointmentController(ILoggerManager logger, IMapper mapper, IScheduleAppointmentrRepository scheduleRepository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.scheduleRepository = scheduleRepository;
    }
   

    //************************************************************//
    //CONSULTA DE CLIENTE POR ID//
    [HttpGet("getAppointment/{id:int}")]
    public async Task<ActionResult<ApiResponse<SheduleAppointmentGet>>> Get(int id)
    {
        var response = new ApiResponse<SheduleAppointmentGet>();

        try
        {
            response.Result = mapper.Map<SheduleAppointmentGet>(await scheduleRepository.FindAsync(c => c.Id == id));
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
    //************************************************************//
    //INICIAMOS METODO POST PARA GUARDAR LA INFORMACION DE CLIENTE//
    [HttpPost("Appointment")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]

    public ActionResult<ApiResponse<SheduleAppointmentGet>> Create( [FromBody] SheduleAppointmentPost item)
    {
        var response = new ApiResponse<SheduleAppointmentGet>();

        try
        {
            if (scheduleRepository.Exists(c => c.Email == item.Email))
            {
                response.Success = false;
                response.Message = $"Email: { item.Email } Already Exists";
                return BadRequest(response);
            }


            ScheduleAppointment appointment = scheduleRepository.Add(mapper.Map<ScheduleAppointment>(item));
            response.Result = mapper.Map<SheduleAppointmentGet>(appointment);

            //METODO PARA ENVIAR CORREO ELECTRONICO//
            var _user = mapper.Map<SheduleAppointmentGet>(scheduleRepository.Find(c => c.Email == item.Email));
            StreamReader reader = new StreamReader(Path.GetFullPath("TemplateEmail/Email.html"));
            string body = string.Empty;
            body = reader.ReadToEnd();
            body = body.Replace("{user}", _user.Name );
            body = body.Replace("{username}", $"{_user.Email}");
            scheduleRepository.SendMail(_user.Email, body, "Avantco");

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
}

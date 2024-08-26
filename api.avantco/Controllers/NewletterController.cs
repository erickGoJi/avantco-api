
using api.avantco.ActionFilter;
using api.avantco.Model.ApiResponse;
using api.avantco.Model.Newsletter;
using AutoMapper;
using biz.avantco.Entities;
using biz.avantco.Repository.NewLetter;
using biz.avantco.Services.ILogger;
using Microsoft.AspNetCore.Mvc;

namespace api.avantco.Controllers;
public class NewletterController : ControllerBase
{
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;
    private readonly INewsLetter newsLetter;

    public NewletterController(ILoggerManager logger, IMapper mapper, INewsLetter newsLetter)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.newsLetter = newsLetter;
    }

    //************************************************************//
    //INICIAMOS METODO POST PARA GUARDAR LA INFORMACION DE CLIENTE//
    [HttpPost("Newsletter")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]

    public ActionResult<ApiResponse<NewsletterDTO>> Create([FromBody] NewsletterDTO item)
    {
        var response = new ApiResponse<NewsletterDTO>();

        try
        {
            if (newsLetter.Exists(c => c.Email == item.Email))
            {
                response.Success = false;
                response.Message = $"Email: { item.Email } Already Exists";
                return BadRequest(response);
            }

            Newsletter newsletter = newsLetter.Add(mapper.Map<Newsletter>(item));
            response.Result = mapper.Map<NewsletterDTO>(newsletter);

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

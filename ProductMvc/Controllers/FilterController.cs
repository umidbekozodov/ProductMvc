using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductMvc.Data;
using ProductMvc.Entities;

namespace ProductMvc.Controllers;

[Route("api[controller]")]
[ApiController]
public class FilterController : ControllerBase
{
    private readonly AppDbContext _context;

    public FilterController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] Filter filter)
    {
        var productAudits = await _context.Histories!.ToListAsync();

        var result = new List<Dtoes.History>();

        filter.StartDate ??= productAudits.Min(productAudit => productAudit.DateTime);
        filter.EndDate ??= productAudits.Max(productAudit => productAudit.DateTime);

        if(productAudits is null) return NotFound();

        foreach(var productAudit in productAudits)
        {
            var duration1 = ((TimeSpan)(productAudit.DateTime - filter.StartDate)).TotalSeconds;
            var duration2 = ((TimeSpan)(productAudit.DateTime - filter.EndDate)).TotalSeconds;

            if (duration1 >= 0 && duration2 <= 0)
            {
                var productAuditDto = new Dtoes.History()
                {
                    Id = productAudit.Id,
                    UserId = productAudit.UserId,
                    OldValues = productAudit.OldValues,
                    NewValues = productAudit.NewValues,
                    Changed = productAudit.Changed,
                    ProductId = productAudit.ProductId,
                    DateTime = productAudit.DateTime
                };

                result.Add(productAuditDto);
            }
        }

        var resultJson = JsonConvert.SerializeObject(result);
        
        return Ok(resultJson);
    }
}
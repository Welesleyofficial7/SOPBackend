using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SOPBackend.DTOs;
using SOPBackend.Services;
using System;
using SOPContracts.Dtos;

namespace SOPBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase, IPromotionApi
    {
        private readonly IPromotionService _promotionService;
        private readonly IMapper _mapper;

        public PromotionController(IPromotionService promotionService, IMapper mapper)
        {
            _promotionService = promotionService;
            _mapper = mapper;
        }

        [HttpGet("getAll", Name = "GetAllPromotions")]
        public IActionResult GetAllPromotions()
        {
            var promotions = _promotionService.GetAllPromotions().ToList();
            var promotionDtos = new List<PromotionDTO>();
            promotions.ForEach(c => promotionDtos.Add(_mapper.Map<PromotionDTO>(c)));
            
            if (promotions == null)
            {
                return NotFound();
            }

            return Ok(
                new
                {
                    self = new { href = Url.Link("GetAllPromotions", null) },
                    promotionDtos,
                    _actions = new
                    {
                        getAll = new
                        {
                            href = Url.Link("GetAllPromotions", null),
                            method = "GET",
                            rel = "Find all order items."
                        },
                        getById = new
                        {
                            href = Url.Link("GetPromotionsById", new { id = "someId" }),
                            method = "GET",
                            rel = "Find an order item by Id."
                        },
                        create = new
                        {
                            href = Url.Link("CreatePromotion", null),
                            method = "POST",
                            rel = "Create a new order item."
                        },
                        update = new
                        {
                            href = Url.Link("UpdatePromotion", new { id = "someId" }),
                            method = "PUT",
                            rel = "Update an existing order item by ID."
                        },
                        delete = new
                        {
                            href = Url.Link("DeletePromotion", new { id = "someId" }),
                            method = "DELETE",
                            rel = "Delete an order item by ID."
                        }
                    }
                }
            );
        }
        
        [HttpGet("getById/{id}", Name = "GetPromotionById")]
        public IActionResult GetPromotionById(Guid id)
        {
            var promotion = _promotionService.GetPromotionById(id);

            if (promotion == null)
            {
                return NotFound("Promotion not found.");
            }
            
            var promotionDto = _mapper.Map<PromotionDTO>(promotion);

            var promotionWithLinks = AddPromotionLinks(promotionDto, promotion.Id);
            return Ok(promotionWithLinks);
        }
        
        [HttpPost("create", Name = "CreatePromotion")]
        public IActionResult CreatePromotion([FromBody] PromotionRequest promotionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var newPromotion = _mapper.Map<Promotion>(promotionDto);
            
            var createdPromotion = _promotionService.CreatePromotion(newPromotion);

            if (createdPromotion == null)
            {
                return BadRequest("Promotion already exists.");
            }
            
            var createdPromotionDto = _mapper.Map<PromotionDTO>(createdPromotion);

            return CreatedAtRoute("GetPromotionById", new { id = createdPromotion.Id }, createdPromotionDto);
        }
        
        [HttpPut("update/{id}", Name = "UpdatePromotion")]
        public IActionResult UpdatePromotion(Guid id, [FromBody] PromotionRequest promotionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var updatedPromotion = _mapper.Map<Promotion>(promotionDto);
            
            var updatedEntity = _promotionService.UpdatePromotion(id, updatedPromotion);

            if (updatedEntity == null)
            {
                return NotFound("Promotion not found.");
            }
            
            var updatedPromotionDto = _mapper.Map<PromotionDTO>(updatedEntity);
            var promotionWithLinks = AddPromotionLinks(updatedPromotionDto, updatedEntity.Id);
            return Ok(promotionWithLinks);
        }
        
        [HttpDelete("delete/{id}", Name = "DeletePromotion")]
        public IActionResult DeletePromotion(Guid id)
        {
            var result = _promotionService.DeletePromotion(id);

            if (!result)
            {
                return NotFound("Promotion not found.");
            }

            return NoContent();
        }
        
        private object AddPromotionLinks(PromotionDTO promotion, Guid objId)
        {
            return new
            {
                promotion,
                _links = new
                {
                    self = new { href = Url.Link("GetPromotionById", new { id = objId }) },
                    update = new { href = Url.Link("UpdatePromotion", new { id = objId }) },
                    delete = new { href = Url.Link("DeletePromotion", new { id = objId }) },
                    all_promotions = new { href = Url.Link("GetAllPromotions", null) }
                }
            };
        }
    }
}

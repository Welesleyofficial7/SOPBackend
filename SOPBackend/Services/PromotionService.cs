using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPBackend.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly ApplicationContext _context;

        public PromotionService(ApplicationContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Promotion>? GetAllPromotions()
        {
            return _context.Promotions.ToList();
        }
        
        public Promotion? GetPromotionById(Guid id)
        {
            var promotion = _context.Promotions.Find(id);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }
            return promotion;
        }
        
        public Promotion? CreatePromotion(Promotion newPromotion)
        {
            var check = _context.Promotions.FirstOrDefault(p => p.Code == newPromotion.Code);

            if (check == null)
            {
                var entry = _context.Promotions.Add(newPromotion);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }
        
        public Promotion? UpdatePromotion(Guid id, Promotion updatedPromotion)
        {
            var promotion = _context.Promotions.Find(id);

            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }
            
            promotion.Code = updatedPromotion.Code;
            promotion.Discount = updatedPromotion.Discount;
            promotion.StartDate = updatedPromotion.StartDate;
            promotion.EndDate = updatedPromotion.EndDate;

            _context.Promotions.Update(promotion);
            _context.SaveChanges();

            return promotion;
        }
        
        public bool DeletePromotion(Guid id)
        {
            var promotion = _context.Promotions.Find(id);

            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                _context.SaveChanges();
                return true;
            }

            return false; 
        }
    }
}

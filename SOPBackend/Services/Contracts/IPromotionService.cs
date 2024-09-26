namespace SOPBackend.Services;

public interface IPromotionService
{
    IEnumerable<Promotion> GetAllPromotions();
    Promotion GetPromotionById(Guid id);
    Promotion? CreatePromotion(Promotion newPromotion);
    Promotion UpdatePromotion(Guid id, Promotion updatedPromotion);
    bool DeletePromotion(Guid id);
}
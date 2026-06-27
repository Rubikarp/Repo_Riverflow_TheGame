namespace RiverFlow
{
    /// <summary>Résout le besoin en eau (crans) d'un biome. Implémenté par un SO côté Unity, mocké en test.</summary>
    public interface IBiomeCatalog
    {
        int RequiredIrrigation(EBiome biome);
    }
}
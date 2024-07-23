namespace Fwbs.Office
{
    public interface IHydratable
    {
        bool IsHydrated { get; }

        void Hydrate();

        void Dehydrate();
    }
}

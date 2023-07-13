
namespace DreamedReality.Entities
{
    public interface ISpawnableEntity
    {
        public bool IsAlive { get; }

        public void Spawn();

        public void Kill();
    }
}

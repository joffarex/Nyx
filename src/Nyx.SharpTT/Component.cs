namespace Nyx.SharpTT
{
    public abstract class Component
    {
        public GameObject GameObject { get; set; }

        public void Start()
        {
        }

        public abstract void Update(float dt); // Delta time
    }
}
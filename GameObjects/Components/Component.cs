namespace Apprentice.GameObjects.Components
{
    class Component
    {
        public GameObject Parent { get; private set; }

        public Component(GameObject parent)
        {
            Parent = parent;
        }
    }
}

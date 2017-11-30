using Apprentice.GameObjects;
using Apprentice.GameObjects.Components;

namespace Apprentice.Spells
{
    abstract class Spell
    {
        // Guaranteed to have Caster component due to parameter of onAddedToKnownList
        public GameObject Owner { get; private set; }
        public string Name { get; private set; }

        public Spell(string name)
        {
            Name = name;
            Owner = null;
        }

        // Owner will cast the spell -- if the focus cost is sufficient, onCast gets called.
        public bool Cast()
        {
            if (Owner == null)
                throw new System.Exception("Cannot cast a spell that isn't owned, nobody to cast.");

            // TODO: Resource (mana/focus) checks here
            OnCast();
            return true;
        }

        // Implement to make the spell do its thing.
        // TODO: This will need to call Casted event when its done, methinks, or Cast will...
        abstract protected void OnCast();

        // Internal call to caster only.
        internal void onAddedToKnownList(Caster caster)
        {
            Owner = caster.Parent;
        }
    }
}

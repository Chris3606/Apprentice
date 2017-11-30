using Apprentice.Spells;
using System.Collections.Generic;

namespace Apprentice.GameObjects.Components
{
    // Might need to know about battery sources here, at least know that its parent is a battery (if not more).  FOr now
    // it should be easy till we get the mechanics down.
    class Caster : Component
    {
        private List<Spell> _knownSpells;
        public IList<Spell> KnownSpells { get => _knownSpells.AsReadOnly(); }

        public Caster(GameObject parent)
            : base(parent)
        {
            _knownSpells = new List<Spell>();
        }

        // TODO: Later these should be probably all predone. Just would call unlock.  if we use a skill tree. otherwise
        // instances may just be added.
        public void AddToLearned(Spell spell)
        {
            // TODO: handle duplicates
            _knownSpells.Add(spell);
            // TODO: Fire spellcasted event when this instnace casts
            spell.onAddedToKnownList(this);
        }

        public bool KnowsSpell(string name)
        {
            foreach (var spell in _knownSpells)
                if (spell.Name == name) return true;

            return false;
        }
    }
}

using RLNET;
using WinMan;

namespace Apprentice
{
    class SpellsPanel : Panel
    {
        static char startingChar = 'a';

        public SpellsPanel(ResizeCalc rootX, ResizeCalc rootY, ResizeCalc width, ResizeCalc height)
            : base(rootX, rootY, width, height, true, false) { }

        public override void UpdateLayout(object sender, UpdateEventArgs e)
        {
            console.Clear();

            console.Print(0, 0, "POWERS", RLColor.Red);
            if (ApprenticeGame.Player.Caster.KnownSpells.Count == 0)
            {
                console.Print(0, 1, "No powers known!", RLColor.White);
                return;
            }
            int y = 1;
            char curChar = startingChar;

            foreach (var spell in ApprenticeGame.Player.Caster.KnownSpells)
            {
                string strToPrint = curChar.ToString() + ") " + spell.Name;
                console.Print(0, y, strToPrint, RLColor.White);
                curChar = (char)(curChar + 1);
                y++;
            }
        }

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyPress.Key == RLKey.Escape)
            {
                e.Cancel = true;
                // TODO: This needs to be some sort of stack so that we cna pop off.
                ApprenticeGame.GameScreen.Show();
                Hide();
                return;
            }

            if (!e.KeyPress.Char.HasValue)
                return;

            int optionIndex = e.KeyPress.Char.Value - startingChar;
            if (optionIndex >= 0 && optionIndex < ApprenticeGame.Player.Caster.KnownSpells.Count)
            {
                Hide();
                ApprenticeGame.GameScreen.Show();
                ApprenticeGame.Player.Caster.KnownSpells[optionIndex].Cast();
                e.Cancel = true;
            }
        }
    }
}

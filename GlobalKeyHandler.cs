using WinMan;
using RLNET;

namespace Apprentice
{
    // Key handler that handles global key commands that apply nearly always, like fullscreen, exit, etc.
    class GlobalKeyHandler : KeyHandler
    {

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch(Input.ActionFor(e.KeyPress))
            {
                case InputAction.QUIT:
                    Engine.RootConsole.Close();
                    break;
                case InputAction.FULLSCREEN:
                    Engine.ToggleFullscreen();
                    break;
            }
        }
    }
}

using WinMan;

namespace Apprentice
{
    // Key handler that handles global key commands that apply nearly always, like fullscreen, exit, etc.
    class GlobalKeyHandler : KeyHandler
    {

        protected override void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch(e.KeyPress.Key)
            {
                case RLNET.RLKey.Q:
                case RLNET.RLKey.Escape:
                    Engine.RootConsole.Close();
                    break;
                case RLNET.RLKey.F11:
                    Engine.ToggleFullscreen();
                    break;

            }
        }
    }
}

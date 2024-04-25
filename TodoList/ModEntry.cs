using StardewModdingAPI;
using StardewValley;
using TodoList.UI.Menus;

namespace TodoList;

internal class ModEntry : Mod {
    public static IMonitor monitor;

    public override void Entry(IModHelper helper) {
        monitor = Monitor;

        helper.Events.Input.ButtonPressed += OnButtonPressed;
    }

    #region Events
    private void OnButtonPressed(object? sender, StardewModdingAPI.Events.ButtonPressedEventArgs e) {
        if (e.Button == SButton.L) {
            if (Game1.activeClickableMenu is TodoMenu todoMenu) {
                todoMenu.exitThisMenu();
            } else {
                Game1.activeClickableMenu = new TodoMenu();
            }
        }
    }
    #endregion
}
